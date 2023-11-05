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

    let rec getDivideExpression (e: DivideExpression) =
        match e with
        | DivideExpression.DivideExpression (e1, (), e2) -> "(" + getDivideExpression e1 + " / " + getNumberExpression e2 + ")"
        | DivideExpression.NumberLiteralExpression e -> getNumberExpression e

    let rec getMultiplyExpression (e: MultiplyExpression) =
        match e with
        | MultiplyExpression.MultiplyExpression (e1, (), e2) -> "(" + getMultiplyExpression e1 + " * " + getDivideExpression e2 + ")"
        | MultiplyExpression.DivideExpression e -> getDivideExpression e

    let rec getSubtractExpression (e: SubtractExpression) =
        match e with
        | SubtractExpression.SubtractExpression (e1, (), e2) -> "(" + getSubtractExpression e1 + " - " + getMultiplyExpression e2 + ")"
        | SubtractExpression.MultiplyExpression e -> getMultiplyExpression e

    let rec getAddExpression (e: AddExpression) =
        match e with
        | AddExpression.AddExpression (e1, (), e2) -> "(" + getAddExpression e1 + " + " + getSubtractExpression e2 + ")"
        | AddExpression.SubtractExpression e -> getSubtractExpression e

    let getExpression (Expression.Expression p) = getAddExpression p

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

    Directory.Delete(dir, true)
