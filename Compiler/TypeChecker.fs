module internal rec Compiler.TypeChecker

open System.Collections.Generic
open Compiler.Type
open Compiler.Ast

type private UntypedExpression = TypedExpression<unit>
type private TypedExpression = TypedExpression<Type>

let private (|Is|_|) v1 v2 = if v1 = v2 then Some() else None

type Context =
    { resolveType: Identifier -> Type
      addTyping: Identifier -> Type -> unit }

let private coerce (t: Type) (e: TypedExpression) =
    { expression = Coerce(e, t)
      expressionType = t }

let private mapExpression (ctx: Context) (e: UntypedExpression) =
    match e.expression with
    | Identifier i ->
        let t = ctx.resolveType i

        { expression = Identifier i
          expressionType = t }
    | NumberLiteral(i, f) ->
        let t =
            match f with
            | Some _ -> BuiltInTypes.float
            | None -> BuiltInTypes.int

        { expression = NumberLiteral(i, f)
          expressionType = t }
    | StringLiteral s ->
        { expression = StringLiteral s
          expressionType = BuiltInTypes.string }
    | BinaryOperation(e1, op, e2) ->
        let e1 = mapExpression ctx e1
        let e2 = mapExpression ctx e2

        match e1.expressionType, e2.expressionType with
        | Is BuiltInTypes.string, Is BuiltInTypes.string when op = Add ->
            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltInTypes.string }
        | Is BuiltInTypes.int, Is BuiltInTypes.int ->
            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltInTypes.int }
        | Is BuiltInTypes.float, Is BuiltInTypes.float ->
            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltInTypes.float }
        | Is BuiltInTypes.float, Is BuiltInTypes.int ->
            let e2 = coerce BuiltInTypes.float e2

            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltInTypes.float }
        | Is BuiltInTypes.int, Is BuiltInTypes.float ->
            let e1 = coerce BuiltInTypes.float e1

            { expression = BinaryOperation(e1, op, e2)
              expressionType = BuiltInTypes.float }
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
    | Let(i, v) ->
        let v = mapExpression ctx v
        ctx.addTyping i v.expressionType

        { expression = Let(i, v)
          expressionType = BuiltInTypes.unit }
    | Sequence es ->
        let es = es |> List.map (mapExpression ctx)
        let t = (es |> List.last).expressionType

        { expression = Sequence es
          expressionType = t }

let check (ast: Program<unit>) : Program<Type> =
    let identifierTypes = Dictionary()
    identifierTypes["println"] <- FunctionType(BuiltInTypes.string, BuiltInTypes.unit)
    identifierTypes["intToStr"] <- FunctionType(BuiltInTypes.int, BuiltInTypes.string)
    identifierTypes["intToStr2"] <- FunctionType(BuiltInTypes.int, FunctionType(BuiltInTypes.string, BuiltInTypes.string))
    identifierTypes["floatToStr"] <- FunctionType(BuiltInTypes.float, BuiltInTypes.string)

    let context =
        { resolveType =
            fun i ->
                match identifierTypes.TryGetValue(i) with
                | false, _ -> failwith $"Can't resolve type for identifier %s{i}"
                | true, t -> t
          addTyping =
            fun i t ->
                if not (identifierTypes.TryAdd(i, t)) then
                    failwith $"Can't add type for identifier %s{i}" }

    match ast with
    | Program e -> mapExpression context e |> Program
