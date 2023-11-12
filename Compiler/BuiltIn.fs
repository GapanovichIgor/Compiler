module internal Compiler.BuiltIn

open Compiler.Type

module Types =
    let unit = ValueType "System.Unit"
    let int = ValueType "System.Int"
    let float = ValueType "System.Float"
    let string = ValueType "System.String"

module Identifiers =
    let opAdd = Ast.createIdentifier "+"
    let opSubtract = Ast.createIdentifier "-"
    let opMultiply = Ast.createIdentifier "*"
    let opDivide = Ast.createIdentifier "/"
    let println = Ast.createIdentifier "println"
    let intToStr = Ast.createIdentifier "intToStr"
    let intToStr2 = Ast.createIdentifier "intToStr2"
    let floatToStr = Ast.createIdentifier "floatToStr"

module IdentifierTypes =
    open Types

    let opAdd = FunctionType(int, FunctionType(int, int))
    let opSubtract = FunctionType(int, FunctionType(int, int))
    let opMultiply = FunctionType(int, FunctionType(int, int))
    let opDivide = FunctionType(int, FunctionType(int, int))
    let println = FunctionType(string, unit)
    let intToStr = FunctionType(int, string)
    let intToStr2 = FunctionType(string, FunctionType(int, string))
    let floatToStr = FunctionType(float, string)
