module internal Compiler.Tokenization

open System
open System.Collections.Generic
open System.IO
open ParserCombinator
open ParserCombinator.Composition
open ParserCombinator.Primitives
open ParserCombinator.CharPrimitives

open Common

type Token = ParserInternal.InputItem

type ParserInternal.InputItem with

    member this.GetPosition() =
        match this with
        | Token.Asterisk positionInSource -> positionInSource
        | Token.BlockClose positionInSource -> positionInSource
        | Token.BlockOpen positionInSource -> positionInSource
        | Token.Break positionInSource -> positionInSource
        | Token.DoubleQuotedString(_, positionInSource) -> positionInSource
        | Token.Equals positionInSource -> positionInSource
        | Token.Identifier(_, positionInSource) -> positionInSource
        | Token.InvalidToken(_, positionInSource) -> positionInSource
        | Token.Let positionInSource -> positionInSource
        | Token.Minus positionInSource -> positionInSource
        | Token.NumberLiteral(_, _, positionInSource) -> positionInSource
        | Token.ParenClose positionInSource -> positionInSource
        | Token.ParenOpen positionInSource -> positionInSource
        | Token.Plus positionInSource -> positionInSource
        | Token.Slash positionInSource -> positionInSource

type private TokenInternal =
    | Token of Token
    | Indentation of int * PositionInSource

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

let private constTokenValue tokenCtor =
    ParseResult.mapSuccess (fun success ->
        tokenCtor
            { startIndex = success.position
              length = success.length })

let private pPlus: Parser<Token> = skipOne '+' >> constTokenValue Token.Plus

let private pMinus: Parser<Token> = skipOne '-' >> constTokenValue Token.Minus

let private pAsterisk: Parser<Token> = skipOne '*' >> constTokenValue Token.Asterisk

let private pSlash: Parser<Token> = skipOne '/' >> constTokenValue Token.Slash

let private pParenOpen: Parser<Token> =
    skipOne '(' >> constTokenValue Token.ParenOpen

let private pParenClose: Parser<Token> =
    skipOne ')' >> constTokenValue Token.ParenClose

let private pLet: Parser<Token> = constString "let" >> constTokenValue Token.Let

let private pBreak: Parser<Token> = skipOne ';' >> constTokenValue Token.Break

let private pEquals: Parser<Token> = skipOne '=' >> constTokenValue Token.Equals

let private stopInvalidTokenOnCharacters =
    set [ '\r'; '\n'; '+'; '-'; '*'; '/'; '('; ')'; ';'; '=' ]

let private pInvalidToken: Parser<Token> =
    let condition c =
        not (isWhiteSpace c || stopInvalidTokenOnCharacters.Contains(c))

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
    >> ParseResult.mapSuccess (fun success ->
        let indentationValue =
            success.value
            |> Seq.map (function
                | ' ' -> 1
                | '\t' -> 4
                | _ -> failwith "Invalid whitespace char")
            |> Seq.sum

        let position =
            { startIndex = success.position
              length = success.length }

        Indentation (indentationValue, position))

let private indentationSubContextEnclosingTokens =
    function
    | Token.ParenOpen _ ->
        function
        | Token.ParenClose _ -> true
        | _ -> false
        |> Some
    | _ -> None

let private isIndentationSubContextClosingToken =
    function
    | Token.ParenClose _ -> true
    | _ -> false

type private IndentationContext =
    | Root of indentationLevels: int list
    | SubContext of isClosingToken: (Token -> bool) * indentationLevels: int list * parent: IndentationContext

