module rec Compiler.CsCodeGenerator

open System.IO
open Compiler.CsAst

type private Output(streamWriter: StreamWriter) =
    let streamWriter = streamWriter

    member _.Write(str: string) =
        streamWriter.Write(str)

    member _.Write(char: char) =
        streamWriter.Write(char)

    member _.WriteLine(str: string) =
        streamWriter.WriteLine(str)

let private getTypeSignature (t: Type) =
    match t with
    | Void -> "void"
    | ValueType t -> t
    | FunctionType (args, result) ->
        match result with
        | Void ->
            if args.Length = 0 then
                "System.Action"
            else
                args
                |> List.map getTypeSignature
                |> String.concat ", "
                |> sprintf "System.Action<%s>"
        | _ ->
            if args.Length = 0 then
                $"System.Func<%s{getTypeSignature result}>"
            else
                let args =
                    args
                    |> List.map getTypeSignature
                    |> String.concat ", "
                $"System.Func<%s{args}, %s{getTypeSignature result}>"


let private generateBinaryOperator (operator: BinaryOperator) (output: Output) =
    match operator with
    | Add -> output.Write("+")
    | Subtract -> output.Write("-")
    | Multiply -> output.Write("*")
    | Divide -> output.Write("/")

let private generateFunctionCallExpression (functionName: Identifier, args: Expression list) (output: Output) =
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

let private generateCast (e: Expression, t: Type) (output: Output) =
    output.Write("(")
    output.Write("(")
    output.Write(getTypeSignature t)
    output.Write(")")
    generateExpression e output
    output.Write(")")

let rec private generateExpression (expression: Expression) (output: Output) =
    match expression with
    | Expression.Identifier i ->
        output.Write(i)
    | Expression.NumberLiteral (i, f, t) ->
        match t with
        | ValueType "System.Int32" ->
            output.Write(string i)
        | ValueType "System.Single" ->
            output.Write(string i)
            output.Write(".")
            match f with
            | Some f -> output.Write(string f)
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

let private generateVar (t: Type, i: Identifier, v: Expression) (output: Output) =
    output.Write(getTypeSignature t)
    output.Write(" ")
    output.Write(i)
    output.Write(" = ")
    generateExpression v output

let private generateStatement (statement: Statement) (output: Output) =
    match statement with
    | Statement.FunctionCall (functionName, args) -> generateFunctionCallExpression (functionName, args) output
    | Statement.Var (t, i, v) -> generateVar (t, i, v) output
    output.WriteLine(";")

let generate (program: Program) (output: Stream) : unit =
    let streamWriter = new StreamWriter(output)
    let output = Output(streamWriter)
    let (Program statements) = program
    for statement in statements do
        generateStatement statement output
    streamWriter.Flush()