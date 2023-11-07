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

type Program = Program of Expression

let private getAtom (e: Parser.AtomExpression) =
    match e with
    | Parser.AtomExpression.Identifier i -> Identifier i
    | Parser.AtomExpression.DoubleQuotedString s -> StringLiteral s
    | Parser.AtomExpression.Number (i, f) -> NumberLiteral (i, f)
    | Parser.AtomExpression.Paren(_, e, _) -> getExpression e
    | Parser.AtomExpression.Block((), e, ()) -> getExpression e

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
    | Parser.Expression.Binding ((), i, (), v, bindingBody) ->
        let body =
            match bindingBody with
            | Parser.BindingBody.Semicolon ((), body)
            | Parser.BindingBody.NewLine ((), body) -> body

        let v = getExpression v
        let body = getExpression body
        Let (i, v, body)

let fromParseTree (Parser.Program e): Program =
    let e = getExpression e
    Program e