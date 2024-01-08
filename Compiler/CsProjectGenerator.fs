module Compiler.CsProjectGenerator

open System.IO
open Compiler.CsAst

let generate (ast: Program, outputDirectory: string) =
    if not (Directory.Exists(outputDirectory)) then
        Directory.CreateDirectory(outputDirectory) |> ignore

    let csprojFile = File.CreateText(Path.Combine(outputDirectory, "Program.csproj"))
    csprojFile.Write("""<Project Sdk="Microsoft.NET.Sdk">""")
    csprojFile.Write("""<PropertyGroup>""")
    csprojFile.Write("""<LangVersion>11</LangVersion>""")
    csprojFile.Write("""<OutputType>Exe</OutputType>""")
    csprojFile.Write("""<TargetFramework>net7.0</TargetFramework>""")
    csprojFile.Write("""<RuntimeIdentifier>win-x64</RuntimeIdentifier>""")
    csprojFile.Write("""<SelfContained>true</SelfContained>""")
    csprojFile.Write("""<PublishSingleFile>true</PublishSingleFile>""")
    csprojFile.Write("""<PublishTrimmed>true</PublishTrimmed>""")
    csprojFile.Write("""</PropertyGroup>""")
    csprojFile.Write("""</Project>""")
    csprojFile.Dispose()

    let programFile = File.CreateText(Path.Combine(outputDirectory, "Program.cs"))
    programFile.WriteLine("#pragma warning disable CS8321")
    programFile.WriteLine("void println(string text) => System.Console.WriteLine(text);")
    programFile.WriteLine("string intToStr(int x) => x.ToString();")
    programFile.WriteLine("System.Func<int, string> intToStrFmt(string format) => (int x) => x.ToString(format);")
    programFile.WriteLine("string floatToStr(float x) => x.ToString();")
    programFile.WriteLine("T failwith<T>(string message) => throw new Exception(message);")
    programFile.WriteLine("#pragma warning restore CS8321")
    programFile.WriteLine()
    programFile.Flush()
    CsCodeGenerator.generate ast programFile.BaseStream
    programFile.Dispose()
