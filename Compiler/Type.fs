module internal Compiler.Type

type Type =
    | ValueType of string
    | FunctionType of Type * Type
