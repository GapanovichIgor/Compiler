module internal rec Compiler.TranspileToCs

open System.Collections.Generic

type private EnclosingFunctionBodyContext =
    { addVar: CsAst.Type -> CsAst.Identifier -> CsAst.Expression -> unit }

let private mapBinaryOperator (o: Ast.BinaryOperator) =
    match o with
    | Ast.BinaryOperator.Add -> CsAst.BinaryOperator.Add
    | Ast.BinaryOperator.Subtract -> CsAst.BinaryOperator.Subtract
    | Ast.BinaryOperator.Multiply -> CsAst.BinaryOperator.Multiply
    | Ast.BinaryOperator.Divide -> CsAst.BinaryOperator.Divide

let private (|Is|_|) v1 v2 =
    if v1 = v2
    then Some ()
    else None

let private mapType (t: Ast.Type): CsAst.Type =
    match t with
    | Is Ast.BuiltInTypes.int -> "System.Int32"
    | Is Ast.BuiltInTypes.float -> "System.Single"
    | Is Ast.BuiltInTypes.string -> "System.String"
    | Is Ast.BuiltInTypes.unit -> failwith "TODO handle void"
    | _ -> failwith "TODO handle other types"

let private mapExpression (ctx: EnclosingFunctionBodyContext) (e: Ast.TypedExpression): CsAst.Expression option =
    match e.expression with
    | Ast.Identifier i -> CsAst.Identifier i |> Some
    | Ast.IntegerLiteral i -> CsAst.IntegerLiteral i |> Some
    | Ast.FloatLiteral (i, f) -> CsAst.FloatLiteral (i, f) |> Some
    | Ast.StringLiteral s -> CsAst.StringLiteral s |> Some
    | Ast.BinaryOperation (e1, op, e2) ->
        let e1 = mapExpression ctx e1
        let op = mapBinaryOperator op
        let e2 = mapExpression ctx e2
        match e1, e2 with
        | Some e1, Some e2 ->
            CsAst.BinaryOperation (e1, op, e2)
            |> Some
        | _ -> failwith "Operands of a binary operation should not be statements"
    | Ast.Application (f, arg) ->
        match f.expression with
        | Ast.Identifier i ->
            let arg = mapExpression ctx arg
            let args =
                match arg with
                | Some arg -> [arg]
                | None -> []

            CsAst.FunctionCall (i, args)
            |> Some
        | _ -> failwith "TODO handle case when function is not represented by an identifier"
    | Ast.Coerce (e, t) ->
        let e = mapExpression ctx e
        let t = mapType t
        match e with
        | Some e ->
            CsAst.Cast (e, t)
            |> Some
        | None -> failwith "Coerce on a statement"
    | Ast.Let (i, v) ->
        let t = mapType v.expressionType
        let v = mapExpression ctx v
        match v with
        | Some v -> ctx.addVar t i v
        | None -> ()
        None
    | Ast.Sequence es ->
        let es = es |> List.map (mapExpression ctx)
        es |> List.last

let private mapStatement (ctx: EnclosingFunctionBodyContext) (e: Ast.TypedExpression): CsAst.Statement list =
    match e.expression with
    | Ast.Let (i, v) ->
        let t = mapType v.expressionType
        let v = mapExpression ctx v
        match v with
        | Some v -> [ CsAst.Statement.Var (t, i, v) ]
        | None -> failwith "Let binding has statement as its value"
    | Ast.Application (f, arg) ->
        match f.expression with
        | Ast.Identifier i ->
            let arg = mapExpression ctx arg
            let args =
                match arg with
                | Some arg -> [arg]
                | None -> []
            [ CsAst.Statement.FunctionCall (i, args) ]
        | _ -> failwith "TODO handle case when function is not represented by an identifier"
    | Ast.Sequence es ->
        es |> List.collect (mapStatement ctx)
    | _ -> failwith "TODO handle other statements"

let private mapFunctionBody (e: Ast.TypedExpression): CsAst.Statement list =
    let statements = List()

    let context =
        { addVar = fun t i v ->
            statements.Add(CsAst.Statement.Var (t, i, v)) }

    match e.expression with
    | Ast.Sequence es ->
        for e in es do
            statements.AddRange(mapStatement context e)
    | _ ->
        statements.AddRange(mapStatement context e)

    List.ofSeq statements

let transpile (ast: Ast.Program): CsAst.Program =
    let (Ast.Program e) = ast

    let statements = mapFunctionBody e

    CsAst.Program statements