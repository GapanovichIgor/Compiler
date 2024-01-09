namespace TypeSolver

open System
open System.Collections.Generic

type internal MonomorphicNodeProperty() =
    let scopes = Dictionary<ScopeId, HashSet<Node>>()

    member _.Set(scopeId: ScopeId, node: Node): bool =
        let scope =
            match scopes.TryGetValue(scopeId) with
            | true, scope -> scope
            | false, _ ->
                let scope = HashSet()
                scopes.Add(scopeId, scope)
                scope

        scope.Add(node)

    member _.Unset(scopeId: ScopeId, node: Node): unit =
        match scopes.TryGetValue(scopeId) with
        | false, _ -> ()
        | true, scope ->
            scope.Remove(node) |> ignore

    member _.IsMonomorphic(scopeId: ScopeId, node: Node): bool =
        match scopes.TryGetValue(scopeId) with
        | false, _ -> false
        | true, scope -> scope.Contains(node)

    member _.GetScopesWhereMonomorphic(node: Node) : ScopeId list =
        scopes
        |> Seq.choose (fun kv ->
            if kv.Value.Contains(node) then
                Some kv.Key
            else
                None)
        |> List.ofSeq