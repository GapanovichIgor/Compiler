namespace Compiler

open System
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type internal AtomTypeId(hint: string) =
    new() = AtomTypeId("AtomTypeId(" + Guid.NewGuid().ToString() + ")")

    override this.ToString() = hint

[<DebuggerDisplay("{ToString()}")>]
type internal VariableTypeId(hint: string) =
    new() = VariableTypeId("VariableTypeId(" + Guid.NewGuid().ToString() + ")")

    override this.ToString() = hint

[<DebuggerDisplay("{ToString()}")>]
type internal TypeReference private (hint: string, hasHint: bool) =
    member val private test = Guid.NewGuid()

    new() = TypeReference("TypeReference(" + Guid.NewGuid().ToString() + ")", false)
    new(hint: string) = TypeReference(hint, true)

    member _.HasHint = hasHint

    override this.ToString() = hint

[<DebuggerDisplay("{ToString()}")>]
type internal Type =
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

[<DebuggerDisplay("{ToString()}")>]
type internal TypeConstructor =
    | NullaryTypeConstructor of Type
    | TypeConstructor of parameter: VariableTypeId * result: TypeConstructor

    override this.ToString() =
        let rec loop typeCtor arity =
            match typeCtor with
            | NullaryTypeConstructor t -> (t, arity)
            | TypeConstructor(_, subCtor) -> loop subCtor (arity + 1)

        let resultType, arity = loop this 0

        let shapeText = Seq.replicate (arity + 1) "*" |> String.concat " -> "

        $"{shapeText} ({resultType})"
