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

let private expression (shape, position) : Ast.Expression =
    { expressionShape = shape
      expressionType = Ast.TypeReference.Create()
      positionInSource = position }

let private mapTerminalEnclosedExpression (ctx: LexicalContext) (e: Parser.TerminalEnclosedExpression) : Ast.Expression * LexicalContext =
    match e with
    | Parser.TerminalEnclosedExpression.Identifier(identifier, pos) ->
        let expressionShape = Ast.IdentifierReference(ctx.GetIdentifier(identifier))
        let expression = expression (expressionShape, pos)
        expression, ctx
    | Parser.TerminalEnclosedExpression.String(string, pos) -> expression (Ast.StringLiteral string, pos), ctx
    | Parser.TerminalEnclosedExpression.Number(integerPart, fractionalPart, pos) -> expression (Ast.NumberLiteral(integerPart, fractionalPart), pos), ctx
    | Parser.TerminalEnclosedExpression.Paren(parenOpen, content, parenClose) ->
        let pos = PositionInSource.fromTo parenOpen parenClose
        let content, _ = mapExpression ctx content
        expression (content.expressionShape, pos), ctx
    | Parser.TerminalEnclosedExpression.Block(_, content, _) ->
        let content, _ = mapExpression ctx content
        content, ctx
    | Parser.TerminalEnclosedExpression.InvalidToken(invalidToken, pos) -> expression (Ast.InvalidToken(invalidToken), pos), ctx

let private mapApplication (ctx: LexicalContext) (e: Parser.Application) =
    match e with
    | Parser.Fallthrough e -> mapTerminalEnclosedExpression ctx e
    | Parser.Application(fn, arg) ->
        let fn, _ = mapApplication ctx fn
        let arg, _ = mapTerminalEnclosedExpression ctx arg
        let application = Ast.Application(fn, arg)
        let pos = PositionInSource.fromTo fn.positionInSource arg.positionInSource
        expression (application, pos), ctx

let private mapBinaryOperator leftOp mapLeft operatorIdentifier operator rightOp mapRight (ctx: LexicalContext) =
    let leftOp, _ = mapLeft ctx leftOp
    let operator = expression (Ast.IdentifierReference(operatorIdentifier), operator)
    let rightOp, _ = mapRight ctx rightOp

    let applyLeft =
        expression (Ast.Application(operator, leftOp), PositionInSource.fromTo leftOp.positionInSource operator.positionInSource)

    let applyRight =
        expression (Ast.Application(applyLeft, rightOp), PositionInSource.fromTo operator.positionInSource rightOp.positionInSource)

    let pos = PositionInSource.fromTo leftOp.positionInSource rightOp.positionInSource
    expression (applyRight.expressionShape, pos), ctx

let private mapArithmeticFirstOrderExpr (ctx: LexicalContext) (e: Parser.ArithmeticFirstOrderExpression) =
    match e with
    | Parser.ArithmeticFirstOrderExpression.Fallthrough e -> mapApplication ctx e
    | Parser.ArithmeticFirstOrderExpression.Multiply(leftOp, operator, rightOp) ->
        mapBinaryOperator leftOp mapArithmeticFirstOrderExpr BuiltIn.Identifiers.opMultiply operator rightOp mapApplication ctx
    | Parser.ArithmeticFirstOrderExpression.Divide(leftOp, operator, rightOp) ->
        mapBinaryOperator leftOp mapArithmeticFirstOrderExpr BuiltIn.Identifiers.opDivide operator rightOp mapApplication ctx

let private mapArithmeticSecondOrderExpr (ctx: LexicalContext) (e: Parser.ArithmeticSecondOrderExpression) =
    match e with
    | Parser.ArithmeticSecondOrderExpression.Fallthrough e -> mapArithmeticFirstOrderExpr ctx e
    | Parser.ArithmeticSecondOrderExpression.Add(leftOp, operator, rightOp) ->
        mapBinaryOperator leftOp mapArithmeticSecondOrderExpr BuiltIn.Identifiers.opAdd operator rightOp mapArithmeticFirstOrderExpr ctx
    | Parser.ArithmeticSecondOrderExpression.Subtract(leftOp, operator, rightOp) ->
        mapBinaryOperator leftOp mapArithmeticSecondOrderExpr BuiltIn.Identifiers.opSubtract operator rightOp mapArithmeticFirstOrderExpr ctx

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
    | Parser.BindingExpression.Binding(let_, (identifierName, _), parameters, _, value) ->
        let parameters, subCtx = mapBindingParameters ctx parameters
        let value, _ = mapArithmeticSecondOrderExpr subCtx value
        let identifier, ctx = ctx.CreateIdentifier(identifierName)
        let binding = Ast.Binding(identifier, parameters, value)
        let pos = PositionInSource.fromTo let_ value.positionInSource
        expression (binding, pos), ctx
    | Parser.BindingExpression.Fallthrough e -> mapArithmeticSecondOrderExpr ctx e

let private mapExpressionConcatenation (ctx: LexicalContext) (e: Parser.ExpressionConcatenation) : Ast.Expression * LexicalContext =
    match e with
    | Parser.ExpressionConcatenation.Concat(head, _, tail) ->
        let head, ctx = mapExpressionConcatenation ctx head
        let tail, ctx = mapBindingExpression ctx tail

        let e =
            match head.expressionShape with
            | Ast.Sequence headSequence -> Ast.Sequence(headSequence @ [ tail ])
            | _ -> Ast.Sequence [ head; tail ]
        let pos = PositionInSource.fromTo head.positionInSource tail.positionInSource

        expression (e, pos), ctx
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
