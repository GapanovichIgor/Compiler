﻿module Compiler.Build

open System
open System.Diagnostics
open System.IO
open Compiler.AstGenerated

module private rec Transpile =
    let getNumber (i, f) =
        match f with
        | Some f -> i.ToString() + "." + f.ToString()
        | None -> i.ToString() + ".0"

    let getAtom (e: AtomExpr) =
        match e with
        | AtomExpr.Number e -> getNumber e
        | AtomExpr.Paren ((), e, ()) -> "(" + getExpression e + ")"

    let rec getArithmeticFirstOrderExpr (e: ArithmeticFirstOrderExpr) =
        match e with
        | ArithmeticFirstOrderExpr.Multiply (e1, (), e2) -> "(" + getArithmeticFirstOrderExpr e1 + " * " + getAtom e2 + ")"
        | ArithmeticFirstOrderExpr.Divide (e1, (), e2) -> "(" + getArithmeticFirstOrderExpr e1 + " / " + getAtom e2 + ")"
        | ArithmeticFirstOrderExpr.Fallthrough e -> getAtom e

    let rec getArithmeticSecondOrderExpr (e: ArithmeticSecondOrderExpr) =
        match e with
        | ArithmeticSecondOrderExpr.Add (e1, (), e2) -> "(" + getArithmeticSecondOrderExpr e1 + " + " + getArithmeticFirstOrderExpr e2 + ")"
        | ArithmeticSecondOrderExpr.Subtract (e1, (), e2) -> "(" + getArithmeticSecondOrderExpr e1 + " - " + getArithmeticFirstOrderExpr e2 + ")"
        | ArithmeticSecondOrderExpr.Fallthrough e -> getArithmeticFirstOrderExpr e

    let getExpression (Expr e) = getArithmeticSecondOrderExpr e

    let getProgram (Program e) = getExpression e

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

    let csExpr = Transpile.getProgram ast
    // let csExpr = ""

    let programFile = File.CreateText($"{csDir}\\Program.cs")
    programFile.Write($"System.Console.WriteLine(%s{csExpr});")
    programFile.Dispose()

    let dotnetProcessStartInfo = ProcessStartInfo("dotnet.exe", "publish -c Release -o ..\\output")
    dotnetProcessStartInfo.WorkingDirectory <- csDir
    let dotnetProcess = Process.Start(dotnetProcessStartInfo)
    dotnetProcess.WaitForExit()

    Directory.Delete(outputPath, true)

    Directory.Move($"{dir}\\output", outputPath)

    // Directory.Delete(dir, true)
