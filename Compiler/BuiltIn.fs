module internal Compiler.BuiltIn

module AtomTypeIds =
    let unit = AtomTypeId("Unit")
    let int = AtomTypeId("Int")
    let float = AtomTypeId("Float")
    let string = AtomTypeId("String")

module Types =
    open AtomTypeIds

    let unit = AtomType unit
    let int = AtomType int
    let float = AtomType float
    let string = AtomType string

module Identifiers =
    let opAdd = Identifier.Create "opAdd"
    let opSubtract = Identifier.Create "opSubtract"
    let opMultiply = Identifier.Create "opMultiply"
    let opDivide = Identifier.Create "opDivide"
    let println = Identifier.Create "println"
    let intToStr = Identifier.Create "intToStr"
    let intToStrFmt = Identifier.Create "intToStrFmt"
    let floatToStr = Identifier.Create "floatToStr"

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
