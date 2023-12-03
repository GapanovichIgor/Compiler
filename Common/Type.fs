namespace Common

open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type Type =
    | AtomType of AtomTypeId
    | FunctionType of parameter: Type * result: Type
    | VariableType of VariableTypeId

    override this.ToString() =
        match this with
        | AtomType atomTypeId -> atomTypeId.ToString()
        | FunctionType(parameter, result) ->
            let parameter =
                match parameter with
                | FunctionType _ -> $"({parameter})"
                | _ -> parameter.ToString()

            $"{parameter} -> {result}"
        | VariableType varTypeId -> varTypeId.ToString()
