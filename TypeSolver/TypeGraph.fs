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
    | EnforcePrototypeInstance of applicationReference: ApplicationReference * prototype: Node * instance: Node
    | SetNonGeneralizable of scope: Scope * node: Node

type private OperationOutcome =
    { followupOperations: Operation list
      nodeSubstitutions: Map<Node, Node> }

    static member Empty =
        { followupOperations = List.empty
          nodeSubstitutions = Map.empty }

    static member Followup(followupOperations: #seq<Operation>) =
        { followupOperations = followupOperations |> List.ofSeq
          nodeSubstitutions = Map.empty }

type Scope =
    | GlobalScope
    | IdentifierScope of Identifier

type TypeGraphInfo =
    { typeReferenceTypes: Map<TypeReference, Type>
      typeReferenceIdentities: Map<TypeReference, Guid>
      functionApplications: FunctionApplication list }

type TypeGraph() =
    let allNodes = HashSet<Node>()

    let typeReferenceNodes = Dictionary<TypeReference, Node>()

    let atoms = AtomNodeProperty()
    let functions = FunctionNodeRelations()
    let instances = ApplicationInstanceNodeRelations()

    // let applicationScopes = Dictionary<ApplicationReference, Scope>()
    let nonGeneralizableScopes = Dictionary<Scope, HashSet<Node>>()
    let applicationScopes = Dictionary<Scope, HashSet<ApplicationReference>>()

    // let applications = HashSet<ApplicationReference * Node * Node>()

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
                        | EnforcePrototypeInstance (appRef, proto, inst) -> EnforcePrototypeInstance (appRef, replace proto, replace inst)
                        | SetNonGeneralizable (scope, node) -> SetNonGeneralizable (scope, replace node)

                    operationSet.Add(operation) |> ignore

        let getHighestPriorityOperation operations =
            operations
            |> Seq.sortBy (function
                | Merge _ -> 1
                | SetAsAtom _ -> 2
                | SetNonGeneralizable _ -> 3
                | AddFunction _ -> 4
                | EnforcePrototypeInstance _ -> 5)
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
                | EnforcePrototypeInstance (appRef, proto, inst) -> enforcePrototypeInstance (appRef, proto, inst)

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
            for appRef, aInstance in instances.GetInstances(a) do
                match instances.TryGetInstance(appRef, b) with
                | Some bInstance ->
                    if aInstance <> bInstance then
                        followupOperations.Add(Merge (aInstance, bInstance))
                | None ->
                    followupOperations.Add(EnforcePrototypeInstance (appRef, b, aInstance))

                instances.RemoveFromApplication(appRef, a, aInstance)

            // If a is an instance then set prototype for b
            match instances.TryGetPrototype(a) with
            | None -> ()
            | Some (aAppRef, aPrototype) ->
                match instances.TryGetPrototype(b) with
                | None ->
                    followupOperations.Add(EnforcePrototypeInstance (aAppRef, aPrototype, b))
                | Some (bAppRef, bPrototype) ->
                    if bAppRef <> aAppRef then
                        failwith "Merged nodes are instances in different applications"

                    if aPrototype <> bPrototype then
                        followupOperations.Add(Merge (aPrototype, bPrototype))

                instances.RemoveFromApplication(aAppRef, aPrototype, a)

            // Merge nonGeneralizableScopes
            for kv in nonGeneralizableScopes do
                let nodes = kv.Value
                if nodes.Remove(a) then
                    nodes.Add(b) |> ignore

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
                for appRef, instance in instances.GetInstances(node) do
                    followupOperations.Add(EnforcePrototypeInstance (appRef, node, instance))

        OperationOutcome.Followup(followupOperations)

    and setNonGeneralizable (scope: Scope, node: Node) =
        let followupOperations = List()

        let scopeNonGeneralizableTRefs =
            match nonGeneralizableScopes.TryGetValue(scope) with
            | true, tRefs -> tRefs
            | false, _ ->
                let tRefs = HashSet()
                nonGeneralizableScopes.Add(scope, tRefs)
                tRefs

        if scopeNonGeneralizableTRefs.Add(node) then
            match applicationScopes.TryGetValue(scope) with
            | false, _ -> ()
            | true, applicationReferences ->
                for appRef in applicationReferences do
                    match instances.TryGetInstance(appRef, node) with
                    | None -> ()
                    | Some instance -> followupOperations.Add(Merge (node, instance))

        OperationOutcome.Followup(followupOperations)

    and enforcePrototypeInstance (applicationReference: ApplicationReference, prototype: Node, instance: Node): OperationOutcome =
        if prototype = instance then
            OperationOutcome.Empty
        else
            let followupOperations = List()

            let scope =
                applicationScopes
                |> Seq.choose (fun kv ->
                    if kv.Value.Contains(applicationReference) then
                        Some kv.Key
                    else
                        None)
                |> Seq.head

            match nonGeneralizableScopes.TryGetValue(scope) with
            | true, nonGeneralizableTRefs when nonGeneralizableTRefs.Contains(prototype) ->
                followupOperations.Add(Merge (prototype, instance))
            | _ ->
                match instances.TryGetInstance(applicationReference, prototype) with
                | Some existingInstance when existingInstance <> instance ->
                    // If prototype already has another instance, then merge the two instances
                    followupOperations.Add(Merge (instance, existingInstance))
                | existingInstance ->
                    if existingInstance |> Option.isNone then
                        instances.AddToApplication(applicationReference, prototype, instance)

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
                            match instances.TryGetInstance(applicationReference, paramProto) with
                            | Some paramInst -> paramInst
                            | None ->
                                let paramInst = createNode ()
                                followupOperations.Add(EnforcePrototypeInstance (applicationReference, paramProto, paramInst))
                                paramInst

                        let resultInst =
                            match instances.TryGetInstance(applicationReference, resultProto) with
                            | Some resultInst -> resultInst
                            | None ->
                                let resultInst = createNode ()
                                followupOperations.Add(EnforcePrototypeInstance (applicationReference, resultProto, resultInst))
                                resultInst

                        followupOperations.Add(AddFunction (instance, paramInst, resultInst))
                    | Some (paramProto, resultProto), Some (paramInst, resultInst) ->
                        match instances.TryGetInstance(applicationReference, paramProto) with
                        | None -> followupOperations.Add(EnforcePrototypeInstance (applicationReference, paramProto, paramInst))
                        | Some paramInstFromMap ->
                            if paramInst <> paramInstFromMap then
                                followupOperations.Add(Merge (paramInst, paramInstFromMap))

                        match instances.TryGetInstance(applicationReference, resultProto) with
                        | None -> followupOperations.Add(EnforcePrototypeInstance (applicationReference, resultProto, resultInst))
                        | Some resultInstFromMap ->
                            if resultInst <> resultInstFromMap then
                                followupOperations.Add(Merge (resultInst, resultInstFromMap))

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
                    for appRef, funcInstance in instances.GetInstances(func) do
                        followupOperations.Add(EnforcePrototypeInstance (appRef, func, funcInstance))

        OperationOutcome.Followup(followupOperations)

    member _.Atom(tRef: TypeReference, atomTypeId: AtomTypeId) =
        let node = getNode tRef
        runOperation (SetAsAtom (node, atomTypeId))

    member _.FunctionDefinition(fn: TypeReference, param: TypeReference, result: TypeReference) =
        let fnNode = getNode fn
        let paramNode = getNode param
        let resultNode = getNode result

        runOperation (AddFunction (fnNode, paramNode, resultNode))

    member _.Application(scope: Scope, applicationReference: ApplicationReference, fn: TypeReference, argument: TypeReference, result: TypeReference) =
        let fnNode = getNode fn
        let argumentNode = getNode argument
        let resultNode = getNode result

        let fnInstanceNode = createNode ()

        let scopeApplications =
            match applicationScopes.TryGetValue(scope) with
            | true, apps -> apps
            | false, _ ->
                let apps = HashSet()
                applicationScopes.Add(scope, apps)
                apps

        scopeApplications.Add(applicationReference) |> ignore

        runOperation (AddFunction (fnInstanceNode, argumentNode, resultNode))
        runOperation (EnforcePrototypeInstance (applicationReference, fnNode, fnInstanceNode))

    member _.Identical(a: TypeReference, b: TypeReference) =
        runOperation (Merge(getNode a, getNode b))

    member _.NonGeneralizable(scope: Scope, typeReference: TypeReference) =
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
          typeReferenceIdentities = typeReferenceIdentities
          functionApplications = [ (* TODO *) ] }

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

            match instances.TryGetPrototype(node) with
            | Some (_, prototype) -> appendLine $"   is instance of {getNodeName prototype}"
            | None -> ()

            appendLine ""

        sb.ToString()
