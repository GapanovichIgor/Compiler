open System
open System.Diagnostics
open System.IO
open System.Text
open Compiler
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
            OutputMessaging.printParseError sourceCode e
            fail ()

    let ast = AstBuilder.buildFromParseTree parseTree

    let astDiagnostics = AstDiagnostics.get ast

    if astDiagnostics.hasErrors then
        OutputMessaging.printDiagnostics sourceCode astDiagnostics
        fail ()

    let typeInformation = TypeSolver.Solver.getTypeInformation ast

    let csAst = CsTranspiler.transpile (ast, typeInformation)

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
        OutputMessaging.printlnError ""
        OutputMessaging.printlnError "Failed"
        -1
