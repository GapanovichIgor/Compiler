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

    member this.IsSubtreeOf(parent: ScopeNode) =
        let rec findParent node =
            match node.parent with
            | Some p ->
                if p = parent then
                    true
                else
                    findParent p
            | None ->
                false

        findParent this

type internal NodeScopes() =
    let rootScope =
        { parent = None
          children = List() }

    let tryFindScopeNode typeNode =
        let rec findScope scopeNode =
            scopeNode.children
            |> Seq.choose (fun (n, s) ->
                if n = typeNode then
                    Some s
                else
                    findScope s)
            |> Seq.tryHead

        findScope rootScope

    member _.Scope(container: Node, contained: Node): unit =
        match tryFindScopeNode container with
        | None -> failwith "Container is not in the scope tree"
        | Some containerScopeNode ->
            match tryFindScopeNode contained with
            | None ->
                let scopeNode =
                    { parent = Some containerScopeNode
                      children = List() }

                containerScopeNode.children.Add(contained, scopeNode)
            | Some containedScopeNode ->
                if containedScopeNode.IsSubtreeOf(containerScopeNode) then
                    let parent = containedScopeNode.parent |> Option.get
                    parent.children.RemoveAll(fun (_, s) -> s = containedScopeNode) |> ignore

                    containedScopeNode.parent <- Some containerScopeNode
                    containerScopeNode.children.Add(contained, containedScopeNode)
                elif not (containerScopeNode.IsSubtreeOf(containedScopeNode)) then
                    failwith "Contained node is in a different branch"

    member _.ScopeRoot(contained: Node): unit =
        match tryFindScopeNode contained with
        | None ->
            let scopeNode =
                { parent = Some rootScope
                  children = List() }

            rootScope.children.Add(contained, scopeNode)
        | Some containedScopeNode ->
            let parent = containedScopeNode.parent |> Option.get
            parent.children.RemoveAll(fun (_, s) -> s = containedScopeNode) |> ignore

            containedScopeNode.parent <- Some rootScope
            rootScope.children.Add(contained, containedScopeNode)

    member _.Unscope(contained: Node): unit =
        match tryFindScopeNode contained with
        | None -> ()
        | Some containerScope ->
            containerScope.children.RemoveAll(fun (n, _) -> n = contained) |> ignore

    member _.GetContainedNodes(container: Node): Node list =
        match tryFindScopeNode container with
        | None -> []
        | Some containerScope ->
            containerScope.children
            |> Seq.map fst
            |> List.ofSeq

    member _.TryGetContainerNode(contained: Node): Node option =
        match tryFindScopeNode contained with
        | None -> None
        | Some containedScope ->
            containedScope.parent
            |> Option.bind (fun parent ->
                parent.parent
                |> Option.bind (fun parentParent ->
                    parentParent.children
                    |> Seq.choose(fun (n, s) ->
                        if s = parent then
                            Some n
                        else
                            None)
                    |> Seq.tryHead))

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