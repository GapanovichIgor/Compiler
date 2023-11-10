
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

let untypedAst = AstBuilder.buildFromParseTree parseTree

let ast = TypeChecker.check untypedAst

let csAst = TranspileToCs.transpile ast

Build.build (csAst, "Build")