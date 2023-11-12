module rec Compiler.CsAst

type Identifier = string

type Type =
    | ValueType of string
    | FunctionType of parameters: Type list * result: Type option

type BinaryOperator =
    | Add
    | Subtract
    | Multiply
    | Divide

type Expression =
    | IdentifierReference of Identifier
    | NumberLiteral of int * int option * Type
    | StringLiteral of string
    | BinaryOperation of Expression * BinaryOperator * Expression
    | FunctionCall of Identifier * Expression list
    | Cast of Expression * Type

type Statement =
    | Var of variableType: Type * variableName: Identifier * assignedExpression: Expression
    | LocalFunction of resultType: Type option * functionName: Identifier * parameters: (Type * Identifier) list * body: StatementSequence
    | FunctionCall of functionName: Identifier * arguments: Expression list
    | Return of Expression

type StatementSequence = Statement list

type Program = Program of StatementSequence
