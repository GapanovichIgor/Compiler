module Compiler.Build

open System.Diagnostics
open System.IO
open Compiler.CsAst

let internal build (ast: Program, outputPath: string) =
    let buildDir = "tmp"
    let csDir = $"{buildDir}\\cs"

    if Directory.Exists(buildDir) then
        Directory.Delete(buildDir, true)

    Directory.CreateDirectory(csDir) |> ignore

    let csprojFile = File.CreateText($"{csDir}\\Program.csproj")
    csprojFile.Write("""<Project Sdk="Microsoft.NET.Sdk">""")
    csprojFile.Write("""<PropertyGroup>""")
    csprojFile.Write("""<OutputType>Exe</OutputType>""")
    csprojFile.Write("""<TargetFramework>net7.0</TargetFramework>""")
    csprojFile.Write("""<RuntimeIdentifier>win-x64</RuntimeIdentifier>""")
    csprojFile.Write("""<SelfContained>true</SelfContained>""")
    csprojFile.Write("""<PublishSingleFile>true</PublishSingleFile>""")
    csprojFile.Write("""<PublishTrimmed>true</PublishTrimmed>""")
    csprojFile.Write("""</PropertyGroup>""")
    csprojFile.Write("""</Project>""")
    csprojFile.Dispose()

    let programFile = File.CreateText($"{csDir}\\Program.cs")
    programFile.WriteLine("""void println(string text) => System.Console.WriteLine(text);""")
    programFile.WriteLine("""string intToStr(int x) => x.ToString();""")
    programFile.WriteLine("""System.Func<int, string> intToStrFmt(string format) => (int x) => x.ToString(format);""")
    programFile.WriteLine("""string floatToStr(float x) => x.ToString();""")
    programFile.WriteLine()
    programFile.Flush()
    CsCodeGenerator.generate ast programFile.BaseStream
    programFile.Dispose()

    let dotnetProcessStartInfo = ProcessStartInfo("dotnet.exe", "publish -c Release -o ..\\output")
    dotnetProcessStartInfo.WorkingDirectory <- csDir
    let dotnetProcess = Process.Start(dotnetProcessStartInfo)
    dotnetProcess.WaitForExit()

    if Directory.Exists(outputPath) then
        Directory.Delete(outputPath, true)

    Directory.Move($"{buildDir}\\output", outputPath)

    // Directory.Delete(buildDir, true)
