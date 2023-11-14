open System
open System.Diagnostics
open System.IO
open Compiler
open Compiler.Parser
open Compiler.Tokenization

let private getArgMap (args: string list) =
    seq {
        for i = 0 to args.Length - 1 do
            if i % 2 <> 0 then
                yield args[i - 1].Trim('-'), args[i]
    }
    |> Map.ofSeq

let private (|ArgMapWith|_|) requiredParams args =
    let argMap = getArgMap args

    if requiredParams |> List.forall (fun p -> argMap |> Map.containsKey p) then
        Some argMap
    else
        None

let private generateCs (sourceFilePath: string, outputDir: string) =
    let sourceCodeStream = File.OpenRead(sourceFilePath)

    let tokens, _ = tokenize sourceCodeStream

    let parseTree =
        parse tokens |> Result.defaultWith (fun _ -> failwith "Parsing failed")

    let ast = AstBuilder.buildFromParseTree parseTree

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
        generateCs (sourceFilePath, tempDir)

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
        generateCs (sourceFilePath, tempDir)

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

    match args with
    | "generate" :: "csharp" :: "project" :: ArgMapWith [ "source"; "output" ] argMap ->
        let sourceFilePath = argMap["source"]
        let outputDir = argMap["output"]
        generateCs (sourceFilePath, outputDir)
        0
    | "build" :: ArgMapWith [ "source"; "output" ] argMap ->
        let sourceFilePath = argMap["source"]
        let outputDir = argMap["output"]
        build (sourceFilePath, outputDir)
        0
    | "run" :: ArgMapWith ["source"] argMap ->
        let sourceFilePath = argMap["source"]
        run sourceFilePath
        0
    | _ ->
        printfn "Invalid arguments"
        1
