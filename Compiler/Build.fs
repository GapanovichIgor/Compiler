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

    let getNumber (i, f) =
        match f with
        | Some f -> i.ToString() + "." + f.ToString()
        | None -> i.ToString()

    let getNumberExpression (NumberLiteralExpression.NumberLiteralExpression n) = getNumber n

    let rec getArithmeticOpOrderA (e: ArithmeticOpOrderA) =
        match e with
        | ArithmeticOpOrderA.Multiply (e1, (), e2) -> "(" + getArithmeticOpOrderA e1 + " * " + getNumberExpression e2 + ")"
        | ArithmeticOpOrderA.Divide (e1, (), e2) -> "(" + getArithmeticOpOrderA e1 + " / " + getNumberExpression e2 + ")"
        | ArithmeticOpOrderA.Fallthrough e -> getNumberExpression e

    let rec getArithmeticOpOrderB (e: ArithmeticOpOrderB) =
        match e with
        | ArithmeticOpOrderB.Add (e1, (), e2) -> "(" + getArithmeticOpOrderB e1 + " + " + getArithmeticOpOrderA e2 + ")"
        | ArithmeticOpOrderB.Subtract (e1, (), e2) -> "(" + getArithmeticOpOrderB e1 + " - " + getArithmeticOpOrderA e2 + ")"
        | ArithmeticOpOrderB.Fallthrough e -> getArithmeticOpOrderA e

    let getExpression (Expression.Expression p) = getArithmeticOpOrderB p

    let getProgram (Program e) = getExpression e

    let csExpr = getProgram ast

    let programFile = File.CreateText($"{csDir}\\Program.cs")
    programFile.Write($"""System.Console.WriteLine(%s{csExpr});""")
    programFile.Dispose()

    let dotnetProcessStartInfo = ProcessStartInfo("dotnet.exe", "publish -c Release -o ..\\output")
    dotnetProcessStartInfo.WorkingDirectory <- csDir
    let dotnetProcess = Process.Start(dotnetProcessStartInfo)
    dotnetProcess.WaitForExit()

    Directory.Delete(outputPath, true)

    Directory.Move($"{dir}\\output", outputPath)

    // Directory.Delete(dir, true)
