
open System.IO
open Compiler
open Compiler.Parser
open Compiler.Tokenization

let sourceCodeStream = File.OpenRead("Program.txt")

match tokenize sourceCodeStream with
| Error e -> failwith "TODO"
| Ok tokens ->

let parserInput =
    tokens
    |> List.map (function
        | TNumberLiteral (i, f) -> InputItem.NumberLiteral (i, f)
        | TDoubleQuotedString s -> InputItem.DoubleQuotedString s
        | TPlus -> InputItem.Plus ()
        | TMinus -> InputItem.Minus ()
        | TAsterisk -> InputItem.Asterisk ()
        | TSlash -> InputItem.Slash ()
        | TParenOpen -> InputItem.ParenOpen ()
        | TParenClose -> InputItem.ParenClose ()
        | TIdentifier i -> InputItem.Identifier i
        | TInvalid t -> failwith "todo"
        | TNewLine -> failwith "todo"
        | TBlockOpen -> failwith "todo"
        | TBlockClose -> failwith "todo")

match parse parserInput with
| Error e -> failwith "TODO"
| Ok parseTree ->

let untypedAst = UntypedAst.fromParseTree parseTree

let ast = Ast.fromUntypedAst untypedAst

let csAst = TranspileToCs.transpile ast

Build.build (csAst, "Build")