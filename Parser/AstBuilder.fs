module internal rec Compiler.AstBuilder

open Common

type private LexicalContext =
    { identifiers: Map<string, Identifier> }

    static member Empty = { identifiers = Map.empty }

    member this.AttachIdentifier(identifier: Identifier) =
        { identifiers = this.identifiers |> Map.add identifier.Name identifier }

    member this.CreateIdentifier(identifierName: string) =
        let identifier = Identifier.Create(identifierName)

        let newContext = { identifiers = this.identifiers |> Map.add identifierName identifier }

        identifier, newContext

    member this.GetIdentifier(identifierName: string) =
        this.identifiers |> Map.find identifierName

let private expression (shape, position) : Ast.Expression =
    let typeRefHint =
        match shape with
        | Ast.IdentifierReference identifier -> $"idRef({identifier.Name})"
        | Ast.NumberLiteral _ -> "numLiteral"
        | Ast.StringLiteral _ -> "strLiteral"
        | Ast.Application _ -> "application"
        | Ast.Binding _ -> "binding"
        | Ast.Sequence _ -> "sequence"
        | Ast.InvalidToken _ -> "invalidToken"

    { expressionShape = shape
      expressionType = TypeReference(typeRefHint)
      positionInSource = position }

let private mapTerminalEnclosedExpression (ctx: LexicalContext) (e: ParserInternal.TerminalEnclosedExpression) : Ast.Expression * LexicalContext =
    match e with
    | ParserInternal.TerminalEnclosedExpression.Identifier(identifier, pos) ->
        let expressionShape = Ast.IdentifierReference(ctx.GetIdentifier(identifier))
        let expression = expression (expressionShape, pos)
        expression, ctx
    | ParserInternal.TerminalEnclosedExpression.String(string, pos) -> expression (Ast.StringLiteral string, pos), ctx
    | ParserInternal.TerminalEnclosedExpression.Number(integerPart, fractionalPart, pos) -> expression (Ast.NumberLiteral(integerPart, fractionalPart), pos), ctx
    | ParserInternal.TerminalEnclosedExpression.Paren(parenOpen, content, parenClose) ->
        let pos = PositionInSource.fromTo parenOpen parenClose
        let content, _ = mapExpression ctx content
        expression (content.expressionShape, pos), ctx
    | ParserInternal.TerminalEnclosedExpression.Block(_, content, _) ->
        let content, _ = mapExpression ctx content
        content, ctx
    | ParserInternal.TerminalEnclosedExpression.InvalidToken(invalidToken, pos) -> expression (Ast.InvalidToken(invalidToken), pos), ctx

let private mapApplication (ctx: LexicalContext) (e: ParserInternal.Application) =
    match e with
    | ParserInternal.Fallthrough e -> mapTerminalEnclosedExpression ctx e
    | ParserInternal.Application(fn, arg) ->
        let fn, _ = mapApplication ctx fn
        let arg, _ = mapTerminalEnclosedExpression ctx arg
        let applicationReference = ApplicationId()
        let application = Ast.Application(applicationReference, fn, arg)
        let pos = PositionInSource.fromTo fn.positionInSource arg.positionInSource
        expression (application, pos), ctx

let private mapBinaryOperator leftOp mapLeft operatorIdentifier operator rightOp mapRight (ctx: LexicalContext) =
    let leftOp, _ = mapLeft ctx leftOp
    let operator = expression (Ast.IdentifierReference(operatorIdentifier), operator)
    let rightOp, _ = mapRight ctx rightOp

    let applyLeft =
        expression (Ast.Application(ApplicationId(), operator, leftOp), PositionInSource.fromTo leftOp.positionInSource operator.positionInSource)

    let applyRight =
        expression (Ast.Application(ApplicationId(), applyLeft, rightOp), PositionInSource.fromTo operator.positionInSource rightOp.positionInSource)

    let pos = PositionInSource.fromTo leftOp.positionInSource rightOp.positionInSource
    expression (applyRight.expressionShape, pos), ctx

