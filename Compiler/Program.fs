
open System.IO
open Compiler

let sourceCodeStream = File.OpenRead("Program.txt")

let tokens = Tokenization.tokenize sourceCodeStream

match tokens with
| Error e -> failwith ""
| Ok tokens ->

let ast = Ast.parse tokens
match ast with
| Error e -> failwith ""
| Ok ast ->

Build.build (ast, "Build")