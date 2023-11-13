module internal Compiler.Type

open System
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type TypeIdentifier =
    private
    | TypeIdentifier of string

    static member Create(identifier: string) =
        if identifier.Length = 0 then
            failwith "Type identifier can not be empty"
        elif not (Char.IsLetter identifier[0]) then
            failwith "Type identifier must start with a letter"
        elif identifier |> Seq.exists (Char.IsLetterOrDigit >> not) then
            failwith "Type identifier must only contain letters and digits"

        TypeIdentifier identifier

    override this.ToString() =
        let (TypeIdentifier identifierString) = this
        identifierString

[<DebuggerDisplay("{ToString()}")>]
type Type =
    | FixedType of TypeIdentifier
    | FunctionType of parameter: Type * result: Type

    override this.ToString() =
        match this with
        | FixedType typeIdentifier -> typeIdentifier.ToString()
        | FunctionType(parameter, result) ->
            let parameter =
                match parameter with
                | FunctionType _ -> $"({parameter})"
                | _ -> parameter.ToString()

            $"{parameter} -> {result}"
