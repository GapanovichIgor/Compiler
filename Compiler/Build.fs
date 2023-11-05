module Compiler.Build

open System
open System.Diagnostics
open System.IO
open Compiler.AstGenerated

let internal build (ast: Program, outputPath: string) =
    let dir = Guid.NewGuid().ToString()
    let csDir = $"{dir}\\cs"
    Directory.CreateDirectory(csDir) |> ignore

    let csprojFile = File.CreateText($"{csDir}\\Program.csproj")
    csprojFile.Write("""<Project Sdk="Microsoft.NET.Sdk">""")
    csprojFile.Write("""<PropertyGroup>""")
    csprojFile.Write("""<OutputType>Exe</OutputType>""")
    csprojFile.Write("""<TargetFramework>net7.0</TargetFramework>""")
    csprojFile.Write("""</PropertyGroup>""")
    csprojFile.Write("""</Project>""")
    csprojFile.Dispose()

    let number =
        match ast with
        | Program (i, f) ->
            match f with
            | Some f -> $"{i}.{f}"
            | None -> i.ToString()

    let programFile = File.CreateText($"{csDir}\\Program.cs")
    programFile.Write($"""System.Console.WriteLine(%s{number});""")
    programFile.Dispose()

    let dotnetProcessStartInfo = ProcessStartInfo("dotnet.exe", "publish -c Release -o ..\\output")
    dotnetProcessStartInfo.WorkingDirectory <- csDir
    let dotnetProcess = Process.Start(dotnetProcessStartInfo)
    dotnetProcess.WaitForExit()

    Directory.Delete(outputPath, true)

    Directory.Move($"{dir}\\output", outputPath)

    Directory.Delete(dir, true)
