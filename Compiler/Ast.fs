module internal rec Compiler.Ast

open System
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type Identifier =
    private
        { name: string
          identity: Guid }

    member this.Name = this.name

    override this.ToString() = this.name

    static member Create(name: string) =
        if name.Length = 0 then
            failwith "Identifier can not be empty"
        elif not (Char.IsLetter name[0]) then
            failwith "Identifier must start with a letter"
        elif name |> Seq.exists (Char.IsLetterOrDigit >> not) then
            failwith "Identifier must only contain letters and digits"
        else
            { name = name
              identity = Guid.NewGuid() }

[<DebuggerDisplay("{ToString()}")>]
type TypeReference =
    private
    | TypeReference of Guid

    override this.ToString() =
        let (TypeReference guid) = this
        let last3 = guid.ToString("N").Substring(29)
        $"TypeRef({last3})"

    static member Create() = TypeReference(Guid.NewGuid())

type ExpressionShape =
    | IdentifierReference of identifier: Identifier
    | NumberLiteral of integerPart: int * fractionalPart: int option
    | StringLiteral of text: string
    | Application of fn: Expression * argument: Expression
    | Binding of identifier: Identifier * parameters: Identifier list * body: Expression
    | Sequence of expressions: Expression list
    | InvalidToken of text: string

type Expression =
    { expressionShape: ExpressionShape
      expressionType: TypeReference
      positionInSource: PositionInSource }

type Program = Program of Expression
