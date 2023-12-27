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
    | SetAsAtom of Node * AtomTypeId
    | AddFunction of func: Node * parameter: Node * result: Node
    | Merge of a: Node * b: Node
    | EnforcePrototypeInstance of prototype: Node * instance: Node * group: Guid
    | ForbidGeneralization of Node

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
    let nonGeneralizable = FlagNodeProperty()
    let functions = FunctionNodeRelations()
    let instances = InstanceNodeRelations()

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
                        | SetAsAtom (node, atomTypeId) -> SetAsAtom (replace node, atomTypeId)
                        | AddFunction (func, param, result) -> AddFunction (replace func, replace param, replace result)
                        | Merge (a, b) -> Merge (replace a, replace b)
                        | EnforcePrototypeInstance (proto, inst, map) -> EnforcePrototypeInstance (replace proto, replace inst, map)
                        | ForbidGeneralization node -> ForbidGeneralization (replace node)

                    operationSet.Add(operation) |> ignore

        let getHighestPriorityOperation operations =
            operations
            |> Seq.sortBy (function
                | Merge _ -> 1
                | SetAsAtom _ -> 2
                | ForbidGeneralization _ -> 3
                | AddFunction _ -> 4
                | EnforcePrototypeInstance _ -> 5)
            |> Seq.head

        while operationSet.Count > 0 do
            let op = getHighestPriorityOperation operationSet

            operationSet.Remove(op) |> ignore

            let outcome =
                match op with
                | SetAsAtom (node, shape) -> setAsAtom (node, shape)
                | AddFunction (func, param, result) -> addFunction (func, param, result)
                | Merge (a, b) -> merge (a, b)
                | EnforcePrototypeInstance (proto, inst, map) -> enforcePrototypeInstance (proto, inst, map)
                | ForbidGeneralization node -> forbidGeneralization node

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

            // Merge instances

            // If a is a prototype then move the instances to b
            for group, aInstance in instances.GetInstances(a) do
                match instances.TryGetInstance(b, group) with
                | Some bInstance ->
                    if aInstance <> bInstance then
                        followupOperations.Add(Merge (aInstance, bInstance))
                | None ->
                    followupOperations.Add(EnforcePrototypeInstance (b, aInstance, group))

                instances.RemoveFromGroup(a, aInstance, group)

            // If a is an instance then set prototype for b
            match instances.TryGetPrototype(a) with
            | None -> ()
            | Some (aGroup, aPrototype) ->
                match instances.TryGetPrototype(b) with
                | None ->
                    followupOperations.Add(EnforcePrototypeInstance (aPrototype, b, aGroup))
                | Some (bGroup, bPrototype) ->
                    if bGroup <> aGroup then
                        failwith "Merged nodes have prototypes in different groups"

                    if aPrototype <> bPrototype then
                        followupOperations.Add(Merge (aPrototype, bPrototype))

                instances.RemoveFromGroup(aPrototype, a, aGroup)

            // Merge nonGeneralizable
            if nonGeneralizable.IsSet(a) then
                nonGeneralizable.Unset(a)
                nonGeneralizable.Set(b)

            // Remove from all nodes
            allNodes.Remove(a) |> ignore

            { followupOperations = followupOperations |> List.ofSeq
              nodeSubstitutions = Map.ofList [ a, b ] }

    and enforcePrototypeInstance (prototype: Node, instance: Node, group: Guid): OperationOutcome =
        if prototype = instance then
            OperationOutcome.Empty
        elif nonGeneralizable.IsSet(prototype) then
            OperationOutcome.Followup([ Merge (prototype, instance) ])
        else
            let followupOperations = List()

            match instances.TryGetInstance(prototype, group) with
            | Some existingInstance when existingInstance <> instance ->
                // If prototype already has another instance, then merge the two instances
                followupOperations.Add(Merge (instance, existingInstance))
            | existingInstance ->
                if existingInstance |> Option.isNone then
                    instances.AddToGroup(prototype, instance, group)

                // If prototype is an atom, then apply atom to instance
                match atoms.TryGetAtomTypeId(prototype) with
                | Some atomTypeId -> followupOperations.Add(SetAsAtom (instance, atomTypeId))
                | None -> ()

                // If prototype is a function, then enforce (paramProto -> paramInst) and (resultProto -> resultInst)
                match functions.TryGetParamResultOfFunction(prototype), functions.TryGetParamResultOfFunction(instance) with
                | None, None
                | None, Some _ -> ()
                | Some (paramProto, resultProto), None ->
                    let paramInst =
                        match instances.TryGetInstance(paramProto, group) with
                        | Some paramInst -> paramInst
                        | None ->
                            let paramInst = createNode ()
                            followupOperations.Add(EnforcePrototypeInstance (paramProto, paramInst, group))
                            paramInst

                    let resultInst =
                        match instances.TryGetInstance(resultProto, group) with
                        | Some resultInst -> resultInst
                        | None ->
                            let resultInst = createNode ()
                            followupOperations.Add(EnforcePrototypeInstance (resultProto, resultInst, group))
                            resultInst

                    followupOperations.Add(AddFunction (instance, paramInst, resultInst))
                | Some (paramProto, resultProto), Some (paramInst, resultInst) ->
                    match instances.TryGetInstance(paramProto, group) with
                    | None -> followupOperations.Add(EnforcePrototypeInstance (paramProto, paramInst, group))
                    | Some paramInstFromMap ->
                        if paramInst <> paramInstFromMap then
                            followupOperations.Add(Merge (paramInst, paramInstFromMap))

                    match instances.TryGetInstance(resultProto, group) with
                    | None -> followupOperations.Add(EnforcePrototypeInstance (resultProto, resultInst, group))
                    | Some resultInstFromMap ->
                        if resultInst <> resultInstFromMap then
                            followupOperations.Add(Merge (resultInst, resultInstFromMap))

            OperationOutcome.Followup(followupOperations)

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
                for group, instance in instances.GetInstances(node) do
                    followupOperations.Add(EnforcePrototypeInstance (node, instance, group))

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
                    for group, funcInstance in instances.GetInstances(func) do
                        followupOperations.Add(EnforcePrototypeInstance (func, funcInstance, group))

                    if nonGeneralizable.IsSet(func) then
                        followupOperations.Add(ForbidGeneralization param)
                        followupOperations.Add(ForbidGeneralization result)

        OperationOutcome.Followup(followupOperations)

    and forbidGeneralization (node: Node) =
        if nonGeneralizable.IsSet(node) then
            OperationOutcome.Empty
        else
            let followupOperations = List()

            nonGeneralizable.Set(node)

            match functions.TryGetParamResultOfFunction(node) with
            | Some (param, result) ->
                followupOperations.Add(ForbidGeneralization param)
                followupOperations.Add(ForbidGeneralization result)
            | None -> ()

            for _, instance in instances.GetInstances(node) do
                followupOperations.Add(Merge (node, instance))

            OperationOutcome.Followup(followupOperations)

    member _.Atom(tRef: TypeReference, atomTypeId: AtomTypeId) =
        let node = getNode tRef
        runOperation (SetAsAtom (node, atomTypeId))

    member _.Function(func: TypeReference, param: TypeReference, result: TypeReference) =
        let funcNode = getNode func
        let paramNode = getNode param
        let resultNode = getNode result

        runOperation (AddFunction (funcNode, paramNode, resultNode))

    member _.Instance(prototype: TypeReference, instance: TypeReference) =
        let prototypeNode = getNode prototype
        let instanceNode = getNode instance

        match instances.TryCreateGroup(prototypeNode, instanceNode) with
        | Some mapId ->
            runOperation (EnforcePrototypeInstance (prototypeNode, instanceNode, mapId))
        | None ->
            ()

    member _.Identical(a: TypeReference, b: TypeReference) =
        runOperation (Merge(getNode a, getNode b))

    member _.NonGeneralizable(typeReference: TypeReference) =
        runOperation (ForbidGeneralization (getNode typeReference))

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

    // override _.ToString() =
    //     let sb = StringBuilder()
    //
    //     sb.AppendLine("=== TypeReferences ===") |> ignore
    //     for kv in typeReferenceNodes do
    //         sb.Append(kv.Key.ToString()) |> ignore
    //         sb.Append(" = ") |> ignore
    //         sb.AppendLine(kv.Value.ToString()) |> ignore
    //     sb.AppendLine("=== Atoms ===") |> ignore
    //     sb.AppendLine(atoms.ToString()) |> ignore
    //     sb.AppendLine("=== Functions ===") |> ignore
    //     sb.AppendLine(functions.ToString()) |> ignore
    //     sb.AppendLine("=== Instances ===") |> ignore
    //     sb.AppendLine(instances.ToString()) |> ignore
    //     sb.AppendLine("=== Non-generalizable ===") |> ignore
    //     sb.AppendLine(nonGeneralizable.ToString()) |> ignore
    //
    //     sb.ToString()

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
                        | None -> nodeNames[n]

                    append $"   is function {nodeNames[param]} -> {nodeNames[result]}"
                    if functions.IsFunction(param) || functions.IsFunction(result) then
                        append $" | {getExpanded node}"
                    appendLine ""
                | None ->
                    appendLine "   is unknown atom"

            match instances.TryGetPrototype(node) with
            | Some (_, prototype) -> appendLine $"   is instance of {nodeNames[prototype]}"
            | None -> ()

            appendLine ""

        sb.ToString()
