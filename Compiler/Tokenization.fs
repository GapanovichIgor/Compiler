module Compiler.Tokenization

open System
open System.IO
open HnkParserCombinator
open HnkParserCombinator.Composition
open HnkParserCombinator.Primitives

type Token =
    | TNumberLiteral of int * int option
    | TPlus
    | TAsterisk
    | TBreak
    | TBlockOpen
    | TBlockClose
    | TInvalid of string

type private TokenParser = CharParser<IndentationBasedLanguageKit.State, PrimitiveError<char>, Token>

let private isWhiteSpace char = char = ' '

let private pNumber: TokenParser =
    let pDigits = oneOrMoreCond Char.IsDigit

    pDigits .>>. optional (skipOne '.' >>. pDigits)
    >> ParseResult.mapValue (fun (integerPart, fractionalPart) ->
        let integerPart = Int32.Parse integerPart
        let fractionalPart = fractionalPart |> Option.map Int32.Parse
        TNumberLiteral (integerPart, fractionalPart))

let private pPlus: TokenParser =
    skipOne '+' >> ParseResult.constValue TPlus

let private pAsterisk: TokenParser =
    skipOne '*' >> ParseResult.constValue TAsterisk

let private pInvalidToken: TokenParser =
    oneOrMoreCond (fun c -> not (isWhiteSpace c) && c <> '\n' && c <> '\r')
    >> ParseResult.mapValue (String >> TInvalid)

let tokenize (stream: Stream) =
    let reader = new StreamReader(stream)

    let readChar () =
        match reader.Read() with
        | -1 -> None
        | c -> Some(char c)

    let tape = Tape(4096, readChar)

    let parseToken =
        chooseFirstLongest [
            pPlus
            pAsterisk
            pNumber
        ]
        |> orElse pInvalidToken

    let parseDocument =
        IndentationBasedLanguageKit.simpleParseDocument
            { parseToken = parseToken
              blockOpenToken = TBlockOpen
              newLineDelimiter = TBreak
              blockCloseToken = TBlockClose
              isWhiteSpace = isWhiteSpace }

    match parseDocument tape with
    | Error e -> Error e
    | Ok result -> Ok result.value
