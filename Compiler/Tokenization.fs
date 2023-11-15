module internal Compiler.Tokenization

open System
open System.Collections.Generic
open System.IO
open ParserCombinator
open ParserCombinator.Composition
open ParserCombinator.Primitives
open ParserCombinator.CharPrimitives

type Token = Parser.InputItem

type private TokenInternal =
    | Token of Token
    | Indentation of int

type private Parser<'a> = CharParser<unit, 'a>

let private isWhiteSpace char = char = ' '

let private getSourcePosition (success: ParseSuccess<_, _>) =
    { startIndex = success.position
      length = success.length }

let private pNumber: Parser<Token> =
    let pDigits = oneOrMoreCond "digit" Char.IsDigit

    pDigits .>>. optional (skipOne '.' >>. pDigits)
    >> ParseResult.mapSuccess (fun success ->
        let integerPart, fractionalPart = success.value
        let integerPart = Int32.Parse integerPart
        let fractionalPart = fractionalPart |> Option.map Int32.Parse

        Token.NumberLiteral(integerPart, fractionalPart, getSourcePosition success))

let private pDoubleQuotedString: Parser<Token> =
    skipOne '"' >>. zeroOrMoreCond (fun c -> c <> '"') .>> skipOne '"'
    >> ParseResult.mapSuccess (fun success -> Token.DoubleQuotedString(String(success.value), getSourcePosition success))

let private pIdentifier: Parser<Token> =
    oneCond "letter" Char.IsLetter .>>. zeroOrMoreCond Char.IsLetterOrDigit
    >> ParseResult.mapSuccess (fun success ->
        let firstChar, rest = success.value

        Token.Identifier(string firstChar + String(rest), getSourcePosition success))

let private pPlus: Parser<Token> = skipOne '+' >> ParseResult.constValue Token.Plus

let private pMinus: Parser<Token> =
    skipOne '-' >> ParseResult.constValue Token.Minus

let private pAsterisk: Parser<Token> =
    skipOne '*' >> ParseResult.constValue Token.Asterisk

let private pSlash: Parser<Token> =
    skipOne '/' >> ParseResult.constValue Token.Slash

let private pParenOpen: Parser<Token> =
    skipOne '(' >> ParseResult.constValue Token.ParenOpen

let private pParenClose: Parser<Token> =
    skipOne ')' >> ParseResult.constValue Token.ParenClose

let private pLet: Parser<Token> =
    constString "let" >> ParseResult.constValue Token.Let

let private pBreak: Parser<Token> =
    skipOne ';' >> ParseResult.constValue Token.Break

let private pEquals: Parser<Token> =
    skipOne '=' >> ParseResult.constValue Token.Equals

let private stopInvalidTokenOnCharacters =
    set [ '\r'; '\n'; '+'; '-'; '*'; '/'; '('; ')'; ';'; '=' ]

let private pInvalidToken: Parser<Token> =
    let condition c = not (isWhiteSpace c || stopInvalidTokenOnCharacters.Contains(c))

    oneOrMoreCond "any char except whitespace" condition
    >> ParseResult.mapSuccess (fun success -> Token.InvalidToken(String(success.value), getSourcePosition success))

let private pToken: Parser<TokenInternal> =
    chooseFirstLongest
        [ pPlus
          pMinus
          pAsterisk
          pSlash
          pParenOpen
          pParenClose
          pEquals
          pBreak
          pLet
          pNumber
          pIdentifier
          pDoubleQuotedString ]
    |> orElse pInvalidToken
    >> ParseResult.mapValue Token

let private pSkipWhitespace: Parser<unit> = skipZeroOrMoreCond isWhiteSpace

let private pSkipNewLine: Parser<unit> =
    chooseFirstLongest [ skipOne '\n'; skipOne '\r'; skipOne '\r' >>. skipOne '\n' ]

let private pIndentation: Parser<TokenInternal> =
    zeroOrMoreCond isWhiteSpace
    >> ParseResult.mapValue (fun chars ->
        let indentationValue =
            chars
            |> Seq.map (function
                | ' ' -> 1
                | '\t' -> 4
                | _ -> failwith "Invalid whitespace char")
            |> Seq.sum

        Indentation indentationValue)

let private indentationSubContextEnclosingTokens: list<Token * Token> =
    [ Token.ParenOpen, Token.ParenClose ]

let private indentationSubContextClosingTokens: Set<Token> =
    indentationSubContextEnclosingTokens |> Seq.map snd |> Set

type private IndentationContext =
    | Root of indentationLevels: int list
    | SubContext of closingToken: Token * indentationLevels: int list * parent: IndentationContext

