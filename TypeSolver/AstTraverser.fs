module internal rec TypeSolver.AstTraverser

open System.Collections.Generic
open Common
open Ast

type Context =
    { identifierTypes: Dictionary<Identifier, TypeReference>
      constraints: TypeGraph
      typeScopes: Dictionary<TypeScopeReference, List<TypeReference>> }

    member this.GetIdentifierType(i: Identifier) =
        match this.identifierTypes.TryGetValue(i) with
        | true, tr -> tr
        | false, _ ->
            let tr = TypeReference($"identifier '{i}'")
            this.identifierTypes.Add(i, tr)
            tr

    member this.AddToScope(tr: TypeReference, scopeRef: TypeScopeReference) =
        match this.typeScopes.TryGetValue(scopeRef) with
        | true, scope -> scope.Add(tr)
        | false, _ ->
            let scope = List()
            scope.Add(tr)
            this.typeScopes.Add(scopeRef, scope)

let private traverseExpression (ctx: Context) (expression: Expression) =
    match expression.expressionShape with
    | IdentifierReference identifier ->
        let identifierTypeRef = ctx.GetIdentifierType(identifier)
        ctx.constraints.Identical(expression.expressionType, identifierTypeRef)
    | NumberLiteral(_, fractionalPart) ->
        let numberType =
            match fractionalPart with
            | Some _ -> BuiltIn.AtomTypeIds.float
            | None -> BuiltIn.AtomTypeIds.int

        ctx.constraints.Atom(expression.expressionType, numberType)
    | StringLiteral _ ->
        ctx.constraints.Atom(expression.expressionType, BuiltIn.AtomTypeIds.string)
    | Application(fn, argument) ->
        let fnInstance = TypeReference($"instance of {fn.expressionType}")
        ctx.constraints.Instance(fn.expressionType, fnInstance)
        ctx.constraints.Function(fnInstance, argument.expressionType, expression.expressionType)

        traverseExpression ctx fn
        traverseExpression ctx argument
    | Binding(identifier, typeScopeRef, parameters, body) ->
        let identifierType = ctx.GetIdentifierType(identifier)

        match parameters with
        | [] -> ctx.constraints.Identical(identifierType, body.expressionType)
        | _ ->
            let rec loop currentFunctionType parameters =
                match parameters with
                | [ parameter ] ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.AddToScope(parameterType, typeScopeRef)
                    ctx.constraints.Function(currentFunctionType, parameterType, body.expressionType)
                | parameter :: parametersRest ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.AddToScope(parameterType, typeScopeRef)
                    let subFunctionType = TypeReference($"binding sub-function of ({identifier}) on parameter ({parameter})")
                    ctx.constraints.Function(currentFunctionType, parameterType, subFunctionType)
                    loop subFunctionType parametersRest
                | _ -> failwith "Invalid state"

            loop identifierType parameters

        ctx.constraints.Atom(expression.expressionType, BuiltIn.AtomTypeIds.unit)

        traverseExpression ctx body

    | Sequence expressions ->
        expressions |> List.iter (traverseExpression ctx)

        match List.tryLast expressions with
        | Some lastExpression -> ctx.constraints.Identical(expression.expressionType, lastExpression.expressionType)
        | None -> ctx.constraints.Atom(expression.expressionType, BuiltIn.AtomTypeIds.unit)

    | InvalidToken _ -> failwith "TODO"

let private traverseProgram (ctx: Context) (program: Program) =
    match program with
    | Program expression -> traverseExpression ctx expression

let collectInfoFromAst (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph) (ast: Program): Map<TypeScopeReference, TypeReference list> =
    let ctx =
        { identifierTypes = identifierTypes
          constraints = graph
          typeScopes = Dictionary() }

    traverseProgram ctx ast

    ctx.typeScopes
    |> Seq.map (fun kv -> (kv.Key, kv.Value |> List.ofSeq))
    |> Map.ofSeq
