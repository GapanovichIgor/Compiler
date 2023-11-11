module internal rec Compiler.TypeChecker

open System.Collections.Generic
open Compiler.Type
open Compiler.Ast

type private UntypedExpression = TypedExpression<unit>
type private TypedExpression = TypedExpression<Type>

let private (|Is|_|) v1 v2 = if v1 = v2 then Some() else None

type TypeContext =
    { getType: Identifier -> Type
      setType: Identifier -> Type -> unit }

let private coerce (t: Type) (e: TypedExpression) =
    { expression = Coerce(e, t)
      expressionType = t }

let private mapExpression (ctx: TypeContext) (e: UntypedExpression) =
    match e.expression with
    | Identifier i ->
        let t = ctx.getType i

        { expression = Identifier i
          expressionType = t }
    | NumberLiteral(i, f) ->
        let t =
            match f with
            | Some _ -> BuiltIn.Types.float
            | None -> BuiltIn.Types.int

        { expression = NumberLiteral(i, f)
          expressionType = t }
    | StringLiteral s ->
        { expression = StringLiteral s
          expressionType = BuiltIn.Types.string }
    | BinaryOperation(e1, op, e2) ->
        let e1 = mapExpression ctx e1
        let e2 = mapExpression ctx e2

        match e1.expressionType, e2.expressionType with
        | Is BuiltIn.Types.string, Is BuiltIn.Types.string when op = Add ->
            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltIn.Types.string }
        | Is BuiltIn.Types.int, Is BuiltIn.Types.int ->
            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltIn.Types.int }
        | Is BuiltIn.Types.float, Is BuiltIn.Types.float ->
            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltIn.Types.float }
        | Is BuiltIn.Types.float, Is BuiltIn.Types.int ->
            let e2 = coerce BuiltIn.Types.float e2

            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltIn.Types.float }
        | Is BuiltIn.Types.int, Is BuiltIn.Types.float ->
            let e1 = coerce BuiltIn.Types.float e1

            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltIn.Types.float }
        | _ -> failwith "TODO"
    | Application(e1, e2) ->
        let e1 = mapExpression ctx e1
        let e2 = mapExpression ctx e2

        match e1.expressionType with
        | FunctionType(argT, resultT) when e2.expressionType = argT ->
            { expression = Application(e1, e2)
              expressionType = resultT }
        | _ -> failwith "TODO"
    | Coerce(e, t) ->
        let e = mapExpression ctx e

        { expression = Coerce(e, t)
          expressionType = t }
    | Binding(i, v) ->
        let v = mapExpression ctx v
        ctx.setType i v.expressionType

        { expression = Binding(i, v)
          expressionType = BuiltIn.Types.unit }
    | Sequence es ->
        let es = es |> List.map (mapExpression ctx)
        let t = (es |> List.last).expressionType

        { expression = Sequence es
          expressionType = t }

let check (ast: Program<unit>) : Program<Type> =
    let identifierTypes = Dictionary()
    identifierTypes[BuiltIn.Identifiers.println] <- FunctionType(BuiltIn.Types.string, BuiltIn.Types.unit)
    identifierTypes[BuiltIn.Identifiers.intToStr] <- FunctionType(BuiltIn.Types.int, BuiltIn.Types.string)
    identifierTypes[BuiltIn.Identifiers.intToStr2] <- FunctionType(BuiltIn.Types.int, FunctionType(BuiltIn.Types.string, BuiltIn.Types.string))
    identifierTypes[BuiltIn.Identifiers.floatToStr] <- FunctionType(BuiltIn.Types.float, BuiltIn.Types.string)

    let context =
        { getType =
            fun i ->
                match identifierTypes.TryGetValue(i) with
                | false, _ -> failwith $"Can't resolve type for identifier %s{i.name}"
                | true, t -> t
          setType =
            fun i t ->
                if not (identifierTypes.TryAdd(i, t)) then
                    failwith $"Can't add type for identifier %s{i.name}" }

    match ast with
    | Program e -> mapExpression context e |> Program
