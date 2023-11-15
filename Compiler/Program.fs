open System
open System.Diagnostics
open System.IO
open System.Text
open Compiler
open Compiler.Diagnostics
open Compiler.Parser
open Compiler.Tokenization

type private CompilerException() =
    inherit Exception()

let fail () = raise (CompilerException())

let private getArgMap (args: string list) =
    let (|ParameterName|_|) (arg: string) =
        if arg.StartsWith("--") then
            Some(arg.Substring(2))
        else
            None

    let rec loop args map =
        match args with
        | [] -> map
        | [ ParameterName flag ]
        | ParameterName flag :: ParameterName _ :: _ ->
            let map = map |> Map.add flag String.Empty
            loop args.Tail map
        | ParameterName parameterName :: value :: argsRest ->
            let map = map |> Map.add parameterName value
            loop argsRest map
        | _ :: argsRest -> loop argsRest map

    loop args Map.empty


let private (|ArgMapWith|_|) requiredParams args =
    let argMap = getArgMap args

    if requiredParams |> List.forall (fun p -> argMap |> Map.containsKey p) then
        Some argMap
    else
        None

let private printlnColor (color: ConsoleColor) (text: string) =
    let originalColor = Console.ForegroundColor
    Console.ForegroundColor <- color
    Console.Error.WriteLine(text)
    Console.ForegroundColor <- originalColor

let private printlnError = printlnColor ConsoleColor.Red

let private printlnWarning = printlnColor ConsoleColor.Yellow

let private printlnSuccess = printlnColor ConsoleColor.Green

let private printDiagnostics (sourceCode: string) (diagnostics: Diagnostics) =
    let getSourceLine (pos: PositionInSource) =
        let lineStartIndex, lineNumber =
            let rec loop i lineStartIndex lineNumber =
                if i = sourceCode.Length then
                    lineStartIndex - 1, lineNumber
                elif i = pos.startIndex then
                    lineStartIndex, lineNumber
                elif sourceCode[i] = '\n' then
                    loop (i + 1) (i + 1) (lineNumber + 1)
                else
                    loop (i + 1) lineStartIndex lineNumber
            loop 0 0 1

        let columnNumber = pos.startIndex - lineStartIndex + 1

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

        let lineLength = lineEndIndex - lineStartIndex

        let lineText = sourceCode.Substring(lineStartIndex, lineLength)

        lineNumber, columnNumber, lineText

    if diagnostics.hasProblems then
        for problem in diagnostics.problems do
            let lineNumber, columnNumber, lineText = getSourceLine problem.positionInSource

            let underline =
                let underline = System.String('^', problem.positionInSource.length)
                let offset = System.String(' ', columnNumber - 1)
                offset + underline

            match problem.level with
            | LevelWarning ->
                printlnWarning $"({lineNumber}, {columnNumber}): Warning: %s{problem.description}"
                printlnWarning lineText
                printlnWarning underline
            | LevelError ->
                printlnError $"({lineNumber}, {columnNumber}): Error: %s{problem.description}"
                printlnError lineText
                printlnError underline
            printfn ""
    else
        printlnSuccess "Success"

let private generateCsProject (sourceFilePath: string, outputDir: string) =
    let sourceCode, sourceCodeStream =
        use fs = File.OpenRead(sourceFilePath)
        let ms = new MemoryStream()
        fs.CopyTo(ms)
        let str = ms.ToArray() |> Encoding.UTF8.GetString
        ms.Position <- 0
        str, ms

    let tokens = tokenize sourceCodeStream

    let parseTree =
        match parse tokens with
        | Ok parseTree -> parseTree
        | Error e ->
            printlnError "Failed to parse"
            fail ()

    let ast = AstBuilder.buildFromParseTree parseTree

    let astDiagnostics = AstDiagnostics.get ast

    printDiagnostics sourceCode astDiagnostics

    if astDiagnostics.hasProblems then
        fail ()

    let typeMap = TypeSolver.getTypeInformation ast

    let csAst = CsTranspiler.transpile (ast, typeMap)

    CsProjectGenerator.generate (csAst, outputDir)

let private build (sourceFilePath: string, outputDir: string) =
    let tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())

    let absoluteOutputDir =
        if Path.IsPathRooted(outputDir) then
            outputDir
        else
            Path.Combine(Environment.CurrentDirectory, outputDir)

    try
        generateCsProject (sourceFilePath, tempDir)

        let dotnetProcess =
            ProcessStartInfo("dotnet.exe", $"publish -c Release -o \"{absoluteOutputDir}\"", WorkingDirectory = tempDir)
            |> Process.Start

        dotnetProcess.WaitForExit()
    finally
        if Directory.Exists(tempDir) then
            Directory.Delete(tempDir, true)

let private run (sourceFilePath: string) =
    let tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())

    try
        generateCsProject (sourceFilePath, tempDir)

        let dotnetProcess =
            ProcessStartInfo("dotnet.exe", "run -c Release", WorkingDirectory = tempDir)
            |> Process.Start

        dotnetProcess.WaitForExit()
    finally
        if Directory.Exists(tempDir) then
            Directory.Delete(tempDir, true)

[<EntryPoint>]
let main args =
    let args = List.ofArray args

    try
        match args with
        | "generate" :: "csharp" :: "project" :: ArgMapWith [ "source"; "output" ] argMap ->
            let sourceFilePath = argMap["source"]
            let outputDir = argMap["output"]
            generateCsProject (sourceFilePath, outputDir)
        | "build" :: ArgMapWith [ "source"; "output" ] argMap ->
            let sourceFilePath = argMap["source"]
            let outputDir = argMap["output"]
            build (sourceFilePath, outputDir)
        | "run" :: ArgMapWith [ "source" ] argMap ->
            let sourceFilePath = argMap["source"]
            run sourceFilePath
        | _ -> printfn "Invalid arguments"

        0
    with
    | :? CompilerException ->
        printlnError "Failed"
        1
    | _ -> reraise ()
