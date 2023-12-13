namespace Common

open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type Type =
    | AtomType of AtomTypeId
    | FunctionType of parameter: Type * result: Type
    | QualifiedType of typeParameters: AtomTypeId list * body: Type

    override this.ToString() =
        match this with
        | AtomType atomTypeId -> atomTypeId.ToString()
        | FunctionType(parameter, result) ->
            let parameter =
                match parameter with
                | FunctionType _ -> $"({parameter})"
                | _ -> parameter.ToString()

            $"{parameter} -> {result}"
        | QualifiedType(typeParameters, body) ->
            let typeParameters =
                typeParameters
                |> Seq.map string
                |> String.concat ", "
            $"forall {typeParameters}. {body}"
