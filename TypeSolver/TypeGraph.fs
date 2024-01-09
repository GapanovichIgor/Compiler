namespace rec TypeSolver

open System.Collections.Generic
open System.Text
open Common

type private Operation =
    | Merge of a: Node * b: Node
    | SetMonomorphic of scope: ScopeId * node: Node
    | SetFunction of func: Node * parameter: Node * result: Node
    | UpdateAssignable of scope: ScopeId * target: Node * assignee: Node

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

    let typeReferenceScopes = Dictionary<TypeReference, ScopeId>()

    let functions = FunctionNodeRelations()
    let assignable = AssignableNodeRelations()
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

    let rec runOperation (operation: Operation) =
        let operationSet = HashSet [operation]

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
                        | UpdateAssignable (scope, target, assignee) -> UpdateAssignable (scope, replace target, replace assignee)

                    operationSet.Add(operation) |> ignore

        let getHighestPriorityOperation operations =
            operations
            |> Seq.sortBy (function
                | Merge _ -> 1
                | SetMonomorphic _ -> 2
                | SetFunction _ -> 3
                | UpdateAssignable _ -> 4)
            |> Seq.head

        while operationSet.Count > 0 do
            let op = getHighestPriorityOperation operationSet

            operationSet.Remove(op) |> ignore

            let outcome =
                match op with
                | Merge (a, b) -> merge (a, b)
                | SetMonomorphic (scope, node) -> setMonomorphic (scope, node)
                | SetFunction (func, param, result) -> setFunction (func, param, result)
                | UpdateAssignable (scope, target, assignee) -> updateAssignable (scope, target, assignee)

            for operation in outcome.followupOperations do
                operationSet.Add(operation) |> ignore

            rebuildSet outcome.nodeSubstitutions

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

            // Merge assignable

            // If a is a target then move the assignees to b
            for scope, aAssignee in assignable.GetAssignees(a) do
                followupOperations.Add(UpdateAssignable (scope, b, aAssignee))

                assignable.Unset(scope, a, aAssignee)

            // If a is an assignee then set its target to b
            match assignable.TryGetTarget(a) with
            | None -> ()
            | Some (aScope, aTarget) ->
                match assignable.TryGetTarget(b) with
                | None -> followupOperations.Add(UpdateAssignable (aScope, aTarget, b))
                | Some (bScope, bTarget) ->
                    if bScope <> aScope then
                        failwith "Merged nodes are assignees in different scopes"

                    if aTarget <> bTarget then
                        followupOperations.Add(Merge (aTarget, bTarget))

                assignable.Unset(aScope, aTarget, a)

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
                match assignable.TryGetTarget(scope, node) with
                | Some target ->
                    assignable.Unset(scope, target, node)
                    followupOperations.Add(Merge (target, node))
                | None -> ()

        OperationOutcome.Followup(followupOperations)

    and updateAssignable (scope: ScopeId, target: Node, assignee: Node): OperationOutcome =
        if target = assignee then
            OperationOutcome.Empty
        elif monomorphic.IsMonomorphic(scope, assignee) then
            OperationOutcome.Followup([ Merge (target, assignee) ])
        else
            let followupOperations = List()

            // If target is a function, then set assignable (assignee.param <- target.param) and (target.result <- assignee.result)
            match functions.TryGetParamResultOfFunction(target) with
            | None ->
                assignable.Set(scope, target, assignee)
            | Some (targetParam, targetResult) ->
                let assigneeParam, assigneeResult =
                    match functions.TryGetParamResultOfFunction(assignee) with
                    | None ->
                        let p = createNode ()
                        let r = createNode ()
                        followupOperations.Add(SetFunction (assignee, p, r))
                        p, r
                    | Some (p, r) -> p, r

                followupOperations.Add(UpdateAssignable (scope, assigneeParam, targetParam))
                followupOperations.Add(UpdateAssignable (scope, targetResult, assigneeResult))

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

                    for scope, funcAssignee in assignable.GetAssignees(func) do
                        followupOperations.Add(UpdateAssignable (scope, func, funcAssignee))

        OperationOutcome.Followup(followupOperations)

    member _.Function(fn: TypeReference, param: TypeReference, result: TypeReference) =
        runOperation (SetFunction (getNode fn, getNode param, getNode result))

    member _.Assignable(scope: ScopeId, target: TypeReference, assignee: TypeReference) =
        runOperation (UpdateAssignable(scope, getNode target, getNode assignee))

    member _.Identical(a: TypeReference, b: TypeReference) =
        runOperation (Merge(getNode a, getNode b))

    member _.Monomorphic(scope: ScopeId, typeReference: TypeReference) =
        runOperation (SetMonomorphic (scope, getNode typeReference))

    member _.DefinedInScope(scope: ScopeId, typeReference: TypeReference) =
        typeReferenceScopes.Add(typeReference, scope)

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
                    match typeReferenceScopes.TryGetValue(typeReference) with
                    | false, _ -> []
                    | true, scope ->
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

    override _.ToString() =
        let nameCounters = Dictionary()
        let nodeNames =
            typeReferenceNodes
            |> Seq.groupBy (fun kv -> kv.Value)
            |> Seq.map (fun (key, tRefs) ->
                let name =
                    tRefs
                    |> Seq.map (fun kv -> kv.Key)
                    |> Seq.map string
                    |> Seq.sortBy (fun s -> s.Length)
                    |> Seq.head

                let name =
                    match nameCounters.TryGetValue(name) with
                    | false, _ ->
                        nameCounters[name] <- 2
                        name
                    | true, counter ->
                        nameCounters[name] <- counter + 1
                        name + string counter
                key, name)
            |> Map.ofSeq

        let getNodeName n =
            match nodeNames |> Map.tryFind n with
            | Some name -> name
            | None -> $"node({n})"

        let sb = StringBuilder()
        let append (s: string) = sb.Append(s) |> ignore
        let appendLine (s: string) = sb.AppendLine(s) |> ignore

        for node, name in nodeNames |> Map.toSeq do
            appendLine name

            match functions.TryGetParamResultOfFunction(node) with
            | Some (param, result) ->
                let rec getExpanded n =
                    match functions.TryGetParamResultOfFunction(n) with
                    | Some (param, result) ->
                        if functions.IsFunction(param) then
                            $"({getExpanded param}) -> {getExpanded result}"
                        else
                            $"{getExpanded param} -> {getExpanded result}"
                    | None -> getNodeName n

                append $"   is function {getNodeName param} -> {getNodeName result}"
                if functions.IsFunction(param) || functions.IsFunction(result) then
                    append $" | {getExpanded node}"
                appendLine ""
            | None ->
                appendLine "   is an atom"

            match assignable.TryGetTarget(node) with
            | Some (_, target) -> appendLine $"   is assignable to {getNodeName target}"
            | None -> ()

            appendLine ""

        sb.ToString()
