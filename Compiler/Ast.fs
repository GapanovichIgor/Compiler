module internal rec Compiler.Ast

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
