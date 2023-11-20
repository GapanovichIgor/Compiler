module internal rec Compiler.CsTranspiler

open System.Collections.Generic

type private TypeInformation =
    { getExpressionType: Ast.Expression -> CsAst.Type option
      getIdentifierType: Identifier -> CsAst.Type }

type private EnclosingFunctionBodyContext =
    { addStatement: CsAst.Statement -> unit
      createIdentifier: unit -> CsAst.Identifier
      mapIdentifier: Identifier -> CsAst.Identifier
      typeInformation: TypeInformation
      getStatements: unit -> CsAst.Statement list }

let private createEnclosingFunctionBodyContext (typeInformation: TypeInformation) =
    let statements = List()
    let identifierNameSet = HashSet<string>()
    let identifierMap = Dictionary<Identifier, CsAst.Identifier>()

    let mutable identifierNameCounter = 0

    let createUniqueIdentifierName baseName =
        let createId () =
            identifierNameCounter <- identifierNameCounter + 1
            baseName + string identifierNameCounter
            |> Identifier.Create

        let mutable identifier = createId ()

        while not (identifierNameSet.Add(identifier.Name)) do
            identifier <- createId ()

        identifier

    let createIdentifier () =
        let i = createUniqueIdentifierName "id"
        identifierMap[i] <- i.Name
        i.Name

    let mapIdentifier (identifier: Identifier) =
        match identifierMap.TryGetValue(identifier) with
        | true, identifierName -> identifierName
        | false, _ ->
            if identifierNameSet.Add(identifier.Name) then
                identifierMap[identifier] <- identifier.Name
                identifier.Name
            else
                let uniqueIdentifierName = createUniqueIdentifierName identifier.Name
                identifierMap[identifier] <- uniqueIdentifierName.Name
                uniqueIdentifierName.Name

    mapIdentifier BuiltIn.Identifiers.opAdd |> ignore
    mapIdentifier BuiltIn.Identifiers.opSubtract |> ignore
    mapIdentifier BuiltIn.Identifiers.opMultiply |> ignore
    mapIdentifier BuiltIn.Identifiers.opDivide |> ignore
    mapIdentifier BuiltIn.Identifiers.println |> ignore
    mapIdentifier BuiltIn.Identifiers.intToStr |> ignore
    mapIdentifier BuiltIn.Identifiers.intToStrFmt |> ignore
    mapIdentifier BuiltIn.Identifiers.floatToStr |> ignore

    { addStatement = statements.Add
      createIdentifier = createIdentifier
      mapIdentifier = mapIdentifier
      typeInformation = typeInformation
      getStatements = fun () -> statements |> List.ofSeq }

let private (|Is|_|) v1 v2 = if v1 = v2 then Some() else None

let private mapBinaryOperator (operator: Identifier) =
    match operator with
    | Is BuiltIn.Identifiers.opAdd -> Some CsAst.BinaryOperator.Add
    | Is BuiltIn.Identifiers.opSubtract -> Some CsAst.BinaryOperator.Subtract
    | Is BuiltIn.Identifiers.opMultiply -> Some CsAst.BinaryOperator.Multiply
    | Is BuiltIn.Identifiers.opDivide -> Some CsAst.BinaryOperator.Divide
    | _ -> None

let private (|BinaryOp|_|) identifier = mapBinaryOperator identifier

let private mapTypeToCsType (t: Type) : CsAst.Type option =
    match t with
    | Is BuiltIn.Types.int -> CsAst.Type.ValueType "System.Int32" |> Some
    | Is BuiltIn.Types.float -> CsAst.Type.ValueType "System.Single" |> Some
    | Is BuiltIn.Types.string -> CsAst.Type.ValueType "System.String" |> Some
    | Is BuiltIn.Types.unit -> None
    | FunctionType(parameterType, resultType) ->
        let parameterType = mapTypeToCsType parameterType
        let resultType = mapTypeToCsType resultType

        let parameterTypes =
            match parameterType with
            | Some p -> [ p ]
            | None -> []

        CsAst.Type.FunctionType(parameterTypes, resultType) |> Some
    | _ -> None

let private mapTypeConstructorToCsType (typeCtor: TypeConstructor) : CsAst.Type option =
    match typeCtor with
    | NullaryTypeConstructor t -> mapTypeToCsType t
    | TypeConstructor _ -> None

