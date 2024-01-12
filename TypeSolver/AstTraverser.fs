module internal rec TypeSolver.AstTraverser

open System.Collections.Generic
open Common
open Ast

type private Context(identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph, scopeTree: ScopeTree, globalScope: ScopeId, globalScopeAtomTypes: TypeReference list) =
    let scopeStack = Stack()
    let monomorphicTypesByScope = Dictionary()

    do
        scopeStack.Push(globalScope)
        monomorphicTypesByScope.Add(globalScope, List(globalScopeAtomTypes))
        for t in globalScopeAtomTypes do
            // scopeTree.DefinedInCurrentScope(t)
            graph.Monomorphic(globalScope, t)

    member _.GetIdentifierType(i: Identifier) =
        match identifierTypes.TryGetValue(i) with
        | true, tr -> tr
        | false, _ ->
            let tr = TypeReference($"id({i})")
            identifierTypes.Add(i, tr)
            tr

    member _.PushScope(identifier: Identifier) =
        // scopeTree.Push(identifier)
        let newScope = ScopeId()
        let monomorphicTypes = List()
        for t in monomorphicTypesByScope[scopeStack.Peek()] do
            monomorphicTypes.Add(t)
            graph.Monomorphic(newScope, t)

        scopeStack.Push(newScope)
        monomorphicTypesByScope.Add(newScope, monomorphicTypes)

    member _.PopScope() =
        // scopeTree.Pop()
        scopeStack.Pop() |> ignore

    member _.Identical(a: TypeReference, b: TypeReference) =
        graph.Identical(a, b)

    member this.IdentifierReference(identifier: Identifier, expressionType: TypeReference) =
        let identifierType = this.GetIdentifierType(identifier)
        // graph.Assignable(scopeStack.Peek(), expressionType, identifierType)
        graph.Identical(identifierType, expressionType)

    member this.DefinedInCurrentScope(typeReference: TypeReference) =
        graph.DefinedInScope(scopeStack.Peek(), typeReference)
    //     scopeTree.DefinedInCurrentScope(typeReference)

    member this.MonomorphicInCurrentScope(typeReference: TypeReference) =
        graph.Monomorphic(scopeStack.Peek(), typeReference)

    member _.Function(fnType: TypeReference, parameterType: TypeReference, resultType: TypeReference) =
        graph.Function(fnType, parameterType, resultType)

    member _.Application(appId: ApplicationId, fn: TypeReference, argument: TypeReference, result: TypeReference) =
        graph.Application(scopeStack.Peek(), appId, fn, argument, result)

let private traverseExpression (ctx: Context) (expression: Expression) =
    match expression.expressionShape with
    | IdentifierReference identifier ->
        ctx.IdentifierReference(identifier, expression.expressionType)
    | NumberLiteral(_, fractionalPart) ->
        let numberType =
            match fractionalPart with
            | Some _ -> BuiltIn.AtomTypeReferences.float
            | None -> BuiltIn.AtomTypeReferences.int

        ctx.Identical(expression.expressionType, numberType)
    | StringLiteral _ -> ctx.Identical(expression.expressionType, BuiltIn.AtomTypeReferences.string)
    | Application(appId, fn, argument) ->
        ctx.Application(appId, fn.expressionType, argument.expressionType, expression.expressionType)
        traverseExpression ctx fn
        traverseExpression ctx argument
    | Binding(identifier, parameters, body) ->
        let identifierType = ctx.GetIdentifierType(identifier)

        ctx.DefinedInCurrentScope(identifierType)

        if parameters.Length = 0 then
            // ctx.MonomorphicInCurrentScope(body.expressionType)
            ctx.Identical(identifierType, body.expressionType)
        else
            ctx.PushScope(identifier)

            let rec loop currentFunctionType parameters =
                match parameters with
                | [ parameter ] ->
                    let parameterType = ctx.GetIdentifierType(parameter)

                    ctx.DefinedInCurrentScope(parameterType)
                    ctx.MonomorphicInCurrentScope(parameterType)
                    ctx.Function(currentFunctionType, parameterType, body.expressionType)
                | parameter :: parametersRest ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    let subFunctionType =
                        TypeReference($"binding sub-function of ({identifier}) on parameter ({parameter})")

                    ctx.DefinedInCurrentScope(parameterType)
                    ctx.MonomorphicInCurrentScope(parameterType)
                    ctx.Function(currentFunctionType, parameterType, subFunctionType)
                    loop subFunctionType parametersRest
                | _ -> failwith "Invalid state"

            loop identifierType parameters

        ctx.Identical(expression.expressionType, BuiltIn.AtomTypeReferences.unit)

        traverseExpression ctx body

        if parameters.Length > 0 then
            ctx.PopScope()

    | Sequence expressions ->
        expressions |> List.iter (traverseExpression ctx)

        match List.tryLast expressions with
        | Some lastExpression -> ctx.Identical(expression.expressionType, lastExpression.expressionType)
        | None -> ctx.Identical(expression.expressionType, BuiltIn.AtomTypeReferences.unit)

    | InvalidToken _ -> failwith "TODO"

let private traverseProgram (ctx: Context) (program: Program) =
    match program with
    | Program expression -> traverseExpression ctx expression

type TypeReferenceScope =
    { containedTypeReferences: TypeReference list
      childScopes: Map<TypeReference, TypeReferenceScope> }

let collectInfoFromAst (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph, scopeTree: ScopeTree, globalScope: ScopeId, globalScopeAtomTypes: TypeReference list) (ast: Program) =
    let ctx = Context(identifierTypes, graph, scopeTree, globalScope, globalScopeAtomTypes)

    traverseProgram ctx ast
