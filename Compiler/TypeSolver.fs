module internal rec Compiler.TypeSolver

open System.Collections.Generic
open System.Diagnostics
open Compiler.Type
open Compiler.Ast

[<DebuggerDisplay("{ToString()}")>]
type private TypeShape =
    | Void
    | Conflict of TypeShape Set
    | Unknown
    | ValueType of string
    | FunctionType of parameter: TypeShape * result: TypeShape

    override this.ToString() =
        match this with
        | Void -> "#void"
        | Conflict shapes ->
            let shapeText = shapes |> Seq.map string |> String.concat " & "
            $"#conflict({shapeText})"
        | Unknown -> "#unknown"
        | ValueType t -> t.ToString()
        | FunctionType(arg, result) ->
            let arg =
                match arg with
                | FunctionType _ -> $"({arg})"
                | _ -> arg.ToString()
            $"{arg} -> {result}"

    static member Combine(a: TypeShape, b: TypeShape) =
        match a, b with
        | a, b when a = b -> a
        | Conflict a, Conflict b -> Conflict(a + b)
        | Conflict a, b -> Conflict(a |> Set.add b)
        | a, Conflict b -> Conflict(b |> Set.add a)
        | Unknown, b -> b
        | a, Unknown -> a
        | FunctionType(parameterA, resultA), FunctionType(parameterB, resultB) ->
            let parameter = TypeShape.Combine(parameterA, parameterB)
            let result = TypeShape.Combine(resultA, resultB)
            FunctionType(parameter, result)
        | a, b -> Conflict(set [ a; b ])

type private TypeRelation =
    | Same of TypeConstraintSet
    | ApplicationFunction of arg: TypeConstraintSet * result: TypeConstraintSet
    | ApplicationArgument of fn: TypeConstraintSet
    | ApplicationResult of fn: TypeConstraintSet

[<DebuggerDisplay("{ToString()}")>]
type private TypeConstraintSet() =
    let mutable shape = Unknown
    let relations = HashSet()

    override _.ToString() = shape.ToString()

    member private _.Shape = shape

    member private _.Relations = relations

    member this.ConstrainShape(newShapeInfo: TypeShape) =
        let newShape = TypeShape.Combine(shape, newShapeInfo)

        if shape <> newShape then
            shape <- newShape
            this.PropagateShapeChangeAll()

    member this.PropagateShapeChangeAll() =
        for r in relations do
            this.PropagateShapeChange(r)

    member this.PropagateShapeChange(relation: TypeRelation) =
        match relation with
        | Same other -> other.ConstrainShape(shape)
        | ApplicationFunction(argument, result) ->
            this.ConstrainShape(FunctionType(argument.Shape, result.Shape))

            match shape with
            | FunctionType(parameterShape, resultShape) ->
                argument.ConstrainShape(parameterShape)
                result.ConstrainShape(resultShape)
            | _ ->
                argument.ConstrainShape(Void)
                result.ConstrainShape(Void)
        | ApplicationArgument fn ->
            fn.ConstrainShape(FunctionType(this.Shape, Unknown))

            match fn.Shape with
            | FunctionType(parameterShape, _) -> this.ConstrainShape(parameterShape)
            | _ -> this.ConstrainShape(Void)
        | ApplicationResult fn ->
            fn.ConstrainShape(FunctionType(Unknown, this.Shape))

            match fn.Shape with
            | FunctionType(_, resultShape) -> this.ConstrainShape(resultShape)
            | _ -> this.ConstrainShape(Void)

    member this.AddRelation(relation: TypeRelation) =
        if relations.Add(relation) then
            this.PropagateShapeChange(relation)

    member this.Resolve() =
        let rec mapShapeToType shape =
            match shape with
            | Void
            | Conflict _
            | Unknown -> None
            | ValueType t -> Some(Type.ValueType t)
            | FunctionType(p, r) ->
                match mapShapeToType p, mapShapeToType r with
                | Some p, Some r -> Some(Type.FunctionType(p, r))
                | _ -> None

        mapShapeToType shape

