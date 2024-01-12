namespace TypeSolver

open System.Collections.Generic

type internal ScopeMap<'a when 'a : equality>() =
    let dict = Dictionary<'a, ScopeId>()

    member _.AddToScope(scopeId: ScopeId, x: 'a) =
        dict.Add(x, scopeId)

    member _.GetScope(x: 'a) = dict[x]

    member _.TryGetScope(x: 'a) =
        match dict.TryGetValue(x) with
        | false, _ -> None
        | true, scope -> Some scope

    member _.GetValuesInScope(scopeId: ScopeId) =
        dict
        |> Seq.choose (fun kv ->
            if kv.Value = scopeId then
                Some kv.Key
            else
                None)
        |> List.ofSeq