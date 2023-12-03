namespace rec TypeSolver

open System.Collections.Generic
open Common

type private Node = int

type private PrototypeToInstanceMap = Dictionary<Node, Node>

type private NodeShape =
    | Atom of AtomTypeId
    | Function of param: Node * result: Node

type private Operation =
    | SetAsAtom of Node * AtomTypeId
    | AddFunction of func: Node * parameter: Node * result: Node
    | Merge of a: Node * b: Node
    | EnforcePrototypeInstance of prototype: Node * instance: Node * map: PrototypeToInstanceMap

type private OperationOutcome =
    | Done of followupOperations: Operation list * nodeSubstitutions: Map<Node, Node>
    | Blocked of blockingOperations: Operation list

type TypeGraph() =
    let allNodes = HashSet<Node>()

    let typeReferenceNodes = Dictionary<TypeReference, Node>()

    let atoms = Dictionary<Node, AtomTypeId>()

    let functions = HashSet<Node * Node * Node>()

    let instanceMaps = List<PrototypeToInstanceMap>()

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

                    operationSet.Add(operation) |> ignore

        let getHighestPriorityOperation operations =
            operations
            |> Seq.sortBy (function
                | Merge _ -> 1
                | SetAsAtom _ -> 2
                | AddFunction _ -> 3
                | EnforcePrototypeInstance _ -> 4)
            |> Seq.head

        while operationSet.Count > 0 do
            let operation = getHighestPriorityOperation operationSet

            operationSet.Remove(operation) |> ignore

            let outcome =
                match operation with
                | SetAsAtom (node, shape) -> setAsAtom (node, shape)
                | AddFunction (func, param, result) -> addFunction (func, param, result)
                | Merge (a, b) -> merge (a, b)
                | EnforcePrototypeInstance (proto, inst, map) -> enforcePrototypeInstance (proto, inst, map)

            match outcome with
            | Done (followupOperations, nodeSubstitutions) ->
                rebuildSet nodeSubstitutions
                for operation in followupOperations do
                    operationSet.Add(operation) |> ignore
            | Blocked _ ->
                failwith "Not used"

    and merge (a: Node, b: Node) =
        if a = b then
            Done ([], Map.empty)
        else
            let followupOperations = List()

            // Replace in type reference to node map
            for kv in typeReferenceNodes |> List.ofSeq do
                if kv.Value = a then
                    typeReferenceNodes[kv.Key] <- b

            // If a is an atom type, then set b as this atom type
            match getAtomType a, getAtomType b with
            | None, _ -> ()
            | Some aAtomTypeId, Some bAtomTypeId when aAtomTypeId = bAtomTypeId -> ()
            | Some atomTypeId, _ -> followupOperations.Add(SetAsAtom (b, atomTypeId))
            atoms.Remove(a) |> ignore

            // Merge functions
            let functionA = getFunctionParamResult a
            let functionsWithParameterA = getFunctionsOfParam a
            let functionsWithResultA = getFunctionsOfResult a
            functions.RemoveWhere (fun (f, p, r) -> f = a || p = a || r = a) |> ignore

            // Merge functions where a is the function
            match functionA, getFunctionParamResult b with
            | None, _ -> ()
            | Some (aParam, aResult), None ->
                followupOperations.Add(AddFunction (b, aParam, aResult))
            | Some (aParam, aResult), Some (bParam, bResult) ->
                followupOperations.Add(Merge (aParam, bParam))
                followupOperations.Add(Merge (aResult, bResult))

            // Merge functions where a is parameter
            for func, result in functionsWithParameterA do
                followupOperations.Add(AddFunction (func, b, result))

            // Merge functions where a is result
            for func, param in functionsWithResultA do
                followupOperations.Add(AddFunction (func, param, b))

            // Merge instances
            for instanceMap in instanceMaps do
                // If a is a prototype then move the instance to b
                match instanceMap |> getInstance a with
                | None -> ()
                | Some aInstance ->
                    match instanceMap |> getInstance b with
                    | None ->
                        followupOperations.Add(EnforcePrototypeInstance (b, aInstance, instanceMap))
                    | Some bInstance ->
                        if aInstance <> bInstance then
                            followupOperations.Add(Merge (aInstance, bInstance))

                    instanceMap.Remove(a) |> ignore

                // If a is an instance then set prototype for b
                match instanceMap |> getPrototype a with
                | None -> ()
                | Some aPrototype ->
                    match instanceMap |> getPrototype b with
                    | None ->
                        followupOperations.Add(EnforcePrototypeInstance (aPrototype, b, instanceMap))
                    | Some bPrototype ->
                        if aPrototype <> bPrototype then
                            followupOperations.Add(Merge (aPrototype, bPrototype))

                    instanceMap.Remove(aPrototype) |> ignore

            Done (followupOperations |> List.ofSeq, Map.ofList [ a, b ])

    and enforcePrototypeInstance (prototype: Node, instance: Node, instanceMap: PrototypeToInstanceMap): OperationOutcome =
        if prototype = instance then
            failwith "A type cannot be a prototype of itself"
        else
            let followupOperations = List()

            match instanceMap.TryGetValue(prototype) with
            | true, anotherInstance when anotherInstance <> instance ->
                // If prototype already has another instance, then merge the two instances
                followupOperations.Add(Merge (instance, anotherInstance))
            | alreadyLinked, _ ->
                if not alreadyLinked then
                    instanceMap.Add(prototype, instance)

                // If prototype is an atom, then apply atom to instance
                match atoms.TryGetValue(prototype) with
                | true, atomTypeId -> followupOperations.Add(SetAsAtom (instance, atomTypeId))
                | false, _ -> ()

                // If prototype is a function, then enforce (paramProto -> paramInst) and (resultProto -> resultInst)
                match getFunctionParamResult prototype, getFunctionParamResult instance with
                | None, None
                | None, Some _ -> ()
                | Some (paramProto, resultProto), None ->
                    let paramInst =
                        match instanceMap |> getInstance paramProto with
                        | Some paramInst -> paramInst
                        | None ->
                            let paramInst = createNode ()
                            followupOperations.Add(EnforcePrototypeInstance (paramProto, paramInst, instanceMap))
                            paramInst

                    let resultInst =
                        match instanceMap |> getInstance resultProto with
                        | Some resultInst -> resultInst
                        | None ->
                            let resultInst = createNode ()
                            followupOperations.Add(EnforcePrototypeInstance (resultProto, resultInst, instanceMap))
                            resultInst

                    followupOperations.Add(AddFunction (instance, paramInst, resultInst))
                | Some (paramProto, resultProto), Some (paramInst, resultInst) ->
                    match instanceMap |> getInstance paramProto with
                    | None -> followupOperations.Add(EnforcePrototypeInstance (paramProto, paramInst, instanceMap))
                    | Some paramInstFromMap ->
                        if paramInst <> paramInstFromMap then
                            followupOperations.Add(Merge (paramInst, paramInstFromMap))

                    match instanceMap |> getInstance resultProto with
                    | None -> followupOperations.Add(EnforcePrototypeInstance (resultProto, resultInst, instanceMap))
                    | Some resultInstFromMap ->
                        if resultInst <> resultInstFromMap then
                            followupOperations.Add(Merge (resultInst, resultInstFromMap))

            Done (followupOperations |> List.ofSeq, Map.empty)

    and setAsAtom (node: Node, atomTypeId: AtomTypeId): OperationOutcome =
        let followupOperations = List()

        match atoms.TryGetValue(node) with
        | true, anotherAtomTypeId ->
            if anotherAtomTypeId <> atomTypeId then
                failwith "The node is constrained to be two different atom types"
        | false, _ ->
            let nodeIsFunction = functions |> Seq.exists (fun (f, _, _) -> f = node)
            if nodeIsFunction then
                failwith "The node cannot be an atom type because it is a function"

            atoms.Add(node, atomTypeId)

            for instanceMap in instanceMaps do
                match instanceMap |> getInstance node with
                | Some inst -> followupOperations.Add(EnforcePrototypeInstance (node, inst, instanceMap))
                | None -> ()

        Done (followupOperations |> List.ofSeq, Map.empty)

    and addFunction (func: Node, param: Node, result: Node): OperationOutcome =
        let followupOperations = List()

        let anotherFunctionsOfSameParamResult =
            functions
            |> Seq.choose (function
                | f, p, r when p = param && r = result -> Some f
                | _ -> None)
            |> List.ofSeq

        match anotherFunctionsOfSameParamResult with
        | [ anotherFunc ] ->
            followupOperations.Add(Merge (func, anotherFunc))
        | [] ->
            match getFunctionParamResult func with
            | Some (anotherParam, anotherResult) ->
                if anotherParam <> param then
                    followupOperations.Add(Merge (param, anotherParam))

                if anotherResult <> result then
                    followupOperations.Add(Merge (result, anotherResult))
            | None ->
                let nodeIsAtom = atoms.ContainsKey(func)
                if nodeIsAtom then
                    failwith "The node cannot be a function because it is an atom"

                functions.Add(func, param, result) |> ignore

                for instanceMap in instanceMaps do
                    match instanceMap |> getInstance func with
                    | Some inst -> followupOperations.Add(EnforcePrototypeInstance (func, inst, instanceMap))
                    | None -> ()
        | _ -> failwith "There are multiple functions of same parameter and result"

        Done (followupOperations |> List.ofSeq, Map.empty)

    and getAtomType (node: Node) =
        match atoms.TryGetValue(node) with
        | true, atomTypeId -> Some atomTypeId
        | false, _ -> None

    and getFunctionParamResult (node: Node) =
        let paramResult =
            functions
            |> Seq.choose (function
                | f, p, r when f = node -> Some (p, r)
                | _ -> None)
            |> List.ofSeq

        match paramResult with
        | [] -> None
        | [ param, result ] -> Some (param, result)
        | _ -> failwith "The node has more than one function constraint"

    and getFunctionsOfParam (node: Node) =
        functions
        |> Seq.choose (function
            | f, p, r when p = node -> Some (f, r)
            | _ -> None)
        |> List.ofSeq

    and getFunctionsOfResult (node: Node) =
        functions
        |> Seq.choose (function
            | f, p, r when r = node -> Some (f, p)
            | _ -> None)
        |> List.ofSeq

    and getInstance (prototype: Node) (instanceMap: PrototypeToInstanceMap) =
        match instanceMap.TryGetValue(prototype) with
        | true, instance -> Some instance
        | false, _ -> None

    and getPrototype (instance: Node) (instanceMap: PrototypeToInstanceMap) =
        let prototypes =
            instanceMap
            |> Seq.choose (fun kv ->
                if kv.Value = instance then
                    Some kv.Key
                else
                    None)
            |> List.ofSeq

        match prototypes with
        | [] -> None
        | [ prototype ] -> Some prototype
        | _ -> failwith "There should not be more than one prototype in one instance map"

    and getOrCreateInstance (prototype: Node) (instanceMap: PrototypeToInstanceMap): bool * Node =
        match instanceMap.TryGetValue(prototype) with
        | true, instance -> false, instance
        | false, _ ->
            let instance = createNode ()
            instanceMap.Add(prototype, instance)
            true, instance

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

        if prototypeNode = instanceNode then
            failwith "Type cannot be a prototype of itself"

        let instanceHasDifferentPrototype =
            instanceMaps
            |> Seq.collect id
            |> Seq.exists (fun kv -> kv.Value = instanceNode && kv.Key <> prototypeNode)

        if instanceHasDifferentPrototype then
            failwith "Instance already has a different prototype"

        let thisPrototypeRelationAlreadyExists =
            instanceMaps
            |> Seq.exists (fun instanceMap ->
                match instanceMap.TryGetValue(prototypeNode) with
                | true, i when i = instanceNode -> true
                | _ -> false)

        if not thisPrototypeRelationAlreadyExists then
            let map = PrototypeToInstanceMap()
            instanceMaps.Add(map)

            runOperation (EnforcePrototypeInstance (prototypeNode, instanceNode, map))

    member _.Identical(a: TypeReference, b: TypeReference) =
        runOperation (Merge(getNode a, getNode b))

    member _.GetResult(): Map<TypeReference, Type> =
        let mutable typeMap = Map.empty

        let nodeTypeMap = Dictionary()

        let rec getType node =
            match nodeTypeMap.TryGetValue(node) with
            | true, t -> t
            | false, _ ->
                let t =
                    match getAtomType node with
                    | Some atomTypeId ->
                        match getFunctionParamResult node with
                        | None -> AtomType atomTypeId |> Some
                        | Some _ -> failwith "The type was constrained to be a function and an atom type"
                    | None ->
                        match getFunctionParamResult node with
                        | Some (param, result) ->
                            getType param
                            |> Option.bind(fun p ->
                                getType result
                                |> Option.map(fun r ->
                                    FunctionType (p, r)))
                        | None -> VariableType (VariableTypeId()) |> Some

                nodeTypeMap.Add(node, t)
                t

        for kv in typeReferenceNodes do
            let tRef = kv.Key
            let node = kv.Value

            match getType node with
            | Some t -> typeMap <- typeMap |> Map.add tRef t
            | _ -> ()

        typeMap
