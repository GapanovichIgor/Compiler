﻿module internal rec Compiler.TranspileToCs

open System.Collections.Generic

type private Context =
    { addVar: CsAst.Type -> CsAst.Identifier -> CsAst.Expression -> unit }

let private mapBinaryOperator (o: Ast.BinaryOperator) =
    match o with
    | Ast.BinaryOperator.Add -> CsAst.BinaryOperator.Add
    | Ast.BinaryOperator.Subtract -> CsAst.BinaryOperator.Subtract
    | Ast.BinaryOperator.Multiply -> CsAst.BinaryOperator.Multiply
    | Ast.BinaryOperator.Divide -> CsAst.BinaryOperator.Divide

let private (|Is|_|) v1 v2 =
    if v1 = v2
    then Some ()
    else None

let private mapType (t: Ast.Type): CsAst.Type =
    match t with
    | Is Ast.BuiltInTypes.int -> "System.Int32"
    | Is Ast.BuiltInTypes.float -> "System.Single"
    | Is Ast.BuiltInTypes.string -> "System.String"
    | Is Ast.BuiltInTypes.unit -> failwith "TODO handle void"
    | _ -> failwith "TODO handle other types"

let private getExpression (ctx: Context) (e: Ast.TypedExpression) =
    match e.expression with
    | Ast.Identifier i -> CsAst.Identifier i
    | Ast.IntegerLiteral i -> CsAst.IntegerLiteral i
    | Ast.FloatLiteral (i, f) -> CsAst.FloatLiteral (i, f)
    | Ast.StringLiteral s -> CsAst.StringLiteral s
    | Ast.BinaryOperation (e1, op, e2) ->
        let e1 = getExpression ctx e1
        let op = mapBinaryOperator op
        let e2 = getExpression ctx e2
        CsAst.BinaryOperation (e1, op, e2)
    | Ast.Application (f, arg) ->
        match f.expression with
        | Ast.Identifier i ->
            let arg = getExpression ctx arg
            CsAst.FunctionCall (i, [arg])
        | _ -> failwith "TODO handle case when function is not represented by an identifier"
    | Ast.Coerce (e, t) ->
        let e = getExpression ctx e
        let t = mapType t
        CsAst.Cast (e, t)
    | Ast.Let (i, v, body) ->
        let t = mapType v.expressionType
        let v = getExpression ctx v
        ctx.addVar t i v
        getExpression ctx body

let private getStatement (s: Ast.Statement): CsAst.Statement list =
    match s with
    | Ast.Statement.Expression e ->
        match e.expression with
        | Ast.Application (f, arg) ->
            match f.expression with
            | Ast.Identifier i ->
                let statements = List()
                let context =
                    { addVar = fun t i v ->
                        statements.Add(CsAst.Statement.Var (t, i, v)) }

                let arg = getExpression context arg

                statements.Add(CsAst.Statement.FunctionCall (i, [arg]))

                List.ofSeq statements
            | _ -> failwith "TODO handle case when function is not represented by an identifier"
        | _ ->
            failwith "TODO handle statements beside function calls"

let transpile (ast: Ast.Program): CsAst.Program =
    let (Ast.Program statements) = ast

    let statements = statements |> List.collect getStatement

    CsAst.Program statements