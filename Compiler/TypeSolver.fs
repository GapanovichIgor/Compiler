module internal rec Compiler.TypeSolver

#nowarn "46"

open System.Collections.Generic
open System.Collections.Immutable
open Compiler.Ast

type private Constraint =
    | Same of a: TypeReference * b: TypeReference
    | Atom of typeReference: TypeReference * atomTypeId: AtomTypeId
    | Function of fn: TypeReference * parameter: TypeReference * result: TypeReference
    | Assignable of to_: TypeReference * from: TypeReference
    | AssignableToAtom of typeReference: TypeReference * atomTypeId: AtomTypeId
    | AssignableFromAtom of typeReference: TypeReference * atomTypeId: AtomTypeId

type private InternalData =
    { mutable allTypeReferences: HashSet<TypeReference>
      mutable identifierTypes: Dictionary<Identifier, TypeReference>
      mutable constraints: HashSet<Constraint>
      mutable typeRefReplaceDict: Dictionary<TypeReference, TypeReference> }

    member this.GetIdentifierType(i: Identifier) =
        match this.identifierTypes.TryGetValue(i) with
        | true, t -> t
        | false, _ ->
            let t = TypeReference($"identifier {i.Name}")
            this.identifierTypes[i] <- t
            this.allTypeReferences.Add(t) |> ignore
            t

type TypeMap =
    { identifierTypes: ImmutableDictionary<Identifier, TypeConstructor>
      typeReferenceTypes: ImmutableDictionary<TypeReference, TypeConstructor> }

module private Traverse =
    type private Context(data: InternalData) =
        member this.GetIdentifierType(i: Identifier) = data.GetIdentifierType(i)

        member _.ConstrainSame(a, b) =
            data.allTypeReferences.Add(a) |> ignore
            data.allTypeReferences.Add(b) |> ignore
            data.constraints.Add(Same(a, b)) |> ignore

        member _.ConstrainAtom(typeReference, atomTypeId) =
            data.allTypeReferences.Add(typeReference) |> ignore
            data.constraints.Add(Atom(typeReference, atomTypeId)) |> ignore

        member _.ConstrainFunction(fn, parameter, result) =
            data.allTypeReferences.Add(fn) |> ignore
            data.allTypeReferences.Add(parameter) |> ignore
            data.allTypeReferences.Add(result) |> ignore
            data.constraints.Add(Function(fn, parameter, result)) |> ignore

        member _.ConstrainAssignable(to_, from) =
            data.allTypeReferences.Add(to_) |> ignore
            data.allTypeReferences.Add(from) |> ignore
            data.constraints.Add(Assignable(to_, from)) |> ignore

    let private traverseExpression (ctx: Context) (expression: Expression) =
        match expression.expressionShape with
        | IdentifierReference identifier ->
            let identifierType = ctx.GetIdentifierType(identifier)
            ctx.ConstrainSame(expression.expressionType, identifierType)
        | NumberLiteral(_, fractionalPart) ->
            let numberType =
                match fractionalPart with
                | Some _ -> BuiltIn.AtomTypeIds.float
                | None -> BuiltIn.AtomTypeIds.int

            ctx.ConstrainAtom(expression.expressionType, numberType)
        | StringLiteral _ -> ctx.ConstrainAtom(expression.expressionType, BuiltIn.AtomTypeIds.string)
        | Application(fn, argument) ->
            let parameter = TypeReference($"parameter of ({fn.expressionType})")
            let result = TypeReference($"result of ({fn.expressionType})")
            ctx.ConstrainFunction(fn.expressionType, parameter, result)
            ctx.ConstrainAssignable(parameter, argument.expressionType)
            ctx.ConstrainSame(expression.expressionType, result)

            traverseExpression ctx fn
            traverseExpression ctx argument
        | Binding(identifier, parameters, body) ->
            let identifierType = ctx.GetIdentifierType(identifier)

            let rec loop identifierType parameters =
                match parameters with
                | [] -> ctx.ConstrainSame(identifierType, body.expressionType)
                | [ parameter ] ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.ConstrainFunction(identifierType, parameterType, body.expressionType)
                | parameter :: parametersRest ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    let subFunctionType = TypeReference($"binding sub-function of ({identifierType})")
                    ctx.ConstrainFunction(identifierType, parameterType, subFunctionType)
                    loop subFunctionType parametersRest

            loop identifierType parameters

            ctx.ConstrainAtom(expression.expressionType, BuiltIn.AtomTypeIds.unit)

            // TODO env
            traverseExpression ctx body

        | Sequence expressions ->
            expressions |> List.iter (traverseExpression ctx)

            match List.tryLast expressions with
            | Some lastExpression -> ctx.ConstrainSame(expression.expressionType, lastExpression.expressionType)
            | None -> ctx.ConstrainAtom(expression.expressionType, BuiltIn.AtomTypeIds.unit)

        | InvalidToken _ -> failwith "TODO"

    let private traverseProgram (ctx: Context) (program: Program) =
        match program with
        | Program expression -> traverseExpression ctx expression

    let traverse (data: InternalData) (ast: Program) = traverseProgram (Context(data)) ast

