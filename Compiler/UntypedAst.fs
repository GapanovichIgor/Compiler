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
    | Let of string * Expression
    | Concat of Expression * Expression

type Program = Program of Expression

let private mapTerminalEnclosedExpression (e: Parser.TerminalEnclosedExpression) =
    match e with
    | Parser.TerminalEnclosedExpression.Identifier i -> Identifier i
    | Parser.TerminalEnclosedExpression.String s -> StringLiteral s
    | Parser.TerminalEnclosedExpression.Number (i, f) -> NumberLiteral (i, f)
    | Parser.TerminalEnclosedExpression.Paren(_, e, _) -> mapExpression e
    | Parser.TerminalEnclosedExpression.Block((), e, ()) -> mapExpression e

let private mapApplication (e: Parser.Application) =
    match e with
    | Parser.Fallthrough e -> mapTerminalEnclosedExpression e
    | Parser.Application (e1, e2) -> Application (mapApplication e1, mapTerminalEnclosedExpression e2)

let private mapArithmeticFirstOrderExpr (e: Parser.ArithmeticFirstOrderExpression) =
    match e with
    | Parser.ArithmeticFirstOrderExpression.Fallthrough e ->
        mapApplication e
    | Parser.ArithmeticFirstOrderExpression.Multiply (e1, (), e2) ->
        let e1 = mapArithmeticFirstOrderExpr e1
        let e2 = mapApplication e2
        BinaryOperation (e1, Multiply, e2)
    | Parser.ArithmeticFirstOrderExpression.Divide (e1, (), e2) ->
        let e1 = mapArithmeticFirstOrderExpr e1
        let e2 = mapApplication e2
        BinaryOperation (e1, Divide, e2)

let private mapArithmeticSecondOrderExpr (e: Parser.ArithmeticSecondOrderExpression) =
    match e with
    | Parser.ArithmeticSecondOrderExpression.Fallthrough e ->
        mapArithmeticFirstOrderExpr e
    | Parser.ArithmeticSecondOrderExpression.Add (e1, (), e2) ->
        let e1 = mapArithmeticSecondOrderExpr e1
        let e2 = mapArithmeticFirstOrderExpr e2
        BinaryOperation (e1, Add, e2)
    | Parser.ArithmeticSecondOrderExpression.Subtract (e1, (), e2) ->
        let e1 = mapArithmeticSecondOrderExpr e1
        let e2 = mapArithmeticFirstOrderExpr e2
        BinaryOperation (e1, Subtract, e2)

let private mapBindingExpression (e: Parser.BindingExpression) =
    match e with
    | Parser.BindingExpression.Binding ((), i, (), v) ->
        let v = mapArithmeticSecondOrderExpr v
        Let (i, v)
    | Parser.BindingExpression.Fallthrough e ->
        mapArithmeticSecondOrderExpr e

let private mapExpressionConcatenation (e: Parser.ExpressionConcatenation) =
    match e with
    | Parser.ExpressionConcatenation.Concat (e1, (), e2) ->
        let e1 = mapExpressionConcatenation e1
        let e2 = mapBindingExpression e2
        Concat (e1, e2)
    | Parser.ExpressionConcatenation.Fallthrough e ->
        mapBindingExpression e

let private mapExpression (e: Parser.Expression) =
    match e with
    | Parser.Expression e -> mapExpressionConcatenation e

let fromParseTree (parseTree: Parser.Program): Program =
    match parseTree with
    | Parser.Program e ->
        let e = mapExpression e
        Program e