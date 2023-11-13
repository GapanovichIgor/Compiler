﻿module internal rec Compiler.TypeSolver

open System.Collections.Generic
open System.Diagnostics
open Compiler.Ast
open Compiler.Type

[<DebuggerDisplay("{ToString()}")>]
type private TypeShape =
    | Void
    | Conflict of TypeShape Set
    | Unknown
    | FixedType of TypeIdentifier
    | FunctionType of parameter: TypeShape * result: TypeShape

    override this.ToString() =
        match this with
        | Void -> "#void"
        | Conflict shapes ->
            let shapeText = shapes |> Seq.map string |> String.concat " & "
            $"#conflict({shapeText})"
        | Unknown -> "#unknown"
        | FixedType t -> t.ToString()
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
    | FunctionIdentifier of parameter: TypeConstraintSet * result: TypeConstraintSet
    | FunctionParameter of fnIdentifier: TypeConstraintSet
    | FunctionResult of fnIdentifier: TypeConstraintSet

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
        | Same other -> other.ConstrainShape(this.Shape)
        | ApplicationFunction(argument, result) ->
            this.ConstrainShape(FunctionType(argument.Shape, result.Shape))

            match this.Shape with
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
        | FunctionIdentifier(parameter, result) ->
            this.ConstrainShape(FunctionType(parameter.Shape, result.Shape))

            match this.Shape with
            | FunctionType(parameterShape, resultShape) ->
                parameter.ConstrainShape(parameterShape)
                result.ConstrainShape(resultShape)
            | _ ->
                parameter.ConstrainShape(Void)
                result.ConstrainShape(Void)
        | FunctionParameter fn ->
            fn.ConstrainShape(FunctionType(this.Shape, Unknown))

            match fn.Shape with
            | FunctionType(parameterShape, _) -> this.ConstrainShape(parameterShape)
            | _ -> this.ConstrainShape(Void)
        | FunctionResult fn ->
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
            | FixedType t -> Some(Type.FixedType t)
            | FunctionType(p, r) ->
                match mapShapeToType p, mapShapeToType r with
                | Some p, Some r -> Some(Type.FunctionType(p, r))
                | _ -> None

        mapShapeToType shape

[<DebuggerDisplay("{ToString()}")>]
type private TypeContext() =
    let identifierTypeReference = Dictionary<Identifier, TypeReference>()
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
        | Type.FixedType t -> FixedType t
        | Type.FunctionType(p, r) -> FunctionType(mapToTypeShape p, mapToTypeShape r)

    member _.GetIdentifierTypeReference(identifier: Identifier) =
        match identifierTypeReference.TryGetValue(identifier) with
        | true, t -> t
        | false, _ ->
            let typeReference = TypeReference.Create()
            identifierTypeReference[identifier] <- typeReference
            typeReference

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

    member _.ConstrainFunctionDefinition(functionIdentifier: TypeReference, parameter: TypeReference, result: TypeReference) =
        let functionIdentifier = getConstraints functionIdentifier
        let parameter = getConstraints parameter
        let result = getConstraints result
        functionIdentifier.AddRelation(FunctionIdentifier(parameter, result))
        parameter.AddRelation(FunctionParameter functionIdentifier)
        result.AddRelation(FunctionResult functionIdentifier)

    member _.GetTypeInformation() : TypeInformation =
        let typeReferenceTypes =
            constraints
            |> Seq.map (fun kv ->
                let typeReference = kv.Key
                let constraintSet = kv.Value
                match constraintSet.Resolve() with
                | Some t -> typeReference, t
                | None -> failwith "TODO")
            |> Map.ofSeq

        let identifierTypes =
            identifierTypeReference
            |> Seq.map (fun kv ->
                let identifier = kv.Key
                let typeReference = kv.Value
                identifier, typeReferenceTypes[typeReference])
            |> Map.ofSeq

        { typeReferenceTypes = typeReferenceTypes
          identifierTypes = identifierTypes }

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

        identifierTypeReference
        |> Seq.map (fun kv -> $"{kv.Key} : {constraintSetInfo[constraints[kv.Value]].name}")
        |> String.concat "\n"

let private traverseExpression (ctx: TypeContext) (e: Expression) =
    match e.expressionShape with
    | IdentifierReference identifier ->
        let identifierType = ctx.GetIdentifierTypeReference(identifier)
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
    | Binding(identifier, parameters, bindingValue) ->
        let identifierType = ctx.GetIdentifierTypeReference(identifier)
        let parameterTypes = parameters |> List.map ctx.GetIdentifierTypeReference

        let rec loop identifierType parameterTypes =
            match parameterTypes with
            | [] ->
                // let identifier = bindingValue
                ctx.ConstrainSame(identifierType, bindingValue.expressionType)
            | [ parameterType ] ->
                // let identifier parameter = bindingValue
                ctx.ConstrainFunctionDefinition(identifierType, parameterType, bindingValue.expressionType)
            | parameterType :: parameterTypesRest ->
                // let identifier (parameter : 'parameterType) : 'subFunctionType =
                //     let subFunctionIdentifier : 'subFunctionType = fun ... -> bindingValue
                //     subFunctionIdentifier
                let subFunctionIdentifierType = TypeReference.Create()
                ctx.ConstrainFunctionDefinition(identifierType, parameterType, subFunctionIdentifierType)
                loop subFunctionIdentifierType parameterTypesRest
        loop identifierType parameterTypes

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

type TypeInformation =
    { typeReferenceTypes: Map<TypeReference, Type>
      identifierTypes: Map<Identifier, Type> }

let getTypeInformation (ast: Program) : TypeInformation =
    let context = TypeContext()

    let addBuiltIn (identifier: Identifier) (t: Type) =
        let typeReference = context.GetIdentifierTypeReference(identifier)
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

    context.GetTypeInformation()