module private Solve =
    let private applySame (data: InternalData) =
        let mutable updated = false

        let sameConstraints =
            data.constraints
            |> Seq.choose (function
                | Same(a, b) when a <> b -> Some(a, b)
                | _ -> None)
            |> List

        let sameGroups: List<HashSet<TypeReference>> = List()

        for a, b in sameConstraints do
            let groups =
                sameGroups
                |> Seq.filter (fun group -> group.Contains(a) || group.Contains(b))
                |> List.ofSeq

            match groups with
            | [] ->
                let newGroup = HashSet()
                newGroup.Add(a) |> ignore
                newGroup.Add(b) |> ignore
                sameGroups.Add(newGroup)
            | [ group ] ->
                group.Add(a) |> ignore
                group.Add(b) |> ignore
            | group1 :: [ group2 ] ->
                group2 |> Seq.iter (group1.Add >> ignore)
                sameGroups.Remove(group2) |> ignore
            | _ -> failwith "Invalid state"

        let replaceDict = Dictionary()

        for group in sameGroups do
            let group = group |> List.ofSeq
            let firstWithHint = group |> List.tryFind (fun tr -> tr.HasHint)

            let replacement =
                match firstWithHint with
                | Some tr -> tr
                | None -> group.Head

            for item in group.Tail do
                if item <> replacement then
                    replaceDict[item] <- replacement

        if replaceDict.Count > 0 then
            updated <- true

            let replaceTypeReference tr =
                match replaceDict.TryGetValue(tr) with
                | true, newTr -> newTr
                | false, _ -> tr

            let newConstraints = HashSet()

            for constraint in data.constraints do
                match constraint with
                | Same _ -> ()
                | Atom(tr, t) ->
                    let tr = replaceTypeReference tr
                    newConstraints.Add(Atom(tr, t)) |> ignore
                | Function(fn, parameter, result) ->
                    let fn = replaceTypeReference fn
                    let parameter = replaceTypeReference parameter
                    let result = replaceTypeReference result
                    newConstraints.Add(Function(fn, parameter, result)) |> ignore
                | Assignable(to_, from) ->
                    let to_ = replaceTypeReference to_
                    let from = replaceTypeReference from
                    newConstraints.Add(Assignable(to_, from)) |> ignore
                | AssignableToAtom(tr, atomTypeId) ->
                    let tr = replaceTypeReference tr
                    newConstraints.Add(AssignableToAtom(tr, atomTypeId)) |> ignore
                | AssignableFromAtom(tr, atomTypeId) ->
                    let tr = replaceTypeReference tr
                    newConstraints.Add(AssignableFromAtom(tr, atomTypeId)) |> ignore

            data.constraints <- newConstraints

            for kv in data.typeRefReplaceDict :> seq<_> do
                let trFrom = kv.Key
                let trTo = kv.Value

                match replaceDict.TryGetValue(trTo) with
                | true, newTrTo -> data.typeRefReplaceDict[trFrom] <- newTrTo
                | false, _ -> ()

            for kv in replaceDict :> seq<_> do
                data.typeRefReplaceDict[kv.Key] <- kv.Value

        updated

    let private collapseMultipleFunctionIntoSame (data: InternalData) =
        let mutable updated = false

        let functionConstraints = List()

        let functionGroups: Dictionary<TypeReference, List<TypeReference * TypeReference>> =
            Dictionary()

        let newConstraints = HashSet()

        for constraint in data.constraints do
            match constraint with
            | Function(fn, p, r) ->
                functionConstraints.Add(fn, p, r)

                match functionGroups.TryGetValue(fn) with
                | false, _ ->
                    let group = List()
                    functionGroups[fn] <- group
                    group.Add(p, r)
                    newConstraints.Add(constraint) |> ignore
                | true, group ->
                    updated <- true
                    group.Add(p, r)
            | _ -> newConstraints.Add(constraint) |> ignore

        for kv in functionGroups :> seq<_> do
            let group = kv.Value

            if group.Count > 1 then
                group
                |> Seq.pairwise
                |> Seq.iter (fun ((parameterA, resultA), (parameterB, resultB)) ->
                    newConstraints.Add(Same(parameterA, parameterB)) |> ignore
                    newConstraints.Add(Same(resultA, resultB)) |> ignore)

        data.constraints <- newConstraints

        updated

    let private simplifyAssignable (data: InternalData) =
        let mutable updated = false

        let atomTypeRefs = Dictionary()
        let assignableToAtomTypeRefs = Dictionary()
        let assignableFromAtomTypeRefs = Dictionary()

        for constraint in data.constraints do
            match constraint with
            | Atom(tr, atomTypeId) -> atomTypeRefs[tr] <- atomTypeId
            | AssignableToAtom(tr, atomTypeId) -> assignableToAtomTypeRefs[tr] <- atomTypeId
            | AssignableFromAtom(tr, atomTypeId) -> assignableFromAtomTypeRefs[tr] <- atomTypeId
            | _ -> ()

        let newConstraints = HashSet()

        for constraint in data.constraints do
            match constraint with
            | Assignable(to_, from) ->
                let toIsAtom, toAtom = atomTypeRefs.TryGetValue(to_)
                let fromIsAtom, fromAtom = atomTypeRefs.TryGetValue(from)

                match toIsAtom, fromIsAtom with
                | true, true when toAtom = fromAtom -> updated <- true
                | true, false ->
                    updated <- true
                    newConstraints.Add(AssignableToAtom(from, toAtom)) |> ignore
                | false, true ->
                    updated <- true
                    newConstraints.Add(AssignableFromAtom(to_, fromAtom)) |> ignore
                | _ -> newConstraints.Add(constraint) |> ignore
            | AssignableToAtom(tr, toAtomTypeId) ->
                match assignableFromAtomTypeRefs.TryGetValue(tr) with
                | true, fromAtomTypeId when toAtomTypeId = fromAtomTypeId ->
                    updated <- true
                    newConstraints.Add(Atom(tr, toAtomTypeId)) |> ignore
                | _ -> newConstraints.Add(constraint) |> ignore
            | AssignableFromAtom(tr, fromAtomTypeId) ->
                match assignableToAtomTypeRefs.TryGetValue(tr) with
                | true, toAtomTypeId when toAtomTypeId = fromAtomTypeId ->
                    updated <- true
                    newConstraints.Add(Atom(tr, toAtomTypeId)) |> ignore
                | _ -> newConstraints.Add(constraint) |> ignore
            | _ -> newConstraints.Add(constraint) |> ignore

        data.constraints <- newConstraints

        updated

    let solve (data: InternalData) =
        let mutable updated = true

        while updated do
            updated <- applySame data
            updated <- collapseMultipleFunctionIntoSame data || updated
            updated <- applySame data || updated
            updated <- simplifyAssignable data || updated
        ()

