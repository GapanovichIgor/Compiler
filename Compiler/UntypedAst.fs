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
    | Let of string * Expression * Expression

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

let private getArithmeticFirstOrderExpr (e: Parser.ArithmeticFirstOrderExpression) =
    match e with
    | Parser.ArithmeticFirstOrderExpression.Fallthrough e ->
        getApplication e
    | Parser.ArithmeticFirstOrderExpression.Multiply (e1, (), e2) ->
        let e1 = getArithmeticFirstOrderExpr e1
        let e2 = getApplication e2
        BinaryOperation (e1, Multiply, e2)
    | Parser.ArithmeticFirstOrderExpression.Divide (e1, (), e2) ->
        let e1 = getArithmeticFirstOrderExpr e1
        let e2 = getApplication e2
        BinaryOperation (e1, Divide, e2)

let private getArithmeticSecondOrderExpr (e: Parser.ArithmeticSecondOrderExpression) =
    match e with
    | Parser.ArithmeticSecondOrderExpression.Fallthrough e ->
        getArithmeticFirstOrderExpr e
    | Parser.ArithmeticSecondOrderExpression.Add (e1, (), e2) ->
        let e1 = getArithmeticSecondOrderExpr e1
        let e2 = getArithmeticFirstOrderExpr e2
        BinaryOperation (e1, Add, e2)
    | Parser.ArithmeticSecondOrderExpression.Subtract (e1, (), e2) ->
        let e1 = getArithmeticSecondOrderExpr e1
        let e2 = getArithmeticFirstOrderExpr e2
        BinaryOperation (e1, Subtract, e2)

let private getExpression (e: Parser.Expression) =
    match e with
    | Parser.Expression.Value e -> getArithmeticSecondOrderExpr e
    | Parser.Expression.LetIn ((), i, (), v, (), body) ->
        let v = getExpression v
        let body = getExpression body
        Let (i, v, body)

let fromParseTree (parseTree: Parser.Program): Program =
    let (Parser.Program e) = parseTree
    Program [
        Statement.Expression (getExpression e)
    ]