let private mapToBlocks (tokens: TokenInternal list) : Token list =
    let mutable indentationContextStack = Root []

    let openingToClosingTokenMap: Map<Token, Token> =
        indentationSubContextEnclosingTokens |> Map.ofList

    let tokens' = List()

    let closeBlocksOfIndentationContext (indentationLevels: int list) =
        for _ = 0 to indentationLevels.Length - 2 do
            tokens'.Add(Token.BlockClose)

    for i = 0 to tokens.Length - 1 do
        match tokens[i] with
        | Token token ->
            match openingToClosingTokenMap |> Map.tryFind token with
            | Some closingToken -> indentationContextStack <- SubContext(closingToken, [], indentationContextStack)
            | None ->
                if indentationSubContextClosingTokens.Contains(token) then
                    let mutable keepGoing = true

                    while keepGoing do
                        match indentationContextStack with
                        | SubContext(t, indentationLevels, parentIndentationContext) ->
                            closeBlocksOfIndentationContext indentationLevels
                            indentationContextStack <- parentIndentationContext

                            if t = token then
                                keepGoing <- false
                        | Root _ -> keepGoing <- false

            tokens'.Add(token)
        | Indentation indentation ->
            let mutable indentationLevels =
                match indentationContextStack with
                | SubContext(_, indentationLevels, _)
                | Root indentationLevels -> indentationLevels

            let mutable keepGoing = true

            while keepGoing do
                keepGoing <- false

                match indentationLevels with
                | [] ->
                    // First line in this indentation context
                    indentationLevels <- [ indentation ]
                | [ firstLevel ] when indentation < firstLevel ->
                    // Indentation is decreased below the first level - consider that the level is maintained.
                    // This is normal for sub-contexts, although it is bad formatting if the next token is part of the context body
                    // and not a context closing token.
                    tokens'.Add(Token.Break)
                | currentIndentation :: _ when indentation = currentIndentation ->
                    // Indentation level is maintained
                    tokens'.Add(Token.Break)
                | currentIndentation :: _ when indentation > currentIndentation ->
                    // Indentation increased
                    tokens'.Add(Token.BlockOpen)
                    indentationLevels <- indentation :: indentationLevels
                | _ :: nextIndentation :: _ when indentation > nextIndentation ->
                    // Indentation is decreased, but it's still greater than the previous level - consider that the level is maintained
                    tokens'.Add(Token.Break)
                | _ :: nextIndentation :: restIndentations when indentation = nextIndentation ->
                    // Indentation is decreased to the previous level
                    tokens'.Add(Token.BlockClose)
                    tokens'.Add(Token.Break)
                    indentationLevels <- nextIndentation :: restIndentations
                | _ :: restIndentations ->
                    // Indentation is decreased, possibly several levels
                    tokens'.Add(Token.BlockClose)
                    indentationLevels <- restIndentations
                    keepGoing <- true

            indentationContextStack <-
                match indentationContextStack with
                | SubContext(t, _, parent) -> SubContext(t, indentationLevels, parent)
                | Root _ -> Root indentationLevels

    let rec loop indentationContextStack =
        match indentationContextStack with
        | SubContext(_, indentationLevels, parent) ->
            closeBlocksOfIndentationContext indentationLevels
            loop parent
        | Root indentationLevels -> closeBlocksOfIndentationContext indentationLevels

    loop indentationContextStack

    tokens' |> List.ofSeq

let private stripUnnecessaryBreaks (tokens: Token list) : Token list =
    let tokens' = List()

    for i = 0 to tokens.Length - 1 do
        match i, tokens[i] with
        | i, Token.Break when i = 0 || i = tokens.Length - 1 -> ()
        | _, Token.Break ->
            match tokens[i - 1], tokens[i + 1] with
            | Token.Break, _
            | Token.BlockOpen, _
            | _, Token.BlockClose -> ()
            | _, t when indentationSubContextClosingTokens.Contains(t) -> ()
            | _ -> tokens'.Add(Token.Break)
        | _, token -> tokens'.Add(token)

    tokens' |> List.ofSeq

let private postProcessTokenSequence (tokens: TokenInternal list) : Token list =
    tokens |> mapToBlocks |> stripUnnecessaryBreaks

let private createTape (stream: Stream) : Tape<char> =
    let reader = new StreamReader(stream)

    let readChar () =
        match reader.Read() with
        | -1 -> None
        | c -> Some(char c)

    Tape(4096, readChar)

let tokenize (stream: Stream) : Token list =
    let pToken: Parser<TokenInternal> = pToken |> commitOnSuccess
    let pSkipWhitespace: Parser<unit> = pSkipWhitespace |> commitOnSuccess
    let pSkipNewLine: Parser<unit> = pSkipNewLine |> commitOnSuccess
    let pIndentation: Parser<TokenInternal> = pIndentation |> commitOnSuccess

    let pLineWithTokens: Parser<TokenInternal list> =
        let lineTokens = oneOrMore (pToken .>> pSkipWhitespace)

        pIndentation .>>. lineTokens >> ParseResult.mapValue (fun (l, r) -> l :: r)

    let pEmptyLine: Parser<TokenInternal list> =
        pSkipWhitespace >> ParseResult.constValue []

    let pLine: Parser<TokenInternal list> = pLineWithTokens |> orElse pEmptyLine

    let pFile: Parser<TokenInternal list> =
        let concatLines = ParseResult.mapValue List.concat

        zeroOrMoreDelimited pSkipNewLine pLine >> concatLines

    let tape = createTape stream

    let parseResult =
        pFile (tape, ())
        |> Result.defaultWith (fun _ -> failwith "Failed to tokenize the stream")

    parseResult.value |> postProcessTokenSequence
