open System.IO
open Compiler
open Compiler.Parser
open Compiler.Tokenization

[<EntryPoint>]
let main _ =
    let sourceCodeStream = File.OpenRead("Program.txt")

    let tokens, tokenToCharSourceMap = tokenize sourceCodeStream

    match parse tokens with
    | Error e -> failwith "TODO"
    | Ok parseTree ->
        let ast = AstBuilder.buildFromParseTree parseTree

        let typeMap = TypeSolver.getTypeInformation ast

        let csAst = CsTranspiler.transpile (ast, typeMap)

        Build.build (csAst, "Build")

        0
