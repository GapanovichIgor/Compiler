module internal Compiler.BuiltIn

open Compiler.Type

module Types =
    let unit = ValueType "System.Unit"
    let int = ValueType "System.Int"
    let float = ValueType "System.Float"
    let string = ValueType "System.String"

module Identifiers =
    let println = Ast.createIdentifier "println"
    let intToStr = Ast.createIdentifier "intToStr"
    let intToStr2 = Ast.createIdentifier "intToStr2"
    let floatToStr = Ast.createIdentifier "floatToStr"
