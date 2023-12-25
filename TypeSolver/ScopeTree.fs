namespace TypeSolver

open System.Collections.Generic
open Common

type internal ScopeTreeInfo =
    { globallyScopedAtomTypes: AtomTypeId list
      identifierScopedAtomTypes: Map<Identifier, AtomTypeId list> }

type internal ScopeTree() =
    let stack = Stack<Identifier * List<TypeReference>>()
    let rootScope = List<TypeReference>()
    let subScopes = Dictionary<Identifier, List<TypeReference>>()
    let parentMap = Dictionary<Identifier, Identifier>()

    member _.Push(identifier: Identifier) =
        if subScopes.ContainsKey(identifier) then
            failwith "Identifier already has a scope"

        if stack.Count > 0 then
            let parentIdentifier, _ = stack.Peek()
            parentMap.Add(identifier, parentIdentifier)

        let scope = List()
        subScopes.Add(identifier, scope)
        stack.Push(identifier, scope)

    member _.Pop() = stack.Pop() |> ignore

    member _.Add(typeReference: TypeReference) =
        if stack.Count = 0 then
            rootScope.Add(typeReference)
        else
            let _, scope = stack.Peek()
            scope.Add(typeReference)

    member _.GetResult(typeReferenceTypes: Map<TypeReference, Type>) : ScopeTreeInfo =
        // let atomTypeMap =
        //     typeReferenceTypes
        //     |> Map.toSeq
        //     |> Seq.map (fun (tr, t) ->
        //         let rec collectAtomTypes t =
        //             match t with
        //             | AtomType a -> [ a ]
        //             // | FunctionType (p, r) -> collectAtomTypes p @ collectAtomTypes r
        //             | FunctionType _ -> []
        //             | QualifiedType _ -> failwith "Unexpected qualified type"
        //         tr, collectAtomTypes t)
        //     |> Map.ofSeq

        let globallyScopedAtomTypes =
            rootScope
            |> Seq.choose (fun tr ->
                match typeReferenceTypes |> Map.find tr with
                | AtomType atomTypeId -> Some atomTypeId
                | _ -> None)
            |> List.ofSeq
            |> List.distinct

        let identifierScopedAtomTypes =
            subScopes
            |> Seq.choose (fun kv ->
                let atomTypes =
                    kv.Value
                    |> Seq.collect (fun tr ->
                        let rec collectAtomTypes t =
                            match t with
                            | AtomType a -> [ a ]
                            | FunctionType (p, r) -> collectAtomTypes p @ collectAtomTypes r
                            | QualifiedType _ -> failwith "Unexpected qualified type"
                        typeReferenceTypes[tr]
                        |> collectAtomTypes)
                    |> List.ofSeq
                    |> List.distinct

                if atomTypes.IsEmpty then
                    None
                else
                    Some (kv.Key, atomTypes))
            |> Map.ofSeq

        let rec containedInParents atomTypeId identifier =
            match parentMap.TryGetValue(identifier) with
            | false, _ -> globallyScopedAtomTypes |> List.contains atomTypeId
            | true, parent ->
                let containedInParent =
                    match identifierScopedAtomTypes |> Map.tryFind parent with
                    | None -> false
                    | Some parentAtomTypes -> parentAtomTypes |> List.contains atomTypeId

                if containedInParent then
                    true
                else
                    containedInParents atomTypeId parent

        let identifierScopedAtomTypes =
            identifierScopedAtomTypes
            |> Map.toSeq
            |> Seq.choose (fun (i, atomTypes) ->
                let atomTypes = atomTypes |> List.filter (fun a -> not (containedInParents a i))
                if atomTypes.IsEmpty then
                    None
                else
                    Some (i, atomTypes))
            |> Map.ofSeq

        { globallyScopedAtomTypes = globallyScopedAtomTypes
          identifierScopedAtomTypes = identifierScopedAtomTypes }
