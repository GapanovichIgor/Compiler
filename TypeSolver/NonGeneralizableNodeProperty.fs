namespace TypeSolver

open System
open System.Collections.Generic

type internal NonGeneralizableNodeProperty() =
    let scopes = Dictionary<Guid, HashSet<Node>>()

    member _.Set(scopeId: Guid, node: Node): bool =
        let scope =
            match scopes.TryGetValue(scopeId) with
            | true, scope -> scope
            | false, _ ->
                let scope = HashSet()
                scopes.Add(scopeId, scope)
                scope

        scope.Add(node)

    member _.Unset(scopeId: Guid, node: Node): unit =
        match scopes.TryGetValue(scopeId) with
        | false, _ -> ()
        | true, scope ->
            scope.Remove(node) |> ignore

    member _.IsNotGeneralizable(scopeId: Guid, node: Node): bool =
        match scopes.TryGetValue(scopeId) with
        | false, _ -> false
        | true, scope -> scope.Contains(node)

    member _.GetScopesWhereNotGeneralizable(node: Node) : Guid list =
        scopes
        |> Seq.choose (fun kv ->
            if kv.Value.Contains(node) then
                Some kv.Key
            else
                None)
        |> List.ofSeq