[<DebuggerDisplay("{ToString()}")>]
type private TypeContext() =
    let identifierTypes = Dictionary<Identifier, TypeReference>()
    let constraints = Dictionary<TypeReference, TypeConstraintSet>()

    let getConstraints (typeReference: TypeReference) =
        match constraints.TryGetValue(typeReference) with
        | true, constraints -> constraints
        | false, _ ->
            let c = TypeConstraintSet()
            constraints[typeReference] <- c
            c

    let rec mapToTypeShape (t: Type) : TypeShape =
        match t with
        | Type.ValueType t -> ValueType t
        | Type.FunctionType(p, r) -> FunctionType(mapToTypeShape p, mapToTypeShape r)

    member _.GetIdentifierType(identifier: Identifier) =
        match identifierTypes.TryGetValue(identifier) with
        | true, t -> t
        | false, _ ->
            let t = createTypeReference ()
            identifierTypes[identifier] <- t
            t

    member _.ConstrainExact(typeReference: TypeReference, t: Type) =
        let constraints = getConstraints typeReference
        constraints.ConstrainShape(mapToTypeShape t)

    member _.ConstrainSame(t1: TypeReference, t2: TypeReference) =
        let t1Constraints = getConstraints t1
        let t2Constraints = getConstraints t2
        t1Constraints.AddRelation(Same t2Constraints)
        t2Constraints.AddRelation(Same t1Constraints)

    member _.ConstrainApplication(fn: TypeReference, arg: TypeReference, result: TypeReference) =
        let fn = getConstraints fn
        let arg = getConstraints arg
        let result = getConstraints result
        fn.AddRelation(ApplicationFunction(arg, result))
        arg.AddRelation(ApplicationArgument fn)
        result.AddRelation(ApplicationResult fn)

    member _.GetTypeMap() : TypeMap =
        constraints
        |> Seq.map (fun kv ->
            let t =
                match kv.Value.Resolve() with
                | Some t -> t
                | None -> failwith "TODO"

            kv.Key, t)
        |> Map.ofSeq

    override _.ToString() =
        let constraintSetInfo =
            constraints.Values :> seq<_>
            |> Seq.distinct
            |> Seq.map (fun cs ->
                let name, isSolved =
                    match cs.Resolve() with
                    | Some t -> t.ToString(), true
                    | None -> cs.ToString(), false

                cs, {| name = name; isSolved = isSolved |})
            |> dict

        identifierTypes
        |> Seq.map (fun kv -> $"{kv.Key.name} : {constraintSetInfo[constraints[kv.Value]].name}")
        |> String.concat "\n"

let private traverseExpression (ctx: TypeContext) (e: Expression) =
    match e.expressionShape with
    | IdentifierReference identifier ->
        let identifierType = ctx.GetIdentifierType(identifier)
        ctx.ConstrainSame(e.expressionType, identifierType)
    | NumberLiteral(_, f) ->
        let t =
            match f with
            | Some _ -> BuiltIn.Types.float
            | None -> BuiltIn.Types.int

        ctx.ConstrainExact(e.expressionType, t)
    | StringLiteral _ -> ctx.ConstrainExact(e.expressionType, BuiltIn.Types.string)
    | Application(fn, argument) ->
        ctx.ConstrainApplication(fn.expressionType, argument.expressionType, e.expressionType)
        traverseExpression ctx fn
        traverseExpression ctx argument
    | Binding(identifier, bindingValue) ->
        let identifierType = ctx.GetIdentifierType(identifier)
        ctx.ConstrainSame(identifierType, bindingValue.expressionType)
        ctx.ConstrainExact(e.expressionType, BuiltIn.Types.unit)
        traverseExpression ctx bindingValue
    | Sequence expressions ->
        let lastExpression = expressions |> List.last
        ctx.ConstrainSame(lastExpression.expressionType, e.expressionType)

        for e in expressions do
            traverseExpression ctx e

let private traverseProgram (ctx: TypeContext) (program: Program) =
    match program with
    | Program e -> traverseExpression ctx e

type TypeMap = Map<TypeReference, Type>

let getTypeMap (ast: Program) : TypeMap =
    let context = TypeContext()

    let addBuiltIn (identifier: Identifier) (t: Type) =
        let typeReference = context.GetIdentifierType(identifier)
        context.ConstrainExact(typeReference, t)

    addBuiltIn BuiltIn.Identifiers.opAdd BuiltIn.IdentifierTypes.opAdd
    addBuiltIn BuiltIn.Identifiers.opSubtract BuiltIn.IdentifierTypes.opSubtract
    addBuiltIn BuiltIn.Identifiers.opMultiply BuiltIn.IdentifierTypes.opMultiply
    addBuiltIn BuiltIn.Identifiers.opDivide BuiltIn.IdentifierTypes.opDivide
    addBuiltIn BuiltIn.Identifiers.println BuiltIn.IdentifierTypes.println
    addBuiltIn BuiltIn.Identifiers.intToStr BuiltIn.IdentifierTypes.intToStr
    addBuiltIn BuiltIn.Identifiers.intToStrFmt BuiltIn.IdentifierTypes.intToStrFmt
    addBuiltIn BuiltIn.Identifiers.floatToStr BuiltIn.IdentifierTypes.floatToStr

    traverseProgram context ast

    context.GetTypeMap()
