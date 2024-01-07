namespace rec TypeSolver

open System
open System.Collections.Generic
open System.Text
open Common

type private PrototypeToInstanceMap = Dictionary<Node, Node>

type private NodeShape =
    | Atom of AtomTypeId
    | Function of param: Node * result: Node

type private Operation =
    | Merge of a: Node * b: Node
    | SetAsAtom of Node * AtomTypeId
    | SetNonGeneralizable of scope: Guid * node: Node
    | AddFunction of func: Node * parameter: Node * result: Node
    | UpdateAssignable of scope: Guid * target: Node * assignee: Node

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
    { typeReferenceTypes: Map<TypeReference, Type>
      typeReferenceIdentities: Map<TypeReference, Guid> }

type TypeGraph() =
    let allNodes = HashSet<Node>()

    let typeReferenceNodes = Dictionary<TypeReference, Node>()

    let atoms = AtomNodeProperty()
    let functions = FunctionNodeRelations()
    let assignable = AssignableNodeRelations()
    let nonGeneralizable = NonGeneralizableNodeProperty()

    let createNode =
        let mutable newNodeId = 1

        fun () ->
            let node = newNodeId

            newNodeId <- newNodeId + 1

            if not (allNodes.Add node) then
                failwith "Duplicate node"

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
                        | SetAsAtom (node, atomTypeId) -> SetAsAtom (replace node, atomTypeId)
                        | SetNonGeneralizable (scope, node) -> SetNonGeneralizable (scope, replace node)
                        | AddFunction (func, param, result) -> AddFunction (replace func, replace param, replace result)
                        | UpdateAssignable (scope, target, assignee) -> UpdateAssignable (scope, replace target, replace assignee)

                    operationSet.Add(operation) |> ignore

        let getHighestPriorityOperation operations =
            operations
            |> Seq.sortBy (function
                | Merge _ -> 1
                | SetAsAtom _ -> 2
                | SetNonGeneralizable _ -> 3
                | AddFunction _ -> 4
                | UpdateAssignable _ -> 5)
            |> Seq.head

        while operationSet.Count > 0 do
            let op = getHighestPriorityOperation operationSet

            operationSet.Remove(op) |> ignore

            let outcome =
                match op with
                | Merge (a, b) -> merge (a, b)
                | SetAsAtom (node, shape) -> setAsAtom (node, shape)
                | SetNonGeneralizable (scope, node) -> setNonGeneralizable (scope, node)
                | AddFunction (func, param, result) -> addFunction (func, param, result)
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

            // If a is an atom type, then set b as this atom type
            match atoms.TryGetAtomTypeId(a), atoms.TryGetAtomTypeId(b) with
            | None, _ -> ()
            | Some aAtomTypeId, Some bAtomTypeId when aAtomTypeId = bAtomTypeId -> ()
            | Some atomTypeId, _ -> followupOperations.Add(SetAsAtom (b, atomTypeId))

            atoms.Unset(a)

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
                followupOperations.Add(AddFunction (b, aParam, aResult))
            | Some (aParam, aResult), Some (bParam, bResult) ->
                followupOperations.Add(Merge (aParam, bParam))
                followupOperations.Add(Merge (aResult, bResult))

            // Merge functions where a is parameter
            for func, _, result in functionsWithParameterA do
                followupOperations.Add(AddFunction (func, b, result))

            // Merge functions where a is result
            for func, param, _ in functionsWithResultA do
                followupOperations.Add(AddFunction (func, param, b))

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

            // Merge nonGeneralizableScopes
            for aScope in nonGeneralizable.GetScopesWhereNotGeneralizable(a) do
                nonGeneralizable.Unset(aScope, a)
                followupOperations.Add(SetNonGeneralizable(aScope, b))

            // Remove from all nodes
            allNodes.Remove(a) |> ignore

            { followupOperations = followupOperations |> List.ofSeq
              nodeSubstitutions = Map.ofList [ a, b ] }

    and setAsAtom (node: Node, atomTypeId: AtomTypeId): OperationOutcome =
        if functions.IsFunction(node) then
            failwith "The node cannot be an atom type because it is a function"

        let followupOperations = List()

        match atoms.TryGetNodeByAtomTypeId(atomTypeId) with
        | Some existingNode ->
            if existingNode <> node then
                followupOperations.Add(Merge (node, existingNode))
        | None ->
            if atoms.Set(node, atomTypeId) then
                for scope, assignee in assignable.GetAssignees(node) do
                    followupOperations.Add(UpdateAssignable (scope, node, assignee))

        OperationOutcome.Followup(followupOperations)

    and setNonGeneralizable (scope: Guid, node: Node) =
        let followupOperations = List()

        if nonGeneralizable.Set(scope, node) then
            for assignee in assignable.GetAssignees(scope, node) do
                assignable.Unset(scope, node, assignee)
                followupOperations.Add(Merge (assignee, node))

        OperationOutcome.Followup(followupOperations)

    and updateAssignable (scope: Guid, target: Node, assignee: Node): OperationOutcome =
        if target = assignee then
            OperationOutcome.Empty
        elif nonGeneralizable.IsNotGeneralizable(scope, target) then
            OperationOutcome.Followup([ Merge (target, assignee) ])
        else
            let followupOperations = List()

            // If target is an atom, then assignee is also this atom
            match atoms.TryGetAtomTypeId(target) with
            | Some atomTypeId -> followupOperations.Add(SetAsAtom (assignee, atomTypeId))
            | None -> ()

            // If target is a function, then set assignable (assignee.param <- target.param) and (target.result <- assignee.result)
            match functions.TryGetParamResultOfFunction(target) with
            | None -> ()
            | Some (targetParam, targetResult) ->
                let assigneeParam, assigneeResult =
                    match functions.TryGetParamResultOfFunction(assignee) with
                    | None ->
                        let p = createNode ()
                        let r = createNode ()
                        followupOperations.Add(AddFunction (assignee, p, r))
                        p, r
                    | Some (p, r) -> p, r

                followupOperations.Add(UpdateAssignable (scope, assigneeParam, targetParam))
                followupOperations.Add(UpdateAssignable (scope, targetResult, assigneeResult))

            OperationOutcome.Followup(followupOperations)

    and addFunction (func: Node, param: Node, result: Node): OperationOutcome =
        if atoms.IsAtom(func) then
            failwith "The node cannot be a function because it is an atom"

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
                    for scope, funcAssignee in assignable.GetAssignees(func) do
                        followupOperations.Add(UpdateAssignable (scope, func, funcAssignee))

        OperationOutcome.Followup(followupOperations)

    member _.Atom(tRef: TypeReference, atomTypeId: AtomTypeId) =
        let node = getNode tRef
        runOperation (SetAsAtom (node, atomTypeId))

    member _.Function(fn: TypeReference, param: TypeReference, result: TypeReference) =
        runOperation (AddFunction (getNode fn, getNode param, getNode result))

    member _.Assignable(scope: Guid, target: TypeReference, assignee: TypeReference) =
        runOperation (UpdateAssignable(scope, getNode target, getNode assignee))

    member _.Identical(a: TypeReference, b: TypeReference) =
        runOperation (Merge(getNode a, getNode b))

    member _.NonGeneralizable(scope: Guid, typeReference: TypeReference) =
        runOperation (SetNonGeneralizable (scope, getNode typeReference))

    member _.GetResult(): TypeGraphInfo =
        let mutable typeReferenceTypes = Map.empty
        let mutable typeReferenceIdentities = Map.empty

        let nodeTypeMap = Dictionary()

        let rec getType node =
            match nodeTypeMap.TryGetValue(node) with
            | true, t -> t
            | false, _ ->
                let type_ =
                    match atoms.TryGetAtomTypeId(node), functions.TryGetParamResultOfFunction(node) with
                    | None, None -> AtomType (AtomTypeId())
                    | Some atomTypeId, None -> AtomType atomTypeId
                    | None, Some (param, result) -> FunctionType (getType param, getType result)
                    | Some _, Some _ -> failwith "The type was constrained to be a function and an atom type"

                nodeTypeMap.Add(node, type_)
                type_

        let nodeGuids =
            allNodes
            |> Seq.map (fun node -> (node, Guid.NewGuid()))
            |> Map.ofSeq

        for kv in typeReferenceNodes do
            let tRef = kv.Key
            let node = kv.Value

            let type_ = getType node
            let identity = nodeGuids |> Map.find node
            typeReferenceTypes <- typeReferenceTypes |> Map.add tRef type_
            typeReferenceIdentities <- typeReferenceIdentities |> Map.add tRef identity

        { typeReferenceTypes = typeReferenceTypes
          typeReferenceIdentities = typeReferenceIdentities }

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

            match atoms.TryGetAtomTypeId(node) with
            | Some atomTypeId ->
                appendLine $"   is atom {atomTypeId}"
            | None ->
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
                    appendLine "   is unknown atom"

            match assignable.TryGetTarget(node) with
            | Some (_, target) -> appendLine $"   is assignable to {getNodeName target}"
            | None -> ()

            appendLine ""

        sb.ToString()
