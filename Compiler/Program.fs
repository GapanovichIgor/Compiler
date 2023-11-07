
open System.IO
open Compiler

let sourceCodeStream = File.OpenRead("Program.txt")

let tokens = Tokenization.tokenize sourceCodeStream

match tokens with
| Error e -> failwith ""
| Ok tokens ->

let parseTree = Parser.parse tokens
match parseTree with
| Error e -> failwith ""
| Ok parseTree ->

Build.build (parseTree, "Build")