﻿module rec Compiler.CsAst

type Identifier = string

type Type = string

type BinaryOperator =
    | Add
    | Subtract
    | Multiply
    | Divide

type Expression =
    | Identifier of Identifier
    | NumberLiteral of int * int option * Type
    | StringLiteral of string
    | BinaryOperation of Expression * BinaryOperator * Expression
    | FunctionCall of Identifier * Expression list
    | Cast of Expression * Type

type Statement =
    | Var of Type * Identifier * Expression
    | FunctionCall of Identifier * Expression list

type StatementSequence = Statement list

type Program = Program of StatementSequence