let private addBuiltIns (data: InternalData) =
    [ BuiltIn.Identifiers.println, BuiltIn.IdentifierTypes.println
      BuiltIn.Identifiers.intToStr, BuiltIn.IdentifierTypes.intToStr
      BuiltIn.Identifiers.intToStrFmt, BuiltIn.IdentifierTypes.intToStrFmt
      BuiltIn.Identifiers.floatToStr, BuiltIn.IdentifierTypes.floatToStr
      BuiltIn.Identifiers.opAdd, BuiltIn.IdentifierTypes.opAdd
      BuiltIn.Identifiers.opSubtract, BuiltIn.IdentifierTypes.opSubtract
      BuiltIn.Identifiers.opMultiply, BuiltIn.IdentifierTypes.opMultiply
      BuiltIn.Identifiers.opDivide, BuiltIn.IdentifierTypes.opDivide
      ]
    |> List.iter (fun (identifier, t) ->
        let identifierType = data.GetIdentifierType(identifier)

        let rec loop typeRef t =
            match t with
            | AtomType t -> data.constraints.Add(Atom(typeRef, t)) |> ignore
            | FunctionType(parameterT, resultT) ->
                let parameterTypeRef = TypeReference($"parameter of ({typeRef})")
                let resultTypeRef = TypeReference($"result of ({typeRef})")

                data.allTypeReferences.Add(parameterTypeRef) |> ignore
                data.allTypeReferences.Add(resultTypeRef) |> ignore

                data.constraints.Add(Function(typeRef, parameterTypeRef, resultTypeRef))
                |> ignore

                loop parameterTypeRef parameterT
                loop resultTypeRef resultT
            | VariableType _ -> failwith "TODO"

        loop identifierType t)

