module Compiler.Parser

open Compiler.ParserGenerated
open Compiler.Tokenization

let private toInputItem (token: Token) : InputItem =
    match token with
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
    | TBlockClose -> failwith "todo"

let internal parse (tokens: #seq<Token>) : Result<Program, ParseError> =
    let inputItems = tokens |> Seq.map toInputItem

    parse inputItems