let private mapExpression (ctx: EnclosingFunctionBodyContext) (e: Ast.Expression) : CsAst.Expression option =
    match e.expressionShape with
    | Ast.IdentifierReference i -> CsAst.Expression.IdentifierReference(ctx.mapIdentifier i) |> Some
    | Ast.NumberLiteral(i, f) ->
        ctx.typeInformation.getExpressionType e
        |> Option.map (fun t -> CsAst.NumberLiteral(i, f, t))
    | Ast.StringLiteral s -> CsAst.Expression.StringLiteral s |> Some
    | Ast.Application(f, arg) ->
        match f.expressionShape with
        | Ast.Application({ expressionShape = Ast.IdentifierReference(BinaryOp op) }, leftOp) ->
            let leftOp = mapExpression ctx leftOp
            let rightOp = mapExpression ctx arg

            match leftOp, rightOp with
            | Some e1, Some e2 -> CsAst.Expression.BinaryOperation(e1, op, e2) |> Some
            | _ -> failwith "Operands of a binary operation should not be statements"
        | Ast.IdentifierReference i ->
            let arg = mapExpression ctx arg

            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            CsAst.FunctionCall(ctx.mapIdentifier i, args) |> Some
        | _ ->
            let varT =
                ctx.typeInformation.getExpressionType f
                |> Option.require "Function expression must have a type"

            let varId = ctx.createIdentifier ()

            let varBody =
                mapExpression ctx f |> Option.require "Function expression must have a value"

            ctx.addStatement (CsAst.Statement.Var(varT, varId, varBody))
            let arg = mapExpression ctx arg

            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            CsAst.FunctionCall(varId, args) |> Some
    | Ast.Binding(identifier, [], body) ->
        let bodyType =
            ctx.typeInformation.getExpressionType body
            |> Option.defaultWith (fun () -> failwith "")

        let body = mapExpression ctx body

        match body with
        | Some body -> ctx.addStatement (CsAst.Statement.Var(bodyType, ctx.mapIdentifier identifier, body))
        | None -> ()

        None
    | Ast.Binding(identifier, parameters, body) ->
        let identifierType = ctx.typeInformation.getIdentifierType identifier
        let identifier = ctx.mapIdentifier identifier

        let rec loop ctx identifierType identifier parameters =
            match parameters with
            | [] -> failwith "Impossible state"
            | [ parameterIdentifier ] ->
                let returnType =
                    match identifierType with
                    | CsAst.Type.FunctionType(_, resultType) -> resultType
                    | _ -> failwith "Identifier type must be a function"
                // let returnType = ctx.typeInformation.getExpressionType body
                let body = mapFunctionBody ctx.typeInformation body
                let parameterType = ctx.typeInformation.getIdentifierType parameterIdentifier
                let parameterIdentifier = ctx.mapIdentifier parameterIdentifier

                CsAst.Statement.LocalFunction(returnType, identifier, [ parameterType, parameterIdentifier ], body)
                |> ctx.addStatement
            | parameterIdentifier :: parametersRest ->
                let parameterType = ctx.typeInformation.getIdentifierType parameterIdentifier
                let parameterIdentifier = ctx.mapIdentifier parameterIdentifier

                let subFunctionIdentifier = ctx.createIdentifier ()

                let subFunctionIdentifierType =
                    match identifierType with
                    | CsAst.Type.FunctionType(_, resultType) -> resultType |> Option.require "Identifier type must be a function that returns a function"
                    | _ -> failwith "Identifier type must be a function"

                let subCtx = createEnclosingFunctionBodyContext ctx.typeInformation
                loop subCtx subFunctionIdentifierType subFunctionIdentifier parametersRest

                CsAst.Statement.Return(CsAst.Expression.IdentifierReference(subFunctionIdentifier))
                |> subCtx.addStatement

                let body = subCtx.getStatements ()

                CsAst.Statement.LocalFunction(Some subFunctionIdentifierType, identifier, [ parameterType, parameterIdentifier ], body)
                |> ctx.addStatement

        loop ctx identifierType identifier parameters

        None
    | Ast.Sequence es ->
        let rec loop es =
            match es with
            | [] -> None
            | [ e ] -> mapExpression ctx e
            | s :: esRest ->
                mapStatements ctx s false
                loop esRest

        loop es
    | Ast.InvalidToken _ -> failwith "Invalid AST"

let rec private mapStatements (ctx: EnclosingFunctionBodyContext) (e: Ast.Expression) (generateReturn: bool) : unit =
    match e.expressionShape with
    | Ast.Sequence es ->
        for i = 0 to es.Length - 1 do
            let e = es[i]
            let generateReturn = generateReturn && i = es.Length - 1
            mapStatements ctx e generateReturn
    | Ast.Binding _ -> mapExpression ctx e |> ignore
    | _ when generateReturn ->
        let e = mapExpression ctx e |> Option.require "Expression for a return statement must have a value"
        CsAst.Statement.Return e |> ctx.addStatement
    | Ast.Application _ ->
        let e = mapExpression ctx e
        match e with
        | Some e ->
            match e with
            | CsAst.Expression.FunctionCall (i, args) ->
                CsAst.Statement.FunctionCall (i, args) |> ctx.addStatement
            | _ -> failwith "Expected a FunctionCall expression"
        | None -> ()
    | _ -> failwith "Can not create a statement out of this expression"

let private mapFunctionBody (typeInfo: TypeInformation) (e: Ast.Expression) : CsAst.Statement list =
    let ctx = createEnclosingFunctionBodyContext typeInfo

    let generateReturn = (typeInfo.getExpressionType e) <> None

    mapStatements ctx e generateReturn

    ctx.getStatements ()

let transpile (ast: Ast.Program, typeInformation: TypeSolver.TypeMap) : CsAst.Program =
    let (Ast.Program e) = ast

    let typeInformation =
        { getExpressionType = fun e -> typeInformation.typeReferenceTypes[e.expressionType] |> mapTypeConstructorToCsType
          getIdentifierType = fun i -> typeInformation.identifierTypes[i] |> mapTypeConstructorToCsType |> Option.get }

    let statements = mapFunctionBody typeInformation e

    CsAst.Program statements
