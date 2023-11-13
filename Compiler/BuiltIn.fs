module internal Compiler.BuiltIn

open Compiler.Type

module Types =
    let private create name = FixedType (TypeIdentifier.Create(name))

    let unit = create "Unit"
    let int = create "Int"
    let float = create "Float"
    let string = create "String"

module Identifiers =
    open type Ast.Identifier

    let opAdd = Create "opAdd"
    let opSubtract = Create "opSubtract"
    let opMultiply = Create "opMultiply"
    let opDivide = Create "opDivide"
    let println = Create "println"
    let intToStr = Create "intToStr"
    let intToStrFmt = Create "intToStrFmt"
    let floatToStr = Create "floatToStr"

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
