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
    | AtomType t -> t
    | FunctionDelegate (parameters, result) ->
        match result with
        | Some result ->
            if parameters.Length = 0 then
                $"System.Func<%s{getTypeSignature result}>"
            else
                let args =
                    parameters
                    |> Seq.map getTypeSignature
                    |> String.concat ", "
                $"System.Func<%s{args}, %s{getTypeSignature result}>"
        | None ->
            if parameters.Length = 0 then
                "System.Action"
            else
                parameters
                |> Seq.map getTypeSignature
                |> String.concat ", "
                |> sprintf "System.Action<%s>"

let private getTypeOrVoidSignature (t: Type option) =
    match t with
    | None -> "void"
    | Some t -> getTypeSignature t

let private generateBinaryOperator (operator: BinaryOperator) (output: Output) =
    match operator with
    | Add -> output.Write("+")
    | Subtract -> output.Write("-")
    | Multiply -> output.Write("*")
    | Divide -> output.Write("/")

let private generateFunctionCallExpression (functionName: Identifier, tArgs: Type list, args: Expression list) (output: Output) =
    output.Write(functionName)
    if tArgs.Length > 0 then
        output.Write("<")
        let mutable firstArgument = true
        for tArg in tArgs do
            if firstArgument then
                firstArgument <- false
            else
                output.Write(", ")

            output.Write(getTypeSignature tArg)
        output.Write(">")
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
    | Expression.IdentifierReference i ->
        output.Write(i)
    | Expression.NumberLiteral (i, f, t) ->
        match t with
        | AtomType a when a = CsBuiltIn.AtomTypeIdentifiers.int32 ->
            output.Write(string i)
        | AtomType a when a = CsBuiltIn.AtomTypeIdentifiers.single ->
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
    | Expression.FunctionCall (functionName, tArgs, args) ->
        generateFunctionCallExpression (functionName, tArgs, args) output
    | Expression.Cast (e, t) ->
        generateCast (e, t) output

let private generateFunctionCallStatement (functionName: Identifier, tArgs: Type list, args: Expression list) (output: Output) =
    generateFunctionCallExpression (functionName, tArgs, args) output
    output.WriteLine(";")

let private generateLocalFunction (resultType: Type option, functionName: Identifier, typeParameters: TypeIdentifier list, parameters: (Type * Identifier) list, statements: StatementSequence) (output: Output) =
    output.Write(getTypeOrVoidSignature resultType)
    output.Write(" ")
    output.Write(functionName)
    if typeParameters.Length > 0 then
        output.Write("<")
        let mutable firstTypeParameter = true
        for typeParameter in typeParameters do
            if firstTypeParameter then
                firstTypeParameter <- false
            else
                output.Write(", ")
            output.Write(typeParameter)
        output.Write(">")
    output.Write("(")
    let mutable firstParameter = true
    for paramType, paramName in parameters do
        if firstParameter then
            firstParameter <- false
        else
            output.Write(", ")
        output.Write(getTypeSignature paramType)
        output.Write(" ")
        output.Write(paramName)
    output.WriteLine(") {")
    generateStatements statements output
    output.WriteLine("}")

let private generateVar (t: Type, i: Identifier, v: Expression) (output: Output) =
    output.Write(getTypeSignature t)
    output.Write(" ")
    output.Write(i)
    output.Write(" = ")
    generateExpression v output
    output.WriteLine(";")

let private generateReturn (e: Expression) (output: Output) =
    output.Write("return ")
    generateExpression e output
    output.WriteLine(";")

let private generateStatement (statement: Statement) (output: Output) =
    match statement with
    | Statement.FunctionCall (functionName, tArgs, args) -> generateFunctionCallStatement (functionName, tArgs, args) output
    | Statement.LocalFunction (resultType, functionName, typeParameters, parameters, statements) -> generateLocalFunction (resultType, functionName, typeParameters, parameters, statements) output
    | Statement.Var (t, i, v) -> generateVar (t, i, v) output
    | Statement.Return e -> generateReturn e output

let private generateStatements (statements: StatementSequence) (output: Output) =
    for statement in statements do
        generateStatement statement output

let generate (program: Program) (output: Stream) : unit =
    let streamWriter = new StreamWriter(output)
    let output = Output(streamWriter)
    let (Program statements) = program
    generateStatements statements output
    streamWriter.Flush()