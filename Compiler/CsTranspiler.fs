module internal rec Compiler.CsTranspiler

open System.Collections.Generic
open Common

type private TypeInformation =
    { getExpressionType: Ast.Expression -> CsAst.Type option
      getIdentifierType: Identifier -> CsAst.Type }

type private IdentifierScope private (parentMap, parentCounter) =
    let set = HashSet<CsAst.Identifier>()
    let map = Dictionary<Identifier, CsAst.Identifier>()

    let mutable uniqueNameCounter = parentCounter

    do
        for k, v in parentMap do
            set.Add(v) |> ignore
            map.Add(k, v)

    let createUniqueName baseName =
        let createName () =
            uniqueNameCounter <- uniqueNameCounter + 1
            baseName + string uniqueNameCounter

        let mutable name = createName ()

        while not (set.Add(name)) do
            name <- createName ()

        name

    member private _.IdentifierMap = map

    member private _.IdentifierNameCounter = uniqueNameCounter

    static member CreateGlobalScope() = IdentifierScope(Seq.empty, 0)

    member _.CreateSubScope() =
        let map = map :> seq<KeyValuePair<_, _>> |> Seq.map (fun kv -> (kv.Key, kv.Value))

        IdentifierScope(map, uniqueNameCounter)

    member _.CreateIdentifier() =
        let name = createUniqueName "id"
        map[Identifier.Create(name)] <- name
        name

    member _.MapIdentifier(identifier: Identifier) =
        match map.TryGetValue(identifier) with
        | true, name -> name
        | false, _ ->
            if set.Add(identifier.Name) then
                map[identifier] <- identifier.Name
                identifier.Name
            else
                let name = createUniqueName identifier.Name
                map[identifier] <- name
                name

    member this.AttachIdentifier(identifier: Identifier) =
        this.MapIdentifier(identifier) |> ignore

type private TypeScope
    private
    (
        typeInformation: TypeSolver.TypeInformation,
        typeScopeReference: TypeScopeReference option,
        parentVariableTypeNameMap,
        parentUniqueVariableTypeNameCounter
    ) =

    let variableTypeIds =
        match typeScopeReference with
        | Some typeScopeReference -> typeInformation.typeScopes[typeScopeReference] |> List.ofSeq
        | None -> []

    let variableTypeNameSet = HashSet<CsAst.TypeIdentifier>()
    let variableTypeNameMap = Dictionary<VariableTypeId, CsAst.TypeIdentifier>()

    let mutable uniqueVariableTypeNameCounter = parentUniqueVariableTypeNameCounter

    do
        for k, v in parentVariableTypeNameMap do
            variableTypeNameSet.Add(v) |> ignore
            variableTypeNameMap.Add(k, v)

    let createUniqueVariableTypeName () =
        let createName () =
            uniqueVariableTypeNameCounter <- uniqueVariableTypeNameCounter + 1
            "T" + string uniqueVariableTypeNameCounter

        let mutable name = createName ()

        while not (variableTypeNameSet.Add(name)) do
            name <- createName ()

        name

    member this.MapTypeToCsType(t: Type) : CsAst.Type option =
        match t with
        | Is BuiltIn.Types.int -> CsAst.Type.AtomType "System.Int32" |> Some
        | Is BuiltIn.Types.float -> CsAst.Type.AtomType "System.Single" |> Some
        | Is BuiltIn.Types.string -> CsAst.Type.AtomType "System.String" |> Some
        | Is BuiltIn.Types.unit -> None
        | FunctionType(parameterType, resultType) ->
            let parameterType = this.MapTypeToCsType(parameterType)
            let resultType = this.MapTypeToCsType(resultType)

            let parameterTypes =
                match parameterType with
                | Some p -> [ p ]
                | None -> []

            CsAst.Type.FunctionType(parameterTypes, resultType) |> Some
        | VariableType variableTypeId -> CsAst.Type.AtomType(this.MapVariableType(variableTypeId)) |> Some

        | _ -> failwith "TODO"

    static member CreateGlobalScope(typeInformation) =
        TypeScope(typeInformation, None, Seq.empty, 0)

    member _.CreateSubScope(variableTypeScopeReference) =
        let variableTypeNameMap =
            variableTypeNameMap :> seq<KeyValuePair<_, _>>
            |> Seq.map (fun kv -> (kv.Key, kv.Value))

        TypeScope(typeInformation, Some variableTypeScopeReference, variableTypeNameMap, uniqueVariableTypeNameCounter)

    member this.GetExpressionType(expression: Ast.Expression) : CsAst.Type option =
        typeInformation.typeReferenceTypes[expression.expressionType]
        |> this.MapTypeToCsType

    member this.GetIdentifierType(identifier: Identifier) : CsAst.Type =
        typeInformation.identifierTypes[identifier]
        |> this.MapTypeToCsType
        |> Option.get

    member _.MapVariableType(identifier: VariableTypeId) : CsAst.TypeIdentifier =
        match variableTypeNameMap.TryGetValue(identifier) with
        | true, name -> name
        | false, _ ->
            let name = createUniqueVariableTypeName ()
            variableTypeNameMap[identifier] <- name
            name

    member this.GetTypeParameterNames() : CsAst.TypeIdentifier list =
        variableTypeIds |> Seq.map this.MapVariableType |> List.ofSeq

