module internal rec Compiler.AstBuilder

type private LexicalContext =
    { identifiers: Map<string, Ast.Identifier> }

    static member Empty = { identifiers = Map.empty }

    member this.AttachIdentifier(identifier: Ast.Identifier) =
        { this with
            identifiers = this.identifiers |> Map.add identifier.name identifier }

    member this.CreateIdentifier(identifierName: string) =
        let identifier = Ast.createIdentifier identifierName

        let newContext =
            { this with
                identifiers = this.identifiers |> Map.add identifierName identifier }

        identifier, newContext

    member this.GetIdentifier(identifierName: string) =
        this.identifiers |> Map.find identifierName

let private untyped e : Ast.TypedExpression<unit> = { expression = e; expressionType = () }

let private mapTerminalEnclosedExpression (ctx: LexicalContext) (e: Parser.TerminalEnclosedExpression) : Ast.TypedExpression<unit> * LexicalContext =
    match e with
    | Parser.TerminalEnclosedExpression.Identifier i -> Ast.Identifier(ctx.GetIdentifier(i)) |> untyped, ctx
    | Parser.TerminalEnclosedExpression.String s -> Ast.StringLiteral s |> untyped, ctx
    | Parser.TerminalEnclosedExpression.Number(i, f) -> Ast.NumberLiteral(i, f) |> untyped, ctx
    | Parser.TerminalEnclosedExpression.Paren(_, e, _) ->
        let e, _ = mapExpression ctx e
        e, ctx
    | Parser.TerminalEnclosedExpression.Block((), e, ()) ->
        let e, _ = mapExpression ctx e
        e, ctx

let private mapApplication (ctx: LexicalContext) (e: Parser.Application) =
    match e with
    | Parser.Fallthrough e -> mapTerminalEnclosedExpression ctx e
    | Parser.Application(e1, e2) ->
        let e1, _ = mapApplication ctx e1
        let e2, _ = mapTerminalEnclosedExpression ctx e2
        let e = Ast.Application(e1, e2) |> untyped
        e, ctx

let private mapArithmeticFirstOrderExpr (ctx: LexicalContext) (e: Parser.ArithmeticFirstOrderExpression) =
    match e with
    | Parser.ArithmeticFirstOrderExpression.Fallthrough e -> mapApplication ctx e
    | Parser.ArithmeticFirstOrderExpression.Multiply(e1, (), e2) ->
        let e1, _ = mapArithmeticFirstOrderExpr ctx e1
        let e2, _ = mapApplication ctx e2
        let e = Ast.BinaryOperation(e1, Ast.Multiply, e2) |> untyped
        e, ctx
    | Parser.ArithmeticFirstOrderExpression.Divide(e1, (), e2) ->
        let e1, _ = mapArithmeticFirstOrderExpr ctx e1
        let e2, _ = mapApplication ctx e2
        let e = Ast.BinaryOperation(e1, Ast.Divide, e2) |> untyped
        e, ctx

let private mapArithmeticSecondOrderExpr (ctx: LexicalContext) (e: Parser.ArithmeticSecondOrderExpression) =
    match e with
    | Parser.ArithmeticSecondOrderExpression.Fallthrough e -> mapArithmeticFirstOrderExpr ctx e
    | Parser.ArithmeticSecondOrderExpression.Add(e1, (), e2) ->
        let e1, _ = mapArithmeticSecondOrderExpr ctx e1
        let e2, _ = mapArithmeticFirstOrderExpr ctx e2
        let e = Ast.BinaryOperation(e1, Ast.Add, e2) |> untyped
        e, ctx
    | Parser.ArithmeticSecondOrderExpression.Subtract(e1, (), e2) ->
        let e1, _ = mapArithmeticSecondOrderExpr ctx e1
        let e2, _ = mapArithmeticFirstOrderExpr ctx e2
        let e = Ast.BinaryOperation(e1, Ast.Subtract, e2) |> untyped
        e, ctx

let private mapBindingExpression (ctx: LexicalContext) (e: Parser.BindingExpression) =
    match e with
    | Parser.BindingExpression.Binding((), identifierName, (), value) ->
        let value, _ = mapArithmeticSecondOrderExpr ctx value
        let identifier, ctx = ctx.CreateIdentifier(identifierName)
        Ast.Binding(identifier, value) |> untyped, ctx
    | Parser.BindingExpression.Fallthrough e -> mapArithmeticSecondOrderExpr ctx e

let private mapExpressionConcatenation (ctx: LexicalContext) (e: Parser.ExpressionConcatenation) : Ast.TypedExpression<unit> * LexicalContext =
    match e with
    | Parser.ExpressionConcatenation.Concat(e1, (), e2) ->
        let e1, ctx = mapExpressionConcatenation ctx e1
        let e2, ctx = mapBindingExpression ctx e2

        let e =
            match e1.expression with
            | Ast.Sequence s -> Ast.Sequence(s @ [ e2 ]) |> untyped
            | _ -> Ast.Sequence [ e1; e2 ] |> untyped

        e, ctx
    | Parser.ExpressionConcatenation.Fallthrough e -> mapBindingExpression ctx e

let private mapExpression (ctx: LexicalContext) (e: Parser.Expression) : Ast.TypedExpression<unit> * LexicalContext =
    match e with
    | Parser.Expression e -> mapExpressionConcatenation ctx e

let buildFromParseTree (parseTree: Parser.Program) : Ast.Program<unit> =
    let context =
        LexicalContext.Empty
            .AttachIdentifier(BuiltIn.Identifiers.println)
            .AttachIdentifier(BuiltIn.Identifiers.intToStr)
            .AttachIdentifier(BuiltIn.Identifiers.intToStr2)
            .AttachIdentifier(BuiltIn.Identifiers.floatToStr)

    match parseTree with
    | Parser.Program e ->
        let e, _ = mapExpression context e
        Ast.Program e
