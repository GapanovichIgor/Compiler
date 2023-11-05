
open System.IO
open Compiler

let sourceCodeStream = File.OpenRead("Program.txt")

let tokens = Tokenization.tokenize sourceCodeStream

Build.build (null, "Build")