type private EnclosingFunctionBodyContext(identifierScope: IdentifierScope, typeScope: TypeScope) =
    let statements = List()

    static member CreateFromParent(identifierScope: IdentifierScope, typeScope: TypeScope, typeScopeReference: TypeScopeReference) =
        EnclosingFunctionBodyContext(identifierScope.CreateSubScope(), typeScope.CreateSubScope(typeScopeReference))

    member val IdentifierScope: IdentifierScope = identifierScope

    member val TypeScope: TypeScope = typeScope

    member _.AddStatement(statement: CsAst.Statement) : unit = statements.Add(statement)

    member _.GetStatements() : CsAst.Statement list = statements |> List.ofSeq

let private mapBinaryOperator (operator: Identifier) =
    match operator with
    | Is BuiltIn.Identifiers.opAdd -> Some CsAst.BinaryOperator.Add
    | Is BuiltIn.Identifiers.opSubtract -> Some CsAst.BinaryOperator.Subtract
    | Is BuiltIn.Identifiers.opMultiply -> Some CsAst.BinaryOperator.Multiply
    | Is BuiltIn.Identifiers.opDivide -> Some CsAst.BinaryOperator.Divide
    | _ -> None

let private (|BinaryOp|_|) identifier = mapBinaryOperator identifier

