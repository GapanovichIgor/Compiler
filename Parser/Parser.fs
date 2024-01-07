module Parser.Parser

open System.IO
open Compiler
open Compiler.ParserInternal

let parseAst (stream: Stream) : Result<Ast.Program, ParseError> =
    let tokens = Tokenization.tokenize stream

    ParserInternal.parse tokens
    |> Result.map AstBuilder.buildFromParseTree
