module internal rec Compiler.AstBuilder

type private LexicalContext =
    { identifiers: Map<string, Ast.Identifier> }

    static member Empty = { identifiers = Map.empty }

    member this.AttachIdentifier(identifier: Ast.Identifier) =
        { this with
            identifiers = this.identifiers |> Map.add identifier.Name identifier }

    member this.CreateIdentifier(identifierName: string) =
        let identifier = Ast.Identifier.Create(identifierName)

        let newContext =
            { this with
                identifiers = this.identifiers |> Map.add identifierName identifier }

        identifier, newContext

    member this.GetIdentifier(identifierName: string) =
        this.identifiers |> Map.find identifierName

let private expression shape : Ast.Expression =
    { expressionShape = shape
      expressionType = Ast.TypeReference.Create() }

let private mapTerminalEnclosedExpression (ctx: LexicalContext) (e: Parser.TerminalEnclosedExpression) : Ast.Expression * LexicalContext =
    match e with
    | Parser.TerminalEnclosedExpression.Identifier(identifier, _) -> Ast.IdentifierReference(ctx.GetIdentifier(identifier)) |> expression, ctx
    | Parser.TerminalEnclosedExpression.String(string, _) -> Ast.StringLiteral string |> expression, ctx
    | Parser.TerminalEnclosedExpression.Number(integerPart, fractionalPart, _) -> Ast.NumberLiteral(integerPart, fractionalPart) |> expression, ctx
    | Parser.TerminalEnclosedExpression.Paren content ->
        let content, _ = mapExpression ctx content
        content, ctx
    | Parser.TerminalEnclosedExpression.Block content ->
        let content, _ = mapExpression ctx content
        content, ctx
    | Parser.TerminalEnclosedExpression.InvalidToken(invalidToken, positionInSource) -> Ast.InvalidToken(invalidToken, positionInSource) |> expression, ctx

let private mapApplication (ctx: LexicalContext) (e: Parser.Application) =
    match e with
    | Parser.Fallthrough e -> mapTerminalEnclosedExpression ctx e
    | Parser.Application(fn, arg) ->
        let fn, _ = mapApplication ctx fn
        let arg, _ = mapTerminalEnclosedExpression ctx arg
        let application = Ast.Application(fn, arg) |> expression
        application, ctx

let private opMultiplyIdentifierReference =
    Ast.IdentifierReference(BuiltIn.Identifiers.opMultiply) |> expression

let private opDivideIdentifierReference =
    Ast.IdentifierReference(BuiltIn.Identifiers.opDivide) |> expression

let private mapArithmeticFirstOrderExpr (ctx: LexicalContext) (e: Parser.ArithmeticFirstOrderExpression) =
    match e with
    | Parser.ArithmeticFirstOrderExpression.Fallthrough e -> mapApplication ctx e
    | Parser.ArithmeticFirstOrderExpression.Multiply(leftOp, rightOp) ->
        let leftOp, _ = mapArithmeticFirstOrderExpr ctx leftOp
        let rightOp, _ = mapApplication ctx rightOp
        let applyLeft = Ast.Application(opMultiplyIdentifierReference, leftOp) |> expression
        let applyRight = Ast.Application(applyLeft, rightOp) |> expression
        applyRight, ctx
    | Parser.ArithmeticFirstOrderExpression.Divide(leftOp, rightOp) ->
        let leftOp, _ = mapArithmeticFirstOrderExpr ctx leftOp
        let rightOp, _ = mapApplication ctx rightOp
        let applyLeft = Ast.Application(opDivideIdentifierReference, leftOp) |> expression
        let applyRight = Ast.Application(applyLeft, rightOp) |> expression
        applyRight, ctx

let private opAddIdentifierReference =
    Ast.IdentifierReference(BuiltIn.Identifiers.opAdd) |> expression

let private opSubtractIdentifierReference =
    Ast.IdentifierReference(BuiltIn.Identifiers.opSubtract) |> expression

let private mapArithmeticSecondOrderExpr (ctx: LexicalContext) (e: Parser.ArithmeticSecondOrderExpression) =
    match e with
    | Parser.ArithmeticSecondOrderExpression.Fallthrough e -> mapArithmeticFirstOrderExpr ctx e
    | Parser.ArithmeticSecondOrderExpression.Add(leftOp, rightOp) ->
        let leftOp, _ = mapArithmeticSecondOrderExpr ctx leftOp
        let rightOp, _ = mapArithmeticFirstOrderExpr ctx rightOp
        let applyLeft = Ast.Application(opAddIdentifierReference, leftOp) |> expression
        let applyRight = Ast.Application(applyLeft, rightOp) |> expression
        applyRight, ctx
    | Parser.ArithmeticSecondOrderExpression.Subtract(leftOp, rightOp) ->
        let leftOp, _ = mapArithmeticSecondOrderExpr ctx leftOp
        let rightOp, _ = mapArithmeticFirstOrderExpr ctx rightOp
        let applyLeft = Ast.Application(opDivideIdentifierReference, leftOp) |> expression
        let applyRight = Ast.Application(applyLeft, rightOp) |> expression
        applyRight, ctx

let private mapBindingParameters (ctx: LexicalContext) (e: Parser.BindingParameters) =
    match e with
    | Parser.BindingParameters.Empty -> [], ctx
    | Parser.BindingParameters.Cons(head, (parameterName, _)) ->
        let parameter, ctx = ctx.CreateIdentifier(parameterName)
        let headParameters, ctx = mapBindingParameters ctx head
        let parameters = parameter :: headParameters
        parameters, ctx

let private mapBindingExpression (ctx: LexicalContext) (e: Parser.BindingExpression) =
    match e with
    | Parser.BindingExpression.Binding((identifierName, _), parameters, value) ->
        let parameters, subCtx = mapBindingParameters ctx parameters
        let value, _ = mapArithmeticSecondOrderExpr subCtx value
        let identifier, ctx = ctx.CreateIdentifier(identifierName)
        let binding = Ast.Binding(identifier, parameters, value) |> expression
        binding, ctx
    | Parser.BindingExpression.Fallthrough e -> mapArithmeticSecondOrderExpr ctx e

let private mapExpressionConcatenation (ctx: LexicalContext) (e: Parser.ExpressionConcatenation) : Ast.Expression * LexicalContext =
    match e with
    | Parser.ExpressionConcatenation.Concat(head, tail) ->
        let head, ctx = mapExpressionConcatenation ctx head
        let tail, ctx = mapBindingExpression ctx tail

        let e =
            match head.expressionShape with
            | Ast.Sequence headSequence -> Ast.Sequence(headSequence @ [ tail ]) |> expression
            | _ -> Ast.Sequence [ head; tail ] |> expression

        e, ctx
    | Parser.ExpressionConcatenation.Fallthrough e -> mapBindingExpression ctx e

let private mapExpression (ctx: LexicalContext) (e: Parser.Expression) : Ast.Expression * LexicalContext =
    match e with
    | Parser.Expression e -> mapExpressionConcatenation ctx e

let buildFromParseTree (parseTree: Parser.Program) : Ast.Program =
    let context =
        LexicalContext.Empty
            .AttachIdentifier(BuiltIn.Identifiers.println)
            .AttachIdentifier(BuiltIn.Identifiers.intToStr)
            .AttachIdentifier(BuiltIn.Identifiers.intToStrFmt)
            .AttachIdentifier(BuiltIn.Identifiers.floatToStr)

    match parseTree with
    | Parser.Program e ->
        let e, _ = mapExpression context e
        Ast.Program e
