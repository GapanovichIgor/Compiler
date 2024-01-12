namespace rec TypeSolver

open System.Collections.Generic
open Common

type private Operation =
    | Merge of a: Node * b: Node
    | SetMonomorphic of scope: ScopeId * node: Node
    | SetFunction of func: Node * parameter: Node * result: Node
    | SetApplicationTypeSubstitution of applicationId: ApplicationId * placeholder: Node * substitution: Node

type private OperationOutcome =
    { followupOperations: Operation list
      nodeSubstitutions: Map<Node, Node> }

    static member Empty =
        { followupOperations = List.empty
          nodeSubstitutions = Map.empty }

    static member Followup(followupOperations: #seq<Operation>) =
        { followupOperations = followupOperations |> List.ofSeq
          nodeSubstitutions = Map.empty }

type TypeGraphInfo =
    { typeReferenceTypes: Map<TypeReference, Type> }

type TypeGraph() =
    let typeReferenceNodes = Dictionary<TypeReference, Node>()

    let typeReferenceScopes = ScopeMap<TypeReference>()
    let applicationScopes = ScopeMap<ApplicationId>()

    let functions = FunctionNodeRelations()
    let applications = ApplicationNodeRelations()
    let monomorphic = MonomorphicNodeProperty()

    let createNode =
        let mutable newNodeId = 1

        fun () ->
            let node = newNodeId
            newNodeId <- newNodeId + 1
            node

    let getNode (typeRef: TypeReference) =
        match typeReferenceNodes.TryGetValue(typeRef) with
        | true, node -> node
        | false, _ ->
            let node = createNode ()
            typeReferenceNodes.Add(typeRef, node)
            node

    let rec runOperations (operations: Operation list) =
        let operationSet = HashSet operations

        let rebuildSet (nodeSubstitutions: Map<Node, Node>) =
            if not nodeSubstitutions.IsEmpty then
                let replace node =
                    match nodeSubstitutions |> Map.tryFind node with
                    | Some newNode -> newNode
                    | None -> node

                let oldOperations = operationSet |> List.ofSeq

                operationSet.Clear()

                for operation in oldOperations do
                    let operation =
                        match operation with
                        | Merge (a, b) -> Merge (replace a, replace b)
                        | SetMonomorphic (scope, node) -> SetMonomorphic (scope, replace node)
                        | SetFunction (func, param, result) -> SetFunction (replace func, replace param, replace result)
                        | SetApplicationTypeSubstitution (appId, p, s) -> SetApplicationTypeSubstitution (appId, replace p, replace s)

                    operationSet.Add(operation) |> ignore

        let getHighestPriorityOperation operations =
            operations
            |> Seq.sortBy (function
                | Merge _ -> 1
                | SetMonomorphic _ -> 2
                | SetFunction _ -> 3
                | SetApplicationTypeSubstitution _ -> 4)
            |> Seq.head

        while operationSet.Count > 0 do
            let op = getHighestPriorityOperation operationSet

            operationSet.Remove(op) |> ignore

            let outcome =
                match op with
                | Merge (a, b) -> merge (a, b)
                | SetMonomorphic (scope, node) -> setMonomorphic (scope, node)
                | SetFunction (func, param, result) -> setFunction (func, param, result)
                | SetApplicationTypeSubstitution (appId, p, s) -> setApplicationTypeSubstitution (appId, p, s)

            for operation in outcome.followupOperations do
                operationSet.Add(operation) |> ignore

            rebuildSet outcome.nodeSubstitutions
    and runOperation (operation: Operation) =
        runOperations [operation]

    and merge (a: Node, b: Node) =
        if a = b then
            OperationOutcome.Empty
        else
            let followupOperations = List()

            // Replace in type reference to node map
            for kv in typeReferenceNodes |> List.ofSeq do
                if kv.Value = a then
                    typeReferenceNodes[kv.Key] <- b

            // Merge functions
            let paramResultA = functions.TryGetParamResultOfFunction(a)
            let functionsWithParameterA = functions.GetFunctionsOfParameter(a)
            let functionsWithResultA = functions.GetFunctionsOfResult(a)
            functions.RemoveWhereFunction(a)
            functions.RemoveWhereParameter(a)
            functions.RemoveWhereResult(a)

            // Merge functions where a is the function
            match paramResultA, functions.TryGetParamResultOfFunction(b) with
            | None, _ -> ()
            | Some (aParam, aResult), None ->
                followupOperations.Add(SetFunction (b, aParam, aResult))
            | Some (aParam, aResult), Some (bParam, bResult) ->
                followupOperations.Add(Merge (aParam, bParam))
                followupOperations.Add(Merge (aResult, bResult))

            // Merge functions where a is parameter
            for func, _, result in functionsWithParameterA do
                followupOperations.Add(SetFunction (func, b, result))

            // Merge functions where a is result
            for func, param, _ in functionsWithResultA do
                followupOperations.Add(SetFunction (func, param, b))

            // Merge applications

            // For every application where a is placeholder, change it to b
            for appId, substitution in applications.GetSubstitutions(a) do
                applications.RemovePlaceholder(appId, a)
                followupOperations.Add(SetApplicationTypeSubstitution (appId, b, substitution))

            // For every application where a is substitution, change it to b
            for appId, placeholder in applications.GetPlaceholders(a) do
                applications.RemovePlaceholder(appId, placeholder)
                followupOperations.Add(SetApplicationTypeSubstitution (appId, placeholder, b))

            // Merge monomorphic
            for aScope in monomorphic.GetScopesWhereMonomorphic(a) do
                monomorphic.Unset(aScope, a)
                followupOperations.Add(SetMonomorphic(aScope, b))

            { followupOperations = followupOperations |> List.ofSeq
              nodeSubstitutions = Map.ofList [ a, b ] }

    and setMonomorphic (scope: ScopeId, node: Node) =
        let followupOperations = List()

        match functions.TryGetParamResultOfFunction(node) with
        | Some (p, r) ->
            followupOperations.Add(SetMonomorphic (scope, p))
            followupOperations.Add(SetMonomorphic (scope, r))
        | None ->
            if monomorphic.Set(scope, node) then
                for appId in applicationScopes.GetValuesInScope(scope) do
                    match applications.TryGetSubstitution(appId, node) with
                    | Some substitution ->
                        followupOperations.Add(Merge (substitution, node))
                        applications.RemovePlaceholder(appId, node)
                    | None -> ()

        OperationOutcome.Followup(followupOperations)

    and setApplicationTypeSubstitution (appId: ApplicationId, placeholder: Node, substitution: Node): OperationOutcome =
        let scope = applicationScopes.GetScope(appId)

        if monomorphic.IsMonomorphic(scope, placeholder) then
            OperationOutcome.Followup([ Merge (placeholder, substitution) ])
        else
            match applications.TryGetSubstitution(appId, placeholder) with
            | Some existingSubstitution when substitution = existingSubstitution ->
                OperationOutcome.Empty
            | Some existingSubstitution ->
                OperationOutcome.Followup([ Merge (substitution, existingSubstitution) ])
            | None ->
                let followupOperations = List()

                match functions.TryGetParamResultOfFunction(placeholder) with
                | None ->
                    applications.SetSubstitution(appId, placeholder, substitution)
                | Some (placeholderParam, placeholderResult) ->
                    let substitutionParam, substitutionResult =
                        match functions.TryGetParamResultOfFunction(substitution) with
                        | Some (p, r) -> p, r
                        | None ->
                            let p = createNode ()
                            let r = createNode ()
                            followupOperations.Add(SetFunction (substitution, p, r))
                            p, r

                    match applications.TryGetSubstitution(appId, placeholderParam) with
                    | Some existingSubstitutionParam ->
                        if substitutionParam <> existingSubstitutionParam then
                            followupOperations.Add(Merge (substitutionParam, existingSubstitutionParam))
                    | None ->
                        applications.SetSubstitution(appId, placeholderParam, substitutionParam)

                    match applications.TryGetSubstitution(appId, placeholderResult) with
                    | Some existingSubstitutionResult ->
                        if substitutionResult <> existingSubstitutionResult then
                            followupOperations.Add(Merge (substitutionResult, existingSubstitutionResult))
                    | None ->
                        applications.SetSubstitution(appId, placeholderResult, substitutionResult)

                OperationOutcome.Followup(followupOperations)

    and setFunction (func: Node, param: Node, result: Node): OperationOutcome =
        let followupOperations = List()

        let anotherFunctionsOfSameParamResult = functions.TryGetFunctionOfParamResult(param, result)

        match anotherFunctionsOfSameParamResult with
        | Some anotherFunc ->
            followupOperations.Add(Merge (func, anotherFunc))
        | None ->
            match functions.TryGetParamResultOfFunction(func) with
            | Some (anotherParam, anotherResult) ->
                if anotherParam <> param then
                    followupOperations.Add(Merge (param, anotherParam))

                if anotherResult <> result then
                    followupOperations.Add(Merge (result, anotherResult))
            | None ->
                if functions.Set(func, param, result) then
                    for scope in monomorphic.GetScopesWhereMonomorphic(func) do
                        followupOperations.Add(SetMonomorphic (scope, param))
                        followupOperations.Add(SetMonomorphic (scope, result))
                        monomorphic.Unset(scope, func)

                    for appId, substitution in applications.GetSubstitutions(func) do
                        let substitutionParam, substitutionResult =
                            match functions.TryGetParamResultOfFunction(substitution) with
                            | Some (substitutionParam, substitutionResult) -> substitutionParam, substitutionResult
                            | None ->
                                let substitutionParam = createNode ()
                                let substitutionResult = createNode ()
                                followupOperations.Add(SetFunction (substitution, substitutionParam, substitutionResult))
                                substitutionParam, substitutionResult

                        followupOperations.Add(SetApplicationTypeSubstitution (appId, param, substitutionParam))
                        followupOperations.Add(SetApplicationTypeSubstitution (appId, result, substitutionResult))
                        applications.RemovePlaceholder(appId, func)

        OperationOutcome.Followup(followupOperations)

    member _.Function(fn: TypeReference, param: TypeReference, result: TypeReference) =
        runOperation (SetFunction (getNode fn, getNode param, getNode result))

    member _.Application(scope: ScopeId, appId: ApplicationId, fn: TypeReference, argument: TypeReference, result: TypeReference) =
        applicationScopes.AddToScope(scope, appId)

        let fnNode = getNode fn

        let placeholderParam, placeholderResult, operations =
            match functions.TryGetParamResultOfFunction(fnNode) with
            | Some (p, r) -> p, r, []
            | None ->
                let p = createNode ()
                let r = createNode ()
                p, r, [ SetFunction (fnNode, p, r) ]

        let operations =
            SetApplicationTypeSubstitution (appId, placeholderParam, getNode argument) ::
            SetApplicationTypeSubstitution (appId, placeholderResult, getNode result) ::
            operations

        runOperations operations

    member _.Identical(a: TypeReference, b: TypeReference) =
        runOperation (Merge(getNode a, getNode b))

    member _.Monomorphic(scope: ScopeId, typeReference: TypeReference) =
        runOperation (SetMonomorphic (scope, getNode typeReference))

    member _.DefinedInScope(scopeId: ScopeId, typeReference: TypeReference) =
        typeReferenceScopes.AddToScope(scopeId, typeReference)

    member _.GetResult(): TypeGraphInfo =
        let nodeInfo = Dictionary<Node, Type * (Node * AtomTypeId) list>()

        let rec getNodeInfo node =
            match nodeInfo.TryGetValue(node) with
            | true, i -> i
            | false, _ ->
                let type_ =
                    match functions.TryGetParamResultOfFunction(node) with
                    | None ->
                        let atomTypeId = AtomTypeId()

                        AtomType atomTypeId, [node, atomTypeId]
                    | Some (param, result) ->
                        let paramType, paramAtomTypes = getNodeInfo param
                        let resultType, resultAtomTypes = getNodeInfo result

                        FunctionType (paramType, resultType), (paramAtomTypes @ resultAtomTypes |> List.distinct)

                nodeInfo.Add(node, type_)

                type_

        let typeReferenceTypes =
            typeReferenceNodes
            |> Seq.map (fun kv ->
                let typeReference = kv.Key
                let node = kv.Value

                let type_, atomTypes = getNodeInfo node

                let typeParameters =
                    match typeReferenceScopes.TryGetScope(typeReference) with
                    | None -> []
                    | Some scope ->
                        atomTypes
                        |> List.choose (fun (node, atomTypeId) ->
                            if monomorphic.IsMonomorphic(scope, node) then
                                None
                            else
                                Some atomTypeId)

                let type_ =
                    if typeParameters.Length > 0 then
                        QualifiedType (typeParameters, type_)
                    else
                        type_

                typeReference, type_)
            |> Map.ofSeq

        { typeReferenceTypes = typeReferenceTypes }