let private createTypeMap (data: InternalData) =
    let typeMapBuilder = ImmutableDictionary.CreateBuilder()

    let constraintsByTypeRef =
        data.constraints
        |> Seq.groupBy (fun constraint ->
            match constraint with
            | Same _ -> failwith "There should not be Same constraints at this point"
            | Assignable _ -> failwith "There should not be Assignable constraints at this point"
            | Atom(tr, _)
            | Function(tr, _, _)
            | AssignableToAtom(tr, _)
            | AssignableFromAtom(tr, _) -> tr)
        |> Seq.map (fun (tr, cs) -> (tr, List.ofSeq cs))
        |> dict

    let typeReferencesToResolve = List(data.allTypeReferences)

    while typeReferencesToResolve.Count > 0 do
        for i = typeReferencesToResolve.Count - 1 downto 0 do
            let typeReference = typeReferencesToResolve[i]

            let lookupTypeReference =
                match data.typeRefReplaceDict.TryGetValue(typeReference) with
                | true, tr -> tr
                | false, _ -> typeReference

            let constraints = constraintsByTypeRef[lookupTypeReference]

            match constraints with
            | [ Atom(_, atomTypeId) ] ->
                typeMapBuilder.Add(typeReference, NullaryTypeConstructor(AtomType(atomTypeId)))
                typeReferencesToResolve.RemoveAt(i)
            | [ Function(_, parameter, result) ] ->
                let parameterReady, parameterTypeCtor = typeMapBuilder.TryGetValue(parameter)
                let resultReady, resultTypeCtor = typeMapBuilder.TryGetValue(result)

                match parameterReady, resultReady with
                | true, true ->
                    match parameterTypeCtor, resultTypeCtor with
                    | NullaryTypeConstructor parameterType, NullaryTypeConstructor resultType ->
                        typeMapBuilder.Add(typeReference, NullaryTypeConstructor(FunctionType(parameterType, resultType)))
                        typeReferencesToResolve.RemoveAt(i)
                    | _ -> failwith "TODO"
                | _ -> ()
            | _ -> failwith "TODO report error"

    let identifierMapBuilder = ImmutableDictionary.CreateBuilder()

    for kv in data.identifierTypes :> seq<_> do
        let identifier = kv.Key
        let identifierTypeRef = kv.Value

        identifierMapBuilder.Add(identifier, typeMapBuilder[identifierTypeRef])

    { identifierTypes = identifierMapBuilder.ToImmutable()
      typeReferenceTypes = typeMapBuilder.ToImmutable() }

let getTypeMap (ast: Program) : TypeMap =
    let data =
        { allTypeReferences = HashSet()
          identifierTypes = Dictionary()
          constraints = HashSet()
          typeRefReplaceDict = Dictionary() }

    addBuiltIns data

    Traverse.traverse data ast

    Solve.solve data

    createTypeMap data
