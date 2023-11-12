module internal rec Compiler.CsTranspiler

open System
open System.Collections.Generic
open Compiler.Type

type private EnclosingFunctionBodyContext =
    { addPrecedingStatement: CsAst.Statement -> unit
      createIdentifier: unit -> CsAst.Identifier
      mapIdentifier: Ast.Identifier -> CsAst.Identifier
      getCsType: Ast.TypeReference -> CsAst.Type }

let private createEnclosingFunctionBodyContext (statements: List<CsAst.Statement>, getCsType: Ast.TypeReference -> CsAst.Type) =
    let identifierNameSet = HashSet()
    let identifierNameMap = Dictionary()

    let mutable identifierNameCounter = 0

    let createUniqueIdentifierName () =
        let createId () =
            identifierNameCounter <- identifierNameCounter + 1
            "id" + string identifierNameCounter

        let mutable i = createId ()

        while not (identifierNameSet.Add(i)) do
            i <- createId ()

        i

    let createIdentifier () =
        let i = createUniqueIdentifierName ()
        identifierNameMap[Guid.NewGuid()] <- i
        i

    let mapIdentifier (identifier: Ast.Identifier) =
        match identifierNameMap.TryGetValue(identifier.identity) with
        | true, identifierName -> identifierName
        | false, _ ->
            if identifierNameSet.Add(identifier.name) then
                identifierNameMap[identifier.identity] <- identifier.name
                identifier.name
            else
                let uniqueIdentifierName = createUniqueIdentifierName ()
                identifierNameMap[identifier.identity] <- uniqueIdentifierName
                uniqueIdentifierName


    { addPrecedingStatement = statements.Add
      createIdentifier = createIdentifier
      mapIdentifier = mapIdentifier
      getCsType = getCsType }

let private (|Is|_|) v1 v2 = if v1 = v2 then Some() else None

let private mapBinaryOperator (operator: Ast.Identifier) =
    match operator with
    | Is BuiltIn.Identifiers.opAdd -> Some CsAst.BinaryOperator.Add
    | Is BuiltIn.Identifiers.opSubtract -> Some CsAst.BinaryOperator.Subtract
    | Is BuiltIn.Identifiers.opMultiply -> Some CsAst.BinaryOperator.Multiply
    | Is BuiltIn.Identifiers.opDivide -> Some CsAst.BinaryOperator.Divide
    | _ -> None

let private (|BinaryOp|_|) identifier = mapBinaryOperator identifier

let private mapToCsType (t: Type) : CsAst.Type =
    match t with
    | Is BuiltIn.Types.int -> CsAst.ValueType "System.Int32"
    | Is BuiltIn.Types.float -> CsAst.ValueType "System.Single"
    | Is BuiltIn.Types.string -> CsAst.ValueType "System.String"
    | Is BuiltIn.Types.unit -> CsAst.Void
    | FunctionType(argT, resultT) ->
        let argT = mapToCsType argT
        let resultT = mapToCsType resultT
        CsAst.FunctionType([ argT ], resultT)
    | _ -> failwith "TODO handle other types"

let private mapExpression (ctx: EnclosingFunctionBodyContext) (e: Ast.Expression) : CsAst.Expression option =
    match e.expressionShape with
    | Ast.IdentifierReference i -> CsAst.Identifier(ctx.mapIdentifier i) |> Some
    | Ast.NumberLiteral(i, f) ->
        let t = ctx.getCsType e.expressionType
        CsAst.NumberLiteral(i, f, t) |> Some
    | Ast.StringLiteral s -> CsAst.StringLiteral s |> Some
    | Ast.Application(f, arg) ->
        match f.expressionShape with
        | Ast.Application({ expressionShape = Ast.IdentifierReference(BinaryOp op) }, leftOp) ->
            let leftOp = mapExpression ctx leftOp
            let rightOp = mapExpression ctx arg

            match leftOp, rightOp with
            | Some e1, Some e2 -> CsAst.BinaryOperation(e1, op, e2) |> Some
            | _ -> failwith "Operands of a binary operation should not be statements"
        | Ast.IdentifierReference i ->
            let arg = mapExpression ctx arg

            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            CsAst.FunctionCall(ctx.mapIdentifier i, args) |> Some
        | _ ->
            let varT = ctx.getCsType f.expressionType
            let varId = ctx.createIdentifier ()
            let varBody = mapExpression ctx f

            match varBody with
            | Some varBody ->
                ctx.addPrecedingStatement (CsAst.Var(varT, varId, varBody))
                let arg = mapExpression ctx arg

                let args =
                    match arg with
                    | Some arg -> [ arg ]
                    | None -> []

                CsAst.FunctionCall(varId, args) |> Some
            | None -> failwith "Expression was expected to evaluate to a function, but it evaluated to nothing"
    | Ast.Binding(i, v) ->
        let t = ctx.getCsType v.expressionType
        let v = mapExpression ctx v

        match v with
        | Some v -> ctx.addPrecedingStatement (CsAst.Var(t, ctx.mapIdentifier i, v))
        | None -> ()

        None
    | Ast.Sequence es ->
        let mutable es = es |> List.map (mapExpression ctx)

        while es.Length > 1 do
            let eHead = es.Head

            match eHead with
            | Some(CsAst.FunctionCall(f, args)) -> ctx.addPrecedingStatement (CsAst.Statement.FunctionCall(f, args))
            | _ -> ()

            es <- es.Tail

        es.Head

let private mapStatement (ctx: EnclosingFunctionBodyContext) (e: Ast.Expression) : CsAst.Statement list =
    match e.expressionShape with
    | Ast.Binding(i, v) ->
        let t = ctx.getCsType v.expressionType
        let v = mapExpression ctx v

        match v with
        | Some v -> [ CsAst.Statement.Var(t, ctx.mapIdentifier i, v) ]
        | None -> failwith "Let binding has statement as its value"
    | Ast.Application(f, arg) ->
        match f.expressionShape with
        | Ast.IdentifierReference i ->
            let arg = mapExpression ctx arg

            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            [ CsAst.Statement.FunctionCall(ctx.mapIdentifier i, args) ]
        | _ -> failwith "TODO handle case when function is not represented by an identifier"
    | Ast.Sequence es -> es |> List.collect (mapStatement ctx)
    | _ -> failwith "TODO handle other statements"

let private mapFunctionBody (getCsType: Ast.TypeReference -> CsAst.Type) (e: Ast.Expression) : CsAst.Statement list =
    let statements = List()

    let context = createEnclosingFunctionBodyContext (statements, getCsType)

    match e.expressionShape with
    | Ast.Sequence es ->
        for e in es do
            statements.AddRange(mapStatement context e)
    | _ -> statements.AddRange(mapStatement context e)

    List.ofSeq statements

let transpile (ast: Ast.Program, typeMap: TypeSolver.TypeMap) : CsAst.Program =
    let (Ast.Program e) = ast

    let getCsType typeReference =
        typeMap |> Map.find typeReference |> mapToCsType

    let statements = mapFunctionBody getCsType e

    CsAst.Program statements
