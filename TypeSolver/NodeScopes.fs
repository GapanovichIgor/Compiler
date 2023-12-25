namespace TypeSolver

open System.Collections.Generic
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type private ScopeNode =
    { mutable parent: ScopeNode option
      children: List<Node * ScopeNode> }

    member this.Depth =
        match this.parent with
        | None -> 0
        | Some parent -> parent.Depth + 1

    member this.Node =
        match this.parent with
        | None -> None
        | Some parent ->
            parent.children
            |> Seq.choose (fun (n, s) -> if s = this then Some n else None)
            |> Seq.exactlyOne
            |> Some

    member this.SubtreeContains(child: ScopeNode) =
        let rec findChild scope =
            scope.children
            |> Seq.exists (fun (_, s) ->
                if s = child then
                    true
                else
                    findChild s)

        findChild this

    // member this.ContainedInSubtreeOf(parent: ScopeNode) =
    //     let rec findParent node =
    //         match node.parent with
    //         | Some p ->
    //             if p = parent then
    //                 true
    //             else
    //                 findParent p
    //         | None ->
    //             false
    //
    //     findParent this

type internal NodeScopes() =
    let rootScope =
        { parent = None
          children = List() }

    let tryFindOwnedScope typeNode =
        let rec findScope scopeNode =
            scopeNode.children
            |> Seq.choose (fun (n, s) ->
                if n = typeNode then
                    Some s
                else
                    findScope s)
            |> Seq.tryHead

        findScope rootScope

    let tryFindParentScope typeNode =
        tryFindOwnedScope typeNode
        |> Option.bind (fun s -> s.parent)

    member _.Scope(parent: Node, child: Node): unit =
        match tryFindOwnedScope parent with
        | None -> failwith "Container is not in the scope tree"
        | Some parentScope ->
            match tryFindOwnedScope child with
            | None ->
                let childScope =
                    { parent = Some parentScope
                      children = List() }

                parentScope.children.Add(child, childScope)
            | Some childScope ->
                if parentScope.SubtreeContains(childScope) then
                    childScope.parent.Value.children.RemoveAll(fun (_, s) -> s = childScope) |> ignore
                    childScope.parent <- Some parentScope
                    parentScope.children.Add(child, childScope)
                elif not (childScope.parent.Value.SubtreeContains(parentScope)) then
                    failwith "Contained node is in a different branch"

    member _.ScopeRoot(child: Node): unit =
        match tryFindOwnedScope child with
        | None ->
            let scopeNode =
                { parent = Some rootScope
                  children = List() }

            rootScope.children.Add(child, scopeNode)
        | Some ownedScope ->
            let parent = ownedScope.parent |> Option.get
            parent.children.RemoveAll(fun (_, s) -> s = ownedScope) |> ignore

            ownedScope.parent <- Some rootScope
            rootScope.children.Add(child, ownedScope)

    member _.Unscope(node: Node): unit =
        match tryFindParentScope node with
        | None -> ()
        | Some parentScope ->
            parentScope.children.RemoveAll(fun (n, _) -> n = node) |> ignore

    member _.GetChildNodes(parent: Node): Node list =
        match tryFindOwnedScope parent with
        | None -> []
        | Some ownedScope ->
            ownedScope.children
            |> Seq.map fst
            |> List.ofSeq

    member _.TryGetParentNode(child: Node): Node option =
        match tryFindParentScope child with
        | None -> None
        | Some parentScope -> parentScope.Node

    override _.ToString() =
        let rec toString indentation node scope =
            let str = $"{indentation}{node}"
            let indentation = indentation + "   "

            let children =
                scope.children
                |> Seq.map (fun (n, s) -> toString indentation n s)

            Seq.append [ str ] children
            |> String.concat "\n"

        rootScope.children
        |> Seq.map (fun (n, s) -> toString "" n s)
        |> String.concat "\n"