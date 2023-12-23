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
        parentAtomTypeNameMap: (AtomTypeId * CsAst.TypeIdentifier) seq,
        parentUniqueTypeParameterNameCounter: int
    ) =

    // let variableTypeIds =
    //     match typeScopeReference with
    //     | Some typeScopeReference -> typeInformation.typeScopes[typeScopeReference] |> List.ofSeq
    //     | None -> []

    let atomTypeNameSet = HashSet<CsAst.TypeIdentifier>()
    let atomTypeNameMap = Dictionary<AtomTypeId, CsAst.TypeIdentifier>()

    let mutable uniqueTypeParameterNameCounter = parentUniqueTypeParameterNameCounter

    do
        for k, v in parentAtomTypeNameMap do
            atomTypeNameSet.Add(v) |> ignore
            atomTypeNameMap.Add(k, v)


    let createUniqueTypeParameterIdentifier (): CsAst.TypeIdentifier =
        let createName () =
            uniqueTypeParameterNameCounter <- uniqueTypeParameterNameCounter + 1
            "T" + string uniqueTypeParameterNameCounter

        let mutable name = createName ()

        while not (atomTypeNameSet.Add(name)) do
            name <- createName ()

        name

    member this.MapTypeToCsType(t: Type) : CsAst.TypeIdentifier list * CsAst.Type option =
        match t with
        | Is BuiltIn.Types.unit -> [], None
        | AtomType atomTypeId ->
            let typeIdentifier = this.MapAtomTypeIdToCsTypeIdentifier(atomTypeId)
            [], CsAst.Type.AtomType typeIdentifier |> Some
        | FunctionType(parameterType, resultType) ->
            let parameterTypeParameters, parameterType = this.MapTypeToCsType(parameterType)
            if parameterTypeParameters.Length <> 0 then
                failwith "Function parameter cannot be a qualified type"

            let resultTypeParameters, resultType = this.MapTypeToCsType(resultType)
            if resultTypeParameters.Length <> 0 then
                failwith "Function result cannot be a qualified type"

            let parameterTypes =
                match parameterType with
                | Some p -> [ p ]
                | None -> []

            [], CsAst.Type.FunctionDelegate(parameterTypes, resultType) |> Some
        | QualifiedType (typeParameters, typeBody) ->
            let bodyTypeParameters, typeBody = this.MapTypeToCsType(typeBody)
            if bodyTypeParameters.Length <> 0 then
                failwith "Qualified type cannot appear inside another qualified type"

            match typeBody with
            | None
            | Some (CsAst.Type.AtomType _) -> failwith "The type cannot have type parameters"
            | Some (CsAst.Type.FunctionDelegate (parameters, result)) ->
                let typeParameters = typeParameters |> List.map this.MapAtomTypeIdToCsTypeIdentifier
                typeParameters, Some (CsAst.Type.FunctionDelegate (parameters, result))

    member this.MapAtomTypeIdToCsTypeIdentifier(atomTypeId: AtomTypeId): CsAst.TypeIdentifier =
        match atomTypeNameMap.TryGetValue(atomTypeId) with
        | true, typeIdentifier -> typeIdentifier
        | false, _ ->
            let typeIdentifier = createUniqueTypeParameterIdentifier ()
            atomTypeNameMap[atomTypeId] <- typeIdentifier
            typeIdentifier

    static member CreateGlobalScope(typeInformation) =
        let atomTypeNameMap =
            [
                BuiltIn.AtomTypeIds.int, "global::System.Int32"
                BuiltIn.AtomTypeIds.float, "global::System.Single"
                BuiltIn.AtomTypeIds.string, "global::System.String"
            ]

        TypeScope(typeInformation, atomTypeNameMap, 0)

    member _.CreateSubScope() =
        let atomTypeNameMap =
            atomTypeNameMap
            |> Seq.map (fun kv -> (kv.Key, kv.Value))

        TypeScope(typeInformation, atomTypeNameMap, uniqueTypeParameterNameCounter)

    member this.GetExpressionType(expression: Ast.Expression) : CsAst.TypeIdentifier list * CsAst.Type option =
        typeInformation.typeReferenceTypes[expression.expressionType]
        |> this.MapTypeToCsType

    member this.GetIdentifierType(identifier: Identifier) : CsAst.TypeIdentifier list * CsAst.Type =
        let typeParameters, typeBody =
            typeInformation.identifierTypes[identifier]
            |> this.MapTypeToCsType
        typeParameters, typeBody |> Option.get

    // member this.GetTypeParameterNames() : CsAst.TypeIdentifier list =
    //     variableTypeIds |> Seq.map this.MapVariableType |> List.ofSeq