let private mapExpression (ctx: EnclosingFunctionBodyContext) (e: Ast.Expression) : CsAst.Expression option =
    match e.expressionShape with
    | Ast.IdentifierReference i ->
        CsAst.Expression.IdentifierReference(ctx.IdentifierScope.MapIdentifier i)
        |> Some
    | Ast.NumberLiteral(i, f) ->
        ctx.TypeScope.GetExpressionType(e)
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

            CsAst.FunctionCall(ctx.IdentifierScope.MapIdentifier i, args) |> Some
        | _ ->
            let varT =
                ctx.TypeScope.GetExpressionType(f)
                |> Option.require "Function expression must have a type"

            let varId = ctx.IdentifierScope.CreateIdentifier()

            let varBody =
                mapExpression ctx f |> Option.require "Function expression must have a value"

            ctx.AddStatement(CsAst.Statement.Var(varT, varId, varBody))
            let arg = mapExpression ctx arg

            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            CsAst.FunctionCall(varId, args) |> Some
    | Ast.Binding(identifier, _, [], body) ->
        let bodyType =
            ctx.TypeScope.GetExpressionType(body)
            |> Option.defaultWith (fun () -> failwith "")

        let body = mapExpression ctx body

        match body with
        | Some body -> ctx.AddStatement(CsAst.Statement.Var(bodyType, ctx.IdentifierScope.MapIdentifier(identifier), body))
        | None -> ()

        None
    | Ast.Binding(identifier, variableTypeScopeReference, parameters, body) ->
        let identifierType = ctx.TypeScope.GetIdentifierType(identifier)
        let identifier = ctx.IdentifierScope.MapIdentifier(identifier)

        let rec loop (ctx: EnclosingFunctionBodyContext) identifierType identifier parameters first =
            match parameters with
            | [] -> failwith "Impossible state"
            | [ parameterIdentifier ] ->
                let returnType =
                    match identifierType with
                    | CsAst.Type.FunctionType(_, resultType) -> resultType
                    | _ -> failwith "Identifier type must be a function"

                let identifierScope = ctx.IdentifierScope.CreateSubScope()
                let typeScope = ctx.TypeScope.CreateSubScope(variableTypeScopeReference)

                let body = mapFunctionBody identifierScope typeScope body

                let parameterType = ctx.TypeScope.GetIdentifierType(parameterIdentifier)
                let parameterIdentifier = ctx.IdentifierScope.MapIdentifier(parameterIdentifier)

                let typeParameters = if first then typeScope.GetTypeParameterNames() else []

                CsAst.Statement.LocalFunction(returnType, identifier, typeParameters, [ parameterType, parameterIdentifier ], body)
                |> ctx.AddStatement
            | parameterIdentifier :: parametersRest ->
                let parameterType = ctx.TypeScope.GetIdentifierType(parameterIdentifier)
                let parameterIdentifier = ctx.IdentifierScope.MapIdentifier(parameterIdentifier)

                let subFunctionIdentifier = ctx.IdentifierScope.CreateIdentifier()

                let subFunctionIdentifierType =
                    match identifierType with
                    | CsAst.Type.FunctionType(_, resultType) ->
                        resultType
                        |> Option.require "Identifier type must be a function that returns a function"
                    | _ -> failwith "Identifier type must be a function"

                let identifierScope = ctx.IdentifierScope.CreateSubScope()
                let typeScope = ctx.TypeScope.CreateSubScope(variableTypeScopeReference)

                let subCtx =
                    EnclosingFunctionBodyContext.CreateFromParent(identifierScope, typeScope, variableTypeScopeReference)

                loop subCtx subFunctionIdentifierType subFunctionIdentifier parametersRest false

                CsAst.Statement.Return(CsAst.Expression.IdentifierReference(subFunctionIdentifier))
                |> subCtx.AddStatement

                let body = subCtx.GetStatements()

                let typeParameters = if first then typeScope.GetTypeParameterNames() else []

                CsAst.Statement.LocalFunction(Some subFunctionIdentifierType, identifier, typeParameters, [ parameterType, parameterIdentifier ], body)
                |> ctx.AddStatement

        loop ctx identifierType identifier parameters true

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
        let e =
            mapExpression ctx e
            |> Option.require "Expression for a return statement must have a value"

        CsAst.Statement.Return e |> ctx.AddStatement
    | Ast.Application _ ->
        let e = mapExpression ctx e

        match e with
        | Some e ->
            match e with
            | CsAst.Expression.FunctionCall(i, args) -> CsAst.Statement.FunctionCall(i, args) |> ctx.AddStatement
            | _ -> failwith "Expected a FunctionCall expression"
        | None -> ()
    | _ -> failwith "Can not create a statement out of this expression"

let private mapFunctionBody (identifierScope: IdentifierScope) (typeScope: TypeScope) (expression: Ast.Expression) : CsAst.Statement list =
    let ctx = EnclosingFunctionBodyContext(identifierScope, typeScope)

    let generateReturn = typeScope.GetExpressionType(expression) <> None

    mapStatements ctx expression generateReturn

    ctx.GetStatements()

let transpile (ast: Ast.Program, typeInformation: TypeSolver.TypeInformation) : CsAst.Program =
    let (Ast.Program e) = ast

    let globalIdentifierScope = IdentifierScope.CreateGlobalScope()

    [ BuiltIn.Identifiers.opAdd
      BuiltIn.Identifiers.opSubtract
      BuiltIn.Identifiers.opMultiply
      BuiltIn.Identifiers.opDivide
      BuiltIn.Identifiers.println
      BuiltIn.Identifiers.intToStr
      BuiltIn.Identifiers.intToStrFmt
      BuiltIn.Identifiers.floatToStr ]
    |> List.iter globalIdentifierScope.AttachIdentifier

    let globalTypeScope = TypeScope.CreateGlobalScope(typeInformation)

    let ctx = EnclosingFunctionBodyContext(globalIdentifierScope, globalTypeScope)

    mapStatements ctx e false

    let statements = ctx.GetStatements()

    CsAst.Program statements