let private mapArithmeticFirstOrderExpr (ctx: LexicalContext) (e: ParserInternal.ArithmeticFirstOrderExpression) =
    match e with
    | ParserInternal.ArithmeticFirstOrderExpression.Fallthrough e -> mapApplication ctx e
    | ParserInternal.ArithmeticFirstOrderExpression.Multiply(leftOp, operator, rightOp) ->
        mapBinaryOperator leftOp mapArithmeticFirstOrderExpr BuiltIn.Identifiers.opMultiply operator rightOp mapApplication ctx
    | ParserInternal.ArithmeticFirstOrderExpression.Divide(leftOp, operator, rightOp) ->
        mapBinaryOperator leftOp mapArithmeticFirstOrderExpr BuiltIn.Identifiers.opDivide operator rightOp mapApplication ctx

let private mapArithmeticSecondOrderExpr (ctx: LexicalContext) (e: ParserInternal.ArithmeticSecondOrderExpression) =
    match e with
    | ParserInternal.ArithmeticSecondOrderExpression.Fallthrough e -> mapArithmeticFirstOrderExpr ctx e
    | ParserInternal.ArithmeticSecondOrderExpression.Add(leftOp, operator, rightOp) ->
        mapBinaryOperator leftOp mapArithmeticSecondOrderExpr BuiltIn.Identifiers.opAdd operator rightOp mapArithmeticFirstOrderExpr ctx
    | ParserInternal.ArithmeticSecondOrderExpression.Subtract(leftOp, operator, rightOp) ->
        mapBinaryOperator leftOp mapArithmeticSecondOrderExpr BuiltIn.Identifiers.opSubtract operator rightOp mapArithmeticFirstOrderExpr ctx

let private mapBindingParameters (ctx: LexicalContext) (e: ParserInternal.BindingParameters) =
    match e with
    | ParserInternal.BindingParameters.Empty -> [], ctx
    | ParserInternal.BindingParameters.Cons(head, (parameterName, _)) ->
        let parameter, ctx = ctx.CreateIdentifier(parameterName)
        let headParameters, ctx = mapBindingParameters ctx head
        let parameters = headParameters @ [ parameter ]
        parameters, ctx

let private mapBindingExpression (ctx: LexicalContext) (e: ParserInternal.BindingExpression) =
    match e with
    | ParserInternal.BindingExpression.Binding(let_, (identifierName, _), parameters, _, value) ->
        let parameters, subCtx = mapBindingParameters ctx parameters
        let value, _ = mapArithmeticSecondOrderExpr subCtx value
        let identifier, ctx = ctx.CreateIdentifier(identifierName)
        let binding = Ast.Binding(identifier, parameters, value)
        let pos = PositionInSource.fromTo let_ value.positionInSource
        expression (binding, pos), ctx
    | ParserInternal.BindingExpression.Fallthrough e -> mapArithmeticSecondOrderExpr ctx e

let private mapExpressionConcatenation (ctx: LexicalContext) (e: ParserInternal.ExpressionConcatenation) : Ast.Expression * LexicalContext =
    match e with
    | ParserInternal.ExpressionConcatenation.Concat(head, _, tail) ->
        let head, ctx = mapExpressionConcatenation ctx head
        let tail, ctx = mapBindingExpression ctx tail

        let e =
            match head.expressionShape with
            | Ast.Sequence headSequence -> Ast.Sequence(headSequence @ [ tail ])
            | _ -> Ast.Sequence [ head; tail ]
        let pos = PositionInSource.fromTo head.positionInSource tail.positionInSource

        expression (e, pos), ctx
    | ParserInternal.ExpressionConcatenation.Fallthrough e -> mapBindingExpression ctx e

let private mapExpression (ctx: LexicalContext) (e: ParserInternal.Expression) : Ast.Expression * LexicalContext =
    match e with
    | ParserInternal.Expression e -> mapExpressionConcatenation ctx e

let buildFromParseTree (parseTree: ParserInternal.Program) : Ast.Program =
    let context =
        LexicalContext.Empty
            .AttachIdentifier(BuiltIn.Identifiers.println)
            .AttachIdentifier(BuiltIn.Identifiers.intToStr)
            .AttachIdentifier(BuiltIn.Identifiers.intToStrFmt)
            .AttachIdentifier(BuiltIn.Identifiers.floatToStr)
            .AttachIdentifier(BuiltIn.Identifiers.failwith)

    match parseTree with
    | ParserInternal.Program e ->
        let e, _ = mapExpression context e
        Ast.Program e
