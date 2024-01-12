namespace TypeSolver

open System.Collections.Generic

type internal ScopeMap<'a when 'a : equality>() =
    let dictionary = Dictionary<'a, ScopeId>()

    member _.AddToScope(scopeId: ScopeId, x: 'a) =
        dictionary.Add(x, scopeId)

    member _.GetScope(x: 'a) = dictionary[x]

    member _.TryGetScope(x: 'a) =
        match dictionary.TryGetValue(x) with
        | false, _ -> None
        | true, scope -> Some scope