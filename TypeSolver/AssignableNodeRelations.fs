namespace TypeSolver

open System
open System.Collections.Generic
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type internal AssignableNodeRelations() =
    let scopes = Dictionary<Guid, Dictionary<Node, Node>>()

    let validateCanSet (target, assignee) =
        if target = assignee then
            failwith "Target and assignee are the same node"

        let assigneeHasAnotherTarget =
            scopes.Values
            |> Seq.collect id
            |> Seq.exists (fun kv -> kv.Value = assignee && kv.Key <> target)

        if assigneeHasAnotherTarget then
            failwith "Assignee already has another target"

    member _.Set(scopeId: Guid, target: Node, assignee: Node) : unit =
        validateCanSet (target, assignee)

        let scope =
            match scopes.TryGetValue(scopeId) with
            | true, scope -> scope
            | false, _ ->
                let scope = Dictionary()
                scopes.Add(scopeId, scope)
                scope

        scope[assignee] <- target

    member _.Unset(scopeId: Guid, target: Node, assignee: Node) : unit =
        match scopes.TryGetValue(scopeId) with
        | false, _ -> failwith "Invalid scopeId"
        | true, scope ->
            match scope.TryGetValue(assignee) with
            | true, t when t = target ->
                scope.Remove(assignee) |> ignore
            | _ -> failwith "Invalid target-assignee pair"

    member _.GetAssignees(scopeId: Guid, target: Node): Node list =
        match scopes.TryGetValue(scopeId) with
        | false, _ -> []
        | true, scope ->
            scope
            |> Seq.choose (fun kv ->
                if kv.Value = target then
                    Some kv.Key
                else
                    None)
            |> List.ofSeq

    member _.GetAssignees(target: Node) : (Guid * Node) list =
        scopes
        |> Seq.choose (fun kv ->
            let scopeId = kv.Key
            let scope = kv.Value

            scope
            |> Seq.choose (fun kv ->
                if kv.Value = target then
                    Some (scopeId, kv.Key)
                else
                    None)
            |> Seq.tryExactlyOne)
        |> List.ofSeq

    member _.TryGetTarget(assignee: Node) : (Guid * Node) option =
        scopes
        |> Seq.choose (fun kv ->
            let scopeId = kv.Key
            let scope = kv.Value

            match scope.TryGetValue(assignee) with
            | true, target -> Some (scopeId, target)
            | false, _ -> None)
        |> Seq.tryExactlyOne

    override _.ToString() =
        scopes.Values
        |> Seq.map (fun group -> group |> Seq.map (fun kv -> $"{kv.Value} <- {kv.Key}") |> String.concat "\n")
        |> String.concat "\n---\n"
