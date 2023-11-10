module internal rec Compiler.AstBuilder

let private untyped e : Ast.TypedExpression<unit> = { expression = e; expressionType = () }

let private mapTerminalEnclosedExpression (e: Parser.TerminalEnclosedExpression) =
    match e with
    | Parser.TerminalEnclosedExpression.Identifier i -> Ast.Identifier i |> untyped
    | Parser.TerminalEnclosedExpression.String s -> Ast.StringLiteral s |> untyped
    | Parser.TerminalEnclosedExpression.Number(i, f) -> Ast.NumberLiteral(i, f) |> untyped
    | Parser.TerminalEnclosedExpression.Paren(_, e, _) -> mapExpression e
    | Parser.TerminalEnclosedExpression.Block((), e, ()) -> mapExpression e

let private mapApplication (e: Parser.Application) =
    match e with
    | Parser.Fallthrough e -> mapTerminalEnclosedExpression e
    | Parser.Application(e1, e2) -> Ast.Application(mapApplication e1, mapTerminalEnclosedExpression e2) |> untyped

let private mapArithmeticFirstOrderExpr (e: Parser.ArithmeticFirstOrderExpression) =
    match e with
    | Parser.ArithmeticFirstOrderExpression.Fallthrough e -> mapApplication e
    | Parser.ArithmeticFirstOrderExpression.Multiply(e1, (), e2) ->
        let e1 = mapArithmeticFirstOrderExpr e1
        let e2 = mapApplication e2
        Ast.BinaryOperation(e1, Ast.Multiply, e2) |> untyped
    | Parser.ArithmeticFirstOrderExpression.Divide(e1, (), e2) ->
        let e1 = mapArithmeticFirstOrderExpr e1
        let e2 = mapApplication e2
        Ast.BinaryOperation(e1, Ast.Divide, e2) |> untyped

let private mapArithmeticSecondOrderExpr (e: Parser.ArithmeticSecondOrderExpression) =
    match e with
    | Parser.ArithmeticSecondOrderExpression.Fallthrough e -> mapArithmeticFirstOrderExpr e
    | Parser.ArithmeticSecondOrderExpression.Add(e1, (), e2) ->
        let e1 = mapArithmeticSecondOrderExpr e1
        let e2 = mapArithmeticFirstOrderExpr e2
        Ast.BinaryOperation(e1, Ast.Add, e2) |> untyped
    | Parser.ArithmeticSecondOrderExpression.Subtract(e1, (), e2) ->
        let e1 = mapArithmeticSecondOrderExpr e1
        let e2 = mapArithmeticFirstOrderExpr e2
        Ast.BinaryOperation(e1, Ast.Subtract, e2) |> untyped

let private mapBindingExpression (e: Parser.BindingExpression) =
    match e with
    | Parser.BindingExpression.Binding((), i, (), v) ->
        let v = mapArithmeticSecondOrderExpr v
        Ast.Let(i, v) |> untyped
    | Parser.BindingExpression.Fallthrough e -> mapArithmeticSecondOrderExpr e

let private mapExpressionConcatenation (e: Parser.ExpressionConcatenation) : Ast.TypedExpression<unit> =
    match e with
    | Parser.ExpressionConcatenation.Concat(e1, (), e2) ->
        let e1 = mapExpressionConcatenation e1
        let e2 = mapBindingExpression e2

        match e1.expression with
        | Ast.Sequence s -> Ast.Sequence(s @ [ e2 ]) |> untyped
        | _ -> Ast.Sequence [ e1; e2 ] |> untyped
    | Parser.ExpressionConcatenation.Fallthrough e -> mapBindingExpression e

let private mapExpression (e: Parser.Expression) =
    match e with
    | Parser.Expression e -> mapExpressionConcatenation e

let buildFromParseTree (parseTree: Parser.Program) : Ast.Program<unit> =
    match parseTree with
    | Parser.Program e ->
        let e = mapExpression e
        Ast.Program e
