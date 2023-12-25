namespace TypeSolver

open System.Collections.Generic
open Common

type internal ScopeTree() =
    let stack = Stack<List<TypeReference>>()
    let rootScope = List<TypeReference>()
    let subScopes = Dictionary<Identifier, List<TypeReference>>()

    member _.Push(identifier: Identifier) =
        if subScopes.ContainsKey(identifier) then
            failwith "Identifier already has a scope"

        let scope = List()
        subScopes.Add(identifier, scope)
        stack.Push(scope)

    member _.Pop() = stack.Pop() |> ignore

    member _.Add(typeReference: TypeReference) =
        if stack.Count = 0 then
            rootScope.Add(typeReference)
        else
            stack.Peek().Add(typeReference)
