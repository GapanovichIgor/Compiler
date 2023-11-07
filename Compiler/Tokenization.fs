module internal Compiler.Tokenization

open System
open System.IO
open HnkParserCombinator
open HnkParserCombinator.Composition
open HnkParserCombinator.Primitives
open HnkParserCombinator.CharPrimitives

type Token = Parser.InputItem

type private TokenParser = CharParser<IndentationBasedLanguageKit.State, PrimitiveError<char>, Token>

let private isWhiteSpace char = char = ' '

let private pNumber: TokenParser =
    let pDigits = oneOrMoreCond Char.IsDigit

    pDigits .>>. optional (skipOne '.' >>. pDigits)
    >> ParseResult.mapValue (fun (integerPart, fractionalPart) ->
        let integerPart = Int32.Parse integerPart
        let fractionalPart = fractionalPart |> Option.map Int32.Parse
        Token.NumberLiteral (integerPart, fractionalPart))

let private pDoubleQuotedString: TokenParser =
    skipOne '"' >>. zeroOrMoreCond (fun c -> c <> '"') .>> skipOne '"'
    >> ParseResult.mapValue (String >> Token.DoubleQuotedString)

let private pIdentifier: TokenParser =
    oneCond Char.IsLetter .>>. zeroOrMoreCond Char.IsLetterOrDigit
    >> ParseResult.mapValue (fun (firstChar, rest) -> Token.Identifier (string firstChar + String(rest)))

let private pPlus: TokenParser =
    skipOne '+' >> ParseResult.constValue (Token.Plus ())

let private pMinus: TokenParser =
    skipOne '-' >> ParseResult.constValue (Token.Minus ())

let private pAsterisk: TokenParser =
    skipOne '*' >> ParseResult.constValue (Token.Asterisk ())

let private pSlash: TokenParser =
    skipOne '/' >> ParseResult.constValue (Token.Slash ())

let private pParenOpen: TokenParser =
    skipOne '(' >> ParseResult.constValue (Token.ParenOpen ())

let private pParenClose: TokenParser =
    skipOne ')' >> ParseResult.constValue (Token.ParenClose ())

let private pLet: TokenParser =
    constString "let" >> ParseResult.constValue (Token.Let ())

let private pSemicolon: TokenParser =
    skipOne ';' >> ParseResult.constValue (Token.Semicolon ())

let private pEquals: TokenParser =
    skipOne '=' >> ParseResult.constValue (Token.Equals ())

let private pInvalidToken: TokenParser =
    oneOrMoreCond (fun c -> not (isWhiteSpace c) && c <> '\n' && c <> '\r')
    >> ParseResult.mapValue (String >> Token.InvalidToken)

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
            pEquals
            pSemicolon
            pLet
            pNumber
            pIdentifier
            pDoubleQuotedString
        ]
        |> orElse pInvalidToken

    let parseDocument =
        IndentationBasedLanguageKit.simpleParseDocument
            { parseToken = parseToken
              blockOpenToken = Token.BlockOpen ()
              newLineDelimiter = Token.NewLine ()
              blockCloseToken = Token.BlockClose ()
              isWhiteSpace = isWhiteSpace }

    match parseDocument tape with
    | Error e -> Error e
    | Ok result -> Ok result.value
