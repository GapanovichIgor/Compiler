module internal rec Compiler.Ast

open System
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type Identifier =
    { name: string
      identity: Guid }

    override this.ToString() = this.name

let createIdentifier name =
    { name = name
      identity = Guid.NewGuid() }

type TypeReference = TypeReference of Guid

let createTypeReference () = TypeReference(Guid.NewGuid())

type ExpressionShape =
    | IdentifierReference of identifier: Identifier
    | NumberLiteral of integerPart: int * fractionalPart: int option
    | StringLiteral of text: string
    | Application of fn: Expression * argument: Expression
    | Binding of identifier: Identifier * parameters: Identifier list * body: Expression
    | Sequence of expressions: Expression list

type Expression =
    { expressionShape: ExpressionShape
      expressionType: TypeReference }

type Program = Program of Expression
