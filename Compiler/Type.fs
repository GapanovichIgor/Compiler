module Compiler.Type

type Type =
    | ValueType of string
    | FunctionType of Type * Type

module BuiltInTypes =
    let unit = ValueType "System.Unit"
    let int = ValueType "System.Int"
    let float = ValueType "System.Float"
    let string = ValueType "System.String"