type private EnclosingFunctionBodyContext(identifierScope: IdentifierScope, typeScope: TypeScope) =
    let statements = List()

    static member CreateFromParent(identifierScope: IdentifierScope, typeScope: TypeScope) =
        EnclosingFunctionBodyContext(identifierScope.CreateSubScope(), typeScope.CreateSubScope())

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
        let _, type_ = ctx.TypeScope.GetExpressionType(e)
        type_ |> Option.map (fun t -> CsAst.NumberLiteral(i, f, t))
    | Ast.StringLiteral s -> CsAst.Expression.StringLiteral s |> Some
    | Ast.Application(_, f, arg) ->
        match f.expressionShape with
        | Ast.Application(_, { expressionShape = Ast.IdentifierReference(BinaryOp op) }, leftOp) ->
            let leftOp = mapExpression ctx leftOp
            let rightOp = mapExpression ctx arg

            match leftOp, rightOp with
            | Some e1, Some e2 -> CsAst.Expression.BinaryOperation(e1, op, e2) |> Some
            | _ -> failwith "Operands of a binary operation should not be statements"
        | Ast.IdentifierReference i ->
            let identifier = ctx.IdentifierScope.MapIdentifier i

            let arg = mapExpression ctx arg
            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            CsAst.FunctionCall(identifier, [], args) |> Some
        | _ ->
            let typeParameters, type_ = ctx.TypeScope.GetExpressionType(f)

            if typeParameters.Length <> 0 then
                failwith "TODO"

            let varT = type_ |> Option.require "Function expression must have a type"

            let varId = ctx.IdentifierScope.CreateIdentifier()

            let varBody = mapExpression ctx f |> Option.require "Function expression must have a value"

            ctx.AddStatement(CsAst.Statement.Var(varT, varId, varBody))

            let arg = mapExpression ctx arg
            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            CsAst.FunctionCall(varId, [], args) |> Some
    | Ast.Binding(identifier, [], body) ->
        let typeParameters, bodyType = ctx.TypeScope.GetExpressionType(body)

        if typeParameters.Length <> 0 then
            failwith "Parameterless binding cannot have type parameters"

        let bodyType = bodyType |> Option.require "Parameterless binding must have a body type"

        let body = mapExpression ctx body

        match body with
        | Some body -> ctx.AddStatement(CsAst.Statement.Var(bodyType, ctx.IdentifierScope.MapIdentifier(identifier), body))
        | None -> ()

        None
    | Ast.Binding(identifier, parameters, body) ->
        let identifierTypeParameters, identifierType = ctx.TypeScope.GetIdentifierType(identifier)
        let identifier = ctx.IdentifierScope.MapIdentifier(identifier)

        let rec loop (ctx: EnclosingFunctionBodyContext) identifierType identifier parameters first =
            match parameters with
            | [] -> failwith "Impossible state"
            | [ parameterIdentifier ] ->
                let returnType =
                    match identifierType with
                    | CsAst.Type.FunctionDelegate(_, resultType) -> resultType
                    | _ -> failwith "Identifier type must be a function"

                let identifierScope = ctx.IdentifierScope.CreateSubScope()
                let typeScope = ctx.TypeScope.CreateSubScope()

                let body = mapFunctionBody identifierScope typeScope body

                let parameterTypeParameters, parameterType = ctx.TypeScope.GetIdentifierType(parameterIdentifier)
                if parameterTypeParameters.Length <> 0 then
                    failwith "Parameter type cannot be a qualified type"

                let parameterIdentifier = ctx.IdentifierScope.MapIdentifier(parameterIdentifier)

                let typeParameters = if first then identifierTypeParameters else []

                CsAst.Statement.LocalFunction(returnType, identifier, typeParameters, [ parameterType, parameterIdentifier ], body)
                |> ctx.AddStatement
            | parameterIdentifier :: parametersRest ->
                let parameterTypeParameters, parameterType = ctx.TypeScope.GetIdentifierType(parameterIdentifier)
                if parameterTypeParameters.Length <> 0 then
                    failwith "Parameter type cannot be a qualified type"

                let parameterIdentifier = ctx.IdentifierScope.MapIdentifier(parameterIdentifier)

                let subFunctionIdentifier = ctx.IdentifierScope.CreateIdentifier()

                let subFunctionIdentifierType =
                    match identifierType with
                    | CsAst.Type.FunctionDelegate(_, resultType) ->
                        resultType |> Option.require "Identifier type must be a function that returns a function"
                    | _ -> failwith "Identifier type must be a function"

                let identifierScope = ctx.IdentifierScope.CreateSubScope()
                let typeScope = ctx.TypeScope.CreateSubScope()

                let subCtx = EnclosingFunctionBodyContext.CreateFromParent(identifierScope, typeScope)

                loop subCtx subFunctionIdentifierType subFunctionIdentifier parametersRest false

                CsAst.Statement.Return(CsAst.Expression.IdentifierReference(subFunctionIdentifier))
                |> subCtx.AddStatement

                let body = subCtx.GetStatements()

                let typeParameters = if first then identifierTypeParameters else []

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
            | CsAst.Expression.FunctionCall(i, tArgs, args) -> CsAst.Statement.FunctionCall(i, tArgs, args) |> ctx.AddStatement
            | _ -> failwith "Expected a FunctionCall expression"
        | None -> ()
    | _ -> failwith "Can not create a statement out of this expression"

let private mapFunctionBody (identifierScope: IdentifierScope) (typeScope: TypeScope) (expression: Ast.Expression) : CsAst.Statement list =
    let ctx = EnclosingFunctionBodyContext(identifierScope, typeScope)

    let generateReturn =
        match typeScope.GetExpressionType(expression) with
        | _, Some _ -> true
        | _, None -> false

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
