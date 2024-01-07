module internal Compiler.OutputMessaging

open System
open System.IO
open Compiler.Diagnostics
open Compiler.ParserInternal

type private ProblemReport =
    { lineNumber: int
      columnNumber: int
      sourceCodeLine: string
      underline: string }

let private getTokenPositionAndText (token: InputItem) =
    match token with
    | InputItem.Asterisk pos-> pos, "'*'"
    | InputItem.BlockClose pos -> pos, "(block close)"
    | InputItem.BlockOpen pos -> pos, "(block open)"
    | InputItem.Break pos -> pos, "';'"
    | InputItem.DoubleQuotedString (text, pos) ->
        let text =
            if text.Length <= 4
            then text
            else text.Substring(0, 4)
        pos, $"\"{text}\""
    | InputItem.Equals pos -> pos, "'='"
    | InputItem.Identifier (identifier, pos) -> pos, $"'{identifier}' (identifier)"
    | InputItem.InvalidToken (invalidToken, pos) -> pos, $"'{invalidToken}' (invalid token)"
    | InputItem.Let pos -> pos, "'let'"
    | InputItem.Minus pos -> pos, "'-'"
    | InputItem.NumberLiteral (i, f, pos) ->
        let text =
            match f with
            | Some f -> $"'{i}.{f}' (number literal)"
            | None -> $"'{i}' (number literal)"
        pos, text
    | InputItem.ParenClose pos -> pos, "')'"
    | InputItem.ParenOpen pos -> pos, "'('"
    | InputItem.Plus pos -> pos, "'+'"
    | InputItem.Slash pos -> pos, "'/'"

let private getExpectedItemText (expectedItem: ExpectedItem) =
    match expectedItem with
    | EndOfStream -> "(end of file)"
    | Asterisk -> "'*'"
    | BlockClose -> "(block close)"
    | BlockOpen -> "(block open)"
    | Break -> "';'"
    | DoubleQuotedString -> "\"...\" (double quoted string)"
    | Equals -> "'='"
    | Identifier -> "(identifier)"
    | InvalidToken -> "(invalid token)"
    | Let -> "'let'"
    | Minus -> "'-'"
    | NumberLiteral -> "(number literal)"
    | ParenClose -> "')'"
    | ParenOpen -> "'('"
    | Plus -> "'+'"
    | Slash -> "'/'"

let private getProblemReportForRegion (sourceCode: string) (regionStart: int, regionLength: int) =
    let lineStartIndex, lineNumber =
        let rec loop i lineStartIndex lineNumber =
            if i = sourceCode.Length then
                if lineStartIndex = sourceCode.Length then
                    lineStartIndex - 1, lineNumber
                else
                    lineStartIndex, lineNumber
            elif i = regionStart then
                lineStartIndex, lineNumber
            elif sourceCode[i] = '\n' then
                loop (i + 1) (i + 1) (lineNumber + 1)
            else
                loop (i + 1) lineStartIndex lineNumber
        loop 0 0 1

    let columnNumber = regionStart - lineStartIndex + 1

    let lineEndIndex =
        let rec loop i =
            if i = sourceCode.Length then
                i
            elif sourceCode[i] = '\n' then
                if i > 0 && sourceCode[i - 1] = '\r' then
                    i - 1
                else
                    i
            else
                loop (i + 1)
        loop lineStartIndex

    let lineText =
        let lineLength = lineEndIndex - lineStartIndex
        sourceCode.Substring(lineStartIndex, lineLength)

    let underline =
        let underline = System.String('^', regionLength)
        let offset = System.String(' ', columnNumber - 1)
        offset + underline

    { lineNumber = lineNumber
      columnNumber = columnNumber
      sourceCodeLine = lineText
      underline = underline }

let private printColor (color: ConsoleColor) (output: TextWriter) (text: string) =
    let originalColor = Console.ForegroundColor
    Console.ForegroundColor <- color
    output.Write(text)
    Console.ForegroundColor <- originalColor

let private printlnColor (color: ConsoleColor) (output: TextWriter) (text: string) =
    printColor color output text
    output.WriteLine()

let printlnError = printlnColor ConsoleColor.Red Console.Error
let printError = printColor ConsoleColor.Red Console.Error

let printlnWarning = printlnColor ConsoleColor.Yellow Console.Out
let printWarning = printColor ConsoleColor.Yellow Console.Out

let printlnSuccess = printlnColor ConsoleColor.Green Console.Out
let printSuccess = printColor ConsoleColor.Green Console.Out

let printDiagnostics (sourceCode: string) (diagnostics: Diagnostics) =
    if diagnostics.hasProblems then
        for problem in diagnostics.problems do
            let problemReport = getProblemReportForRegion sourceCode (problem.positionInSource.startIndex, problem.positionInSource.length)

            match problem.level with
            | LevelWarning ->
                printlnWarning $"({problemReport.lineNumber}, {problemReport.columnNumber}): Warning: %s{problem.description}"
                printlnWarning problemReport.sourceCodeLine
                printlnWarning problemReport.underline
            | LevelError ->
                printlnError $"({problemReport.lineNumber}, {problemReport.columnNumber}): Error: %s{problem.description}"
                printlnError problemReport.sourceCodeLine
                printlnError problemReport.underline
            printfn ""
    else
        printlnSuccess "Success"

let printParseError (sourceCode: string) (error: ParseError) =
    let problemReport =
        match error.unexpected with
        | Unexpected.EndOfStream ->
            let problemReport = getProblemReportForRegion sourceCode (sourceCode.Length, 1)
            printError $"({problemReport.lineNumber}, {problemReport.columnNumber}): Error: Parser reached the end of the file"
            problemReport
        | Unexpected.InputItem token ->
            let tokenPosition, tokenText = getTokenPositionAndText token
            let problemReport = getProblemReportForRegion sourceCode (tokenPosition.startIndex, tokenPosition.length)
            printError $"({problemReport.lineNumber}, {problemReport.columnNumber}): Error: Parser encountered token %s{tokenText}"
            problemReport

    let expected =
        error.expected
        |> Seq.map getExpectedItemText
        |> String.concat " or "

    printError ", but it expected "
    printlnError expected

    printlnError problemReport.sourceCodeLine
    printlnError problemReport.underline
