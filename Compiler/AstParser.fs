module Compiler.AstParser

open Compiler.AstParserGenerated
open Compiler.Tokenization

let private toTerminal (token: Token) : InputItem =
    match token with
    | TNumberLiteral n -> InputItem.NumberLiteral n
    | TInvalid t -> failwith $"Invalid token {t}"
    | TBreak -> failwith "todo"
    | TBlockOpen -> failwith "todo"
    | TBlockClose -> failwith "todo"

let internal parse (tokens: #seq<Token>) : Result<Program, ParseError> =
    let terminals = tokens |> Seq.map toTerminal

    parse terminals
