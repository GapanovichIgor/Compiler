module internal rec Compiler.Ast

open System
open Compiler.Type

type Identifier =
    { name: string
      identity: Guid }

let createIdentifier name =
    { name = name
      identity = Guid.NewGuid() }

type BinaryOperator =
    | Add
    | Subtract
    | Multiply
    | Divide

type Expression<'t> =
    | Identifier of Identifier
    | NumberLiteral of int * int option
    | StringLiteral of string
    | BinaryOperation of TypedExpression<'t> * BinaryOperator * TypedExpression<'t>
    | Application of TypedExpression<'t> * TypedExpression<'t>
    | Coerce of TypedExpression<'t> * Type
    | Binding of Identifier * TypedExpression<'t>
    | Sequence of TypedExpression<'t> list

type TypedExpression<'t> =
    { expression: Expression<'t>
      expressionType: 't }

type Program<'t> = Program of TypedExpression<'t>
