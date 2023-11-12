
open System.IO
open Compiler
open Compiler.Parser
open Compiler.Tokenization

let sourceCodeStream = File.OpenRead("Program.txt")

match tokenize sourceCodeStream with
| Error e -> failwith "TODO"
| Ok tokens ->

match parse tokens with
| Error e -> failwith "TODO"
| Ok parseTree ->

let ast = AstBuilder.buildFromParseTree parseTree

let typeMap = TypeSolver.getTypeMap ast

let csAst = CsTranspiler.transpile (ast, typeMap)

Build.build (csAst, "Build")