let private mapToBlocks (tokens: TokenInternal list) : Token list =
    let mutable indentationContextStack = Root []

    let tokens' = List<Token>()

    let closeBlocksOfIndentationContext (positionInSource: PositionInSource) (indentationLevels: int list) =
        for _ = 0 to indentationLevels.Length - 2 do
            tokens'.Add(Token.BlockClose positionInSource)

    for i = 0 to tokens.Length - 1 do
        match tokens[i] with
        | Token token ->
            match indentationSubContextEnclosingTokens token with
            | Some isClosingToken -> indentationContextStack <- SubContext(isClosingToken, [], indentationContextStack)
            | None ->
                if isIndentationSubContextClosingToken token then
                    let mutable keepGoing = true

                    while keepGoing do
                        match indentationContextStack with
                        | SubContext(isClosingToken, indentationLevels, parentIndentationContext) ->
                            let tokenPosition = token.GetPosition()

                            let blockClosePosition =
                                { startIndex = tokenPosition.startIndex
                                  length = 1 }

                            closeBlocksOfIndentationContext blockClosePosition indentationLevels
                            indentationContextStack <- parentIndentationContext

                            if isClosingToken token then
                                keepGoing <- false
                        | Root _ -> keepGoing <- false

            tokens'.Add(token)
        | Indentation (indentation, indentationPosition) ->
            let mutable indentationLevels =
                match indentationContextStack with
                | SubContext(_, indentationLevels, _)
                | Root indentationLevels -> indentationLevels

            let positionAfterIndentation =
                { startIndex = indentationPosition.startIndex + indentationPosition.length
                  length = 0 }

            let rec loop () =
                match indentationLevels with
                | [] ->
                    // First line in this indentation context
                    indentationLevels <- [ indentation ]
                | [ firstLevel ] when indentation < firstLevel ->
                    // Indentation is decreased below the first level - consider that the level is maintained.
                    // This is normal for sub-contexts, although it is bad formatting if the next token is part of the context body
                    // and not a context closing token.
                    tokens'.Add(Token.Break positionAfterIndentation)
                | currentIndentation :: _ when indentation = currentIndentation ->
                    // Indentation level is maintained
                    tokens'.Add(Token.Break positionAfterIndentation)
                | currentIndentation :: _ when indentation > currentIndentation ->
                    // Indentation increased
                    tokens'.Add(Token.BlockOpen positionAfterIndentation)
                    indentationLevels <- indentation :: indentationLevels
                | _ :: nextIndentation :: _ when indentation > nextIndentation ->
                    // Indentation is decreased, but it's still greater than the previous level - consider that the level is maintained
                    tokens'.Add(Token.Break positionAfterIndentation)
                | _ :: nextIndentation :: restIndentations when indentation = nextIndentation ->
                    // Indentation is decreased to the previous level
                    tokens'.Add(Token.BlockClose positionAfterIndentation)
                    tokens'.Add(Token.Break positionAfterIndentation)
                    indentationLevels <- nextIndentation :: restIndentations
                | _ :: restIndentations ->
                    // Indentation is decreased, possibly several levels
                    tokens'.Add(Token.BlockClose positionAfterIndentation)
                    indentationLevels <- restIndentations
                    loop ()

            loop ()

            indentationContextStack <-
                match indentationContextStack with
                | SubContext(t, _, parent) -> SubContext(t, indentationLevels, parent)
                | Root _ -> Root indentationLevels

    match Seq.tryLast tokens' with
    | Some lastToken ->
        let lastTokenPosition = lastToken.GetPosition()
        let positionAfterLastToken =
            { startIndex = lastTokenPosition.startIndex + lastTokenPosition.length
              length = 0 }
        let rec loop indentationContextStack =
            match indentationContextStack with
            | SubContext(_, indentationLevels, parent) ->
                closeBlocksOfIndentationContext positionAfterLastToken indentationLevels
                loop parent
            | Root indentationLevels -> closeBlocksOfIndentationContext positionAfterLastToken indentationLevels

        loop indentationContextStack
    | None ->
        ()

    tokens' |> List.ofSeq

let private stripUnnecessaryBreaks (tokens: Token list) : Token list =
    let tokens' = List()

    for i = 0 to tokens.Length - 1 do
        match i, tokens[i] with
        | i, Token.Break _ when i = 0 || i = tokens.Length - 1 -> ()
        | _, Token.Break pos ->
            match tokens[i - 1], tokens[i + 1] with
            | Token.Break _, _
            | Token.BlockOpen _, _
            | _, Token.BlockClose _ -> ()
            | _, t when isIndentationSubContextClosingToken t -> ()
            | _ -> tokens'.Add(Token.Break pos)
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
