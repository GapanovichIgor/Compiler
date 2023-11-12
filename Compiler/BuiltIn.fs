module internal Compiler.BuiltIn

open Compiler.Type

module Types =
    let unit = ValueType "Unit"
    let int = ValueType "Int"
    let float = ValueType "Float"
    let string = ValueType "String"

module Identifiers =
    open Ast

    let opAdd = createIdentifier "+"
    let opSubtract = createIdentifier "-"
    let opMultiply = createIdentifier "*"
    let opDivide = createIdentifier "/"
    let println = createIdentifier "println"
    let intToStr = createIdentifier "intToStr"
    let intToStrFmt = createIdentifier "intToStrFmt"
    let floatToStr = createIdentifier "floatToStr"

module IdentifierTypes =
    open Types

    let opAdd = FunctionType(int, FunctionType(int, int))
    let opSubtract = FunctionType(int, FunctionType(int, int))
    let opMultiply = FunctionType(int, FunctionType(int, int))
    let opDivide = FunctionType(int, FunctionType(int, int))
    let println = FunctionType(string, unit)
    let intToStr = FunctionType(int, string)
    let intToStrFmt = FunctionType(string, FunctionType(int, string))
    let floatToStr = FunctionType(float, string)
