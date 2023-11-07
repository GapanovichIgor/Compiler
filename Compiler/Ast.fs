module internal rec Compiler.Ast

open System.Collections.Generic

type Type =
    | ValueType of string
    | FunctionType of Type * Type

module BuiltInTypes =
    let unit = ValueType "System.Unit"
    let int = ValueType "System.Int"
    let float = ValueType "System.Float"
    let string = ValueType "System.String"

type Identifier = string

type BinaryOperator =
    | Add
    | Subtract
    | Multiply
    | Divide

type Expression =
    | Identifier of Identifier
    | IntegerLiteral of int
    | FloatLiteral of int * int
    | StringLiteral of string
    | BinaryOperation of TypedExpression * BinaryOperator * TypedExpression
    | Application of TypedExpression * TypedExpression
    | Coerce of TypedExpression * Type
    | Let of Identifier * TypedExpression * TypedExpression

type TypedExpression =
    { expression: Expression
      expressionType: Type }

type Statement =
    | Expression of TypedExpression

type StatementSequence = Statement list

type Program = Program of StatementSequence

let private (|Is|_|) v1 v2 =
    if v1 = v2
    then Some ()
    else None

type Context =
    { resolveType: Identifier -> Type
      addTyping: Identifier -> Type -> unit }

let private mapBinaryOperator (o: UntypedAst.BinaryOperator) =
    match o with
    | UntypedAst.BinaryOperator.Add -> Add
    | UntypedAst.BinaryOperator.Subtract -> Subtract
    | UntypedAst.BinaryOperator.Multiply -> Multiply
    | UntypedAst.BinaryOperator.Divide -> Divide

let private coerce (t: Type) (e: TypedExpression) =
    { expression = Coerce (e, t)
      expressionType = t }

let private getExpression (ctx: Context) (e: UntypedAst.Expression) =
    match e with
    | UntypedAst.Identifier i ->
        let t = ctx.resolveType i
        { expression = Identifier i; expressionType = t }
    | UntypedAst.NumberLiteral(i, f) ->
        match f with
        | None -> { expression = IntegerLiteral i; expressionType = BuiltInTypes.int }
        | Some f -> { expression = FloatLiteral (i, f); expressionType = BuiltInTypes.float }
    | UntypedAst.StringLiteral s -> { expression = StringLiteral s; expressionType = BuiltInTypes.string }
    | UntypedAst.BinaryOperation(e1, op, e2) ->
        let e1 = getExpression ctx e1
        let op = mapBinaryOperator op
        let e2 = getExpression ctx e2
        match e1.expressionType, e2.expressionType with
        | Is BuiltInTypes.string, Is BuiltInTypes.string when op = Add ->
            { expression = BinaryOperation (e1, op, e2)
              expressionType = BuiltInTypes.string }
        | Is BuiltInTypes.int, Is BuiltInTypes.int ->
            { expression = BinaryOperation (e1, op, e2)
              expressionType = BuiltInTypes.int }
        | Is BuiltInTypes.float, Is BuiltInTypes.float ->
            { expression = BinaryOperation (e1, op, e2)
              expressionType = BuiltInTypes.float }
        | Is BuiltInTypes.float, Is BuiltInTypes.int ->
            let e2 = coerce BuiltInTypes.float e2
            { expression = BinaryOperation (e1, op, e2)
              expressionType = BuiltInTypes.float }
        | Is BuiltInTypes.int, Is BuiltInTypes.float ->
            let e1 = coerce BuiltInTypes.float e1
            { expression = BinaryOperation (e1, op, e2)
              expressionType = BuiltInTypes.float }
        | _ -> failwith "TODO"
    | UntypedAst.Application(e1, e2) ->
        let e1 = getExpression ctx e1
        let e2 = getExpression ctx e2

        match e1.expressionType with
        | FunctionType (argT, resultT) when e2.expressionType = argT ->
            { expression = Application (e1, e2)
              expressionType = resultT }
        | _ -> failwith "TODO"
    | UntypedAst.Let(i, v, body) ->
        let v = getExpression ctx v
        ctx.addTyping i v.expressionType
        let body = getExpression ctx body
        { expression = Let (i, v, body)
          expressionType = body.expressionType }

let private getStatement (ctx: Context) (statement: UntypedAst.Statement) =
    match statement with
    | UntypedAst.Statement.Expression e -> Expression (getExpression ctx e)

let fromUntypedAst (untypedAst: UntypedAst.Program): Program =
    let identifierTypes = Dictionary()
    identifierTypes["print"] <- FunctionType (BuiltInTypes.string, BuiltInTypes.unit)
    identifierTypes["intToStr"] <- FunctionType (BuiltInTypes.int, BuiltInTypes.string)
    identifierTypes["floatToStr"] <- FunctionType (BuiltInTypes.float, BuiltInTypes.string)
    let context =
        { resolveType = fun i ->
            match identifierTypes.TryGetValue(i) with
            | false, _ -> failwith $"Can't resolve type for identifier %s{i}"
            | true, t -> t
          addTyping = fun i t ->
            if not (identifierTypes.TryAdd(i, t)) then
                failwith $"Can't add type for identifier %s{i}" }

    let (UntypedAst.Program statements) = untypedAst
    let statements = statements |> List.map (getStatement context)
    Program statements