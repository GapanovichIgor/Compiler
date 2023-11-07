module Compiler.Tokenization

open System
open System.IO
open HnkParserCombinator
open HnkParserCombinator.Composition
open HnkParserCombinator.Primitives

type Token =
    | TNumberLiteral of int * int option
    | TDoubleQuotedString of string
    | TPlus
    | TMinus
    | TAsterisk
    | TSlash
    | TParenOpen
    | TParenClose
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

let private pDoubleQuotedString: TokenParser =
    skipOne '"' >>. zeroOrMoreCond (fun c -> c <> '"') .>> skipOne '"'
    >> ParseResult.mapValue (String >> TDoubleQuotedString)

let private pPlus: TokenParser =
    skipOne '+' >> ParseResult.constValue TPlus

let private pMinus: TokenParser =
    skipOne '-' >> ParseResult.constValue TMinus

let private pAsterisk: TokenParser =
    skipOne '*' >> ParseResult.constValue TAsterisk

let private pSlash: TokenParser =
    skipOne '/' >> ParseResult.constValue TSlash

let private pParenOpen: TokenParser =
    skipOne '(' >> ParseResult.constValue TParenOpen

let private pParenClose: TokenParser =
    skipOne ')' >> ParseResult.constValue TParenClose

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
            pMinus
            pAsterisk
            pSlash
            pParenOpen
            pParenClose
            pNumber
            pDoubleQuotedString
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
