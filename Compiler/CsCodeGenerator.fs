﻿module rec Compiler.CsCodeGenerator

open System.IO
open Compiler.CsAst

let private generateBinaryOperator (operator: BinaryOperator) (output: StreamWriter) =
    match operator with
    | Add -> output.Write("+")
    | Subtract -> output.Write("-")
    | Multiply -> output.Write("*")
    | Divide -> output.Write("/")

let private generateFunctionCallExpression (functionName: Identifier, args: Expression list) (output: StreamWriter) =
    output.Write(functionName)
    output.Write("(")
    let mutable firstArgument = true
    for arg in args do
        if firstArgument then
            firstArgument <- false
        else
            output.Write(", ")

        generateExpression arg output
    output.Write(")")

let private generateCast (e: Expression, t: Type) (output: StreamWriter) =
    output.Write("(")
    output.Write("(")
    output.Write(t)
    output.Write(")")
    generateExpression e output
    output.Write(")")

let rec private generateExpression (expression: Expression) (output: StreamWriter) =
    match expression with
    | Expression.Identifier i ->
        output.Write(i)
    | Expression.NumberLiteral (i, f, t) ->
        match t with
        | "System.Int32" ->
            output.Write(i)
        | "System.Single" ->
            output.Write(i)
            output.Write(".")
            match f with
            | Some f -> output.Write(f)
            | None -> output.Write("0")
            output.Write("f")
        | _ ->
            failwith "TODO handle other numeric types"
    | Expression.StringLiteral s ->
        output.Write('"')
        let s = s.Replace("\"", "\\\"")
        output.Write(s)
        output.Write('"')
    | Expression.BinaryOperation (e1, op, e2) ->
        output.Write("(")
        generateExpression e1 output
        output.Write(" ")
        generateBinaryOperator op output
        output.Write(" ")
        generateExpression e2 output
        output.Write(")")
    | Expression.FunctionCall (functionName, args) ->
        generateFunctionCallExpression (functionName, args) output
    | Expression.Cast (e, t) ->
        generateCast (e, t) output

let private generateVar (t: Type, i: Identifier, v: Expression) (output: StreamWriter) =
    output.Write(t)
    output.Write(" ")
    output.Write(i)
    output.Write(" = ")
    generateExpression v output

let private generateStatement (statement: Statement) (output: StreamWriter) =
    match statement with
    | Statement.FunctionCall (functionName, args) -> generateFunctionCallExpression (functionName, args) output
    | Statement.Var (t, i, v) -> generateVar (t, i, v) output
    output.WriteLine(";")

let generate (program: Program) (output: Stream) : unit =
    let output = new StreamWriter(output)
    let (Program statements) = program
    for statement in statements do
        generateStatement statement output
    output.Flush()