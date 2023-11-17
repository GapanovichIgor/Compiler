module internal Compiler.AstDiagnostics

open Compiler.Ast
open Compiler.Diagnostics

let rec private traverseExpression (e: Expression) : Problem list =
    match e.expressionShape with
    | IdentifierReference _
    | NumberLiteral _
    | StringLiteral _ -> []
    | Application(fn, argument) -> traverseExpression fn @ traverseExpression argument
    | Binding(_, _, body) -> traverseExpression body
    | Sequence expressions -> expressions |> List.map traverseExpression |> List.concat
    | InvalidToken text ->
        [ { level = LevelError
            positionInSource = e.positionInSource
            description = $"Invalid token '%s{text}'" } ]

let get (ast: Program) : Diagnostics =
    let (Program e) = ast

    let problems = traverseExpression e

    { problems = problems }
