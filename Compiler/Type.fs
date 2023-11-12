module internal Compiler.Type

open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type Type =
    | ValueType of string
    | FunctionType of parameter: Type * result: Type

    override this.ToString() =
        match this with
        | ValueType t -> t
        | FunctionType (arg, result) -> $"({arg}) -> ({result})"
