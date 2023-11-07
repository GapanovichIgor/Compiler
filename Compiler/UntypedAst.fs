module internal rec Compiler.UntypedAst

type BinaryOperator =
    | Add
    | Subtract
    | Multiply
    | Divide

type Expression =
    | Identifier of string
    | NumberLiteral of int * int option
    | StringLiteral of string
    | BinaryOperation of Expression * BinaryOperator * Expression
    | Application of Expression * Expression

type Statement =
    | Expression of Expression

type StatementSequence = Statement list

type Program = Program of StatementSequence

let private getAtom (e: Parser.AtomExpr) =
    match e with
    | Parser.AtomExpr.Identifier i -> Identifier i
    | Parser.AtomExpr.DoubleQuotedString s -> StringLiteral s
    | Parser.AtomExpr.Number (i, f) -> NumberLiteral (i, f)
    | Parser.AtomExpr.Paren((), e, ()) -> getExpression e

let private getApplication (e: Parser.Application) =
    match e with
    | Parser.Fallthrough e -> getAtom e
    | Parser.Application (e1, e2) -> Application (getApplication e1, getAtom e2)

let private getArithmeticFirstOrderExpr (e: Parser.ArithmeticFirstOrderExpr) =
    match e with
    | Parser.ArithmeticFirstOrderExpr.Fallthrough e ->
        getApplication e
    | Parser.ArithmeticFirstOrderExpr.Multiply (e1, (), e2) ->
        let e1 = getArithmeticFirstOrderExpr e1
        let e2 = getApplication e2
        BinaryOperation (e1, Multiply, e2)
    | Parser.ArithmeticFirstOrderExpr.Divide (e1, (), e2) ->
        let e1 = getArithmeticFirstOrderExpr e1
        let e2 = getApplication e2
        BinaryOperation (e1, Divide, e2)

let private getArithmeticSecondOrderExpr (e: Parser.ArithmeticSecondOrderExpr) =
    match e with
    | Parser.ArithmeticSecondOrderExpr.Fallthrough e ->
        getArithmeticFirstOrderExpr e
    | Parser.ArithmeticSecondOrderExpr.Add (e1, (), e2) ->
        let e1 = getArithmeticSecondOrderExpr e1
        let e2 = getArithmeticFirstOrderExpr e2
        BinaryOperation (e1, Add, e2)
    | Parser.ArithmeticSecondOrderExpr.Subtract (e1, (), e2) ->
        let e1 = getArithmeticSecondOrderExpr e1
        let e2 = getArithmeticFirstOrderExpr e2
        BinaryOperation (e1, Subtract, e2)

let private getExpression (Parser.Expr e) =
    getArithmeticSecondOrderExpr e

let fromParseTree (parseTree: Parser.Program): Program =
    let (Parser.Program e) = parseTree
    Program [
        Statement.Expression (getExpression e)
    ]