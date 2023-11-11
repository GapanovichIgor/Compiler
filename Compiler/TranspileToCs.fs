module internal rec Compiler.TranspileToCs

open System
open System.Collections.Generic
open Compiler.Type

type private TypedExpression = Ast.TypedExpression<Type>

type private EnclosingFunctionBodyContext =
    { addPrecedingStatement: CsAst.Statement -> unit
      createIdentifier: unit -> CsAst.Identifier
      mapIdentifier: Ast.Identifier -> CsAst.Identifier }

let private createEnclosingFunctionBodyContext (statements: List<CsAst.Statement>) =
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
      mapIdentifier = mapIdentifier }

let private mapBinaryOperator (o: Ast.BinaryOperator) =
    match o with
    | Ast.BinaryOperator.Add -> CsAst.BinaryOperator.Add
    | Ast.BinaryOperator.Subtract -> CsAst.BinaryOperator.Subtract
    | Ast.BinaryOperator.Multiply -> CsAst.BinaryOperator.Multiply
    | Ast.BinaryOperator.Divide -> CsAst.BinaryOperator.Divide

let private (|Is|_|) v1 v2 = if v1 = v2 then Some() else None

let private mapType (t: Type) : CsAst.Type =
    match t with
    | Is BuiltIn.Types.int -> CsAst.ValueType "System.Int32"
    | Is BuiltIn.Types.float -> CsAst.ValueType "System.Single"
    | Is BuiltIn.Types.string -> CsAst.ValueType "System.String"
    | Is BuiltIn.Types.unit -> CsAst.Void
    | FunctionType (argT, resultT) ->
        let argT = mapType argT
        let resultT = mapType resultT
        CsAst.FunctionType ([argT], resultT)
    | _ -> failwith "TODO handle other types"

let private mapExpression (ctx: EnclosingFunctionBodyContext) (e: TypedExpression) : CsAst.Expression option =
    match e.expression with
    | Ast.Identifier i -> CsAst.Identifier (ctx.mapIdentifier i) |> Some
    | Ast.NumberLiteral(i, f) ->
        let t = mapType e.expressionType
        CsAst.NumberLiteral(i, f, t) |> Some
    | Ast.StringLiteral s -> CsAst.StringLiteral s |> Some
    | Ast.BinaryOperation(e1, op, e2) ->
        let e1 = mapExpression ctx e1
        let op = mapBinaryOperator op
        let e2 = mapExpression ctx e2

        match e1, e2 with
        | Some e1, Some e2 -> CsAst.BinaryOperation(e1, op, e2) |> Some
        | _ -> failwith "Operands of a binary operation should not be statements"
    | Ast.Application(f, arg) ->
        match f.expression with
        | Ast.Identifier i ->
            let arg = mapExpression ctx arg

            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            CsAst.FunctionCall(ctx.mapIdentifier i, args) |> Some
        | _ ->
            let varT = mapType f.expressionType
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
    | Ast.Coerce(e, t) ->
        let e = mapExpression ctx e
        let t = mapType t

        match e with
        | Some e -> CsAst.Cast(e, t) |> Some
        | None -> failwith "Coerce on a statement"
    | Ast.Binding(i, v) ->
        let t = mapType v.expressionType
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

let private mapStatement (ctx: EnclosingFunctionBodyContext) (e: TypedExpression) : CsAst.Statement list =
    match e.expression with
    | Ast.Binding(i, v) ->
        let t = mapType v.expressionType
        let v = mapExpression ctx v

        match v with
        | Some v -> [ CsAst.Statement.Var(t, ctx.mapIdentifier i, v) ]
        | None -> failwith "Let binding has statement as its value"
    | Ast.Application(f, arg) ->
        match f.expression with
        | Ast.Identifier i ->
            let arg = mapExpression ctx arg

            let args =
                match arg with
                | Some arg -> [ arg ]
                | None -> []

            [ CsAst.Statement.FunctionCall(ctx.mapIdentifier i, args) ]
        | _ -> failwith "TODO handle case when function is not represented by an identifier"
    | Ast.Sequence es -> es |> List.collect (mapStatement ctx)
    | _ -> failwith "TODO handle other statements"

let private mapFunctionBody (e: TypedExpression) : CsAst.Statement list =
    let statements = List()

    let context = createEnclosingFunctionBodyContext statements

    match e.expression with
    | Ast.Sequence es ->
        for e in es do
            statements.AddRange(mapStatement context e)
    | _ -> statements.AddRange(mapStatement context e)

    List.ofSeq statements

let transpile (ast: Ast.Program<Type>) : CsAst.Program =
    let (Ast.Program e) = ast

    let statements = mapFunctionBody e

    CsAst.Program statements
