module internal rec TypeSolver.AstTraverser

open System
open System.Collections.Generic
open Common
open Ast

type private Context(identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph, scopeTree: ScopeTree) =
    let globalScope = Guid.NewGuid()
    let scopeStack = Stack([ globalScope ])

    do Console.WriteLine(graph.ToString())

    member _.GetIdentifierType(i: Identifier) =
        match identifierTypes.TryGetValue(i) with
        | true, tr -> tr
        | false, _ ->
            let tr = TypeReference($"id({i})")
            identifierTypes.Add(i, tr)
            tr

    member _.PushScope(identifier: Identifier) =
        scopeTree.Push(identifier)
        scopeStack.Push(Guid.NewGuid())

    member _.PopScope() =
        scopeTree.Pop()
        scopeStack.Pop() |> ignore

    member _.Identical(a: TypeReference, b: TypeReference) =
        graph.Identical(a, b)

    member this.IdentifierReference(identifier: Identifier, expressionType: TypeReference) =
        let identifierType = this.GetIdentifierType(identifier)
        graph.Assignable(scopeStack.Peek(), expressionType, identifierType)

    member _.Atom(typeReference: TypeReference, atomTypeId: AtomTypeId) =
        graph.Atom(typeReference, atomTypeId)

    member this.DefinedInCurrentScope(identifier: Identifier) =
        let typeReference = this.GetIdentifierType(identifier)
        scopeTree.Add(typeReference)
        graph.NonGeneralizable(scopeStack.Peek(), typeReference)

    member _.Function(fnType: TypeReference, parameterType: TypeReference, resultType: TypeReference) =
        graph.Function(fnType, parameterType, resultType)

let private traverseExpression (ctx: Context) (expression: Expression) =
    match expression.expressionShape with
    | IdentifierReference identifier ->
        ctx.IdentifierReference(identifier, expression.expressionType)
    | NumberLiteral(_, fractionalPart) ->
        let numberType =
            match fractionalPart with
            | Some _ -> BuiltIn.AtomTypeIds.float
            | None -> BuiltIn.AtomTypeIds.int

        ctx.Atom(expression.expressionType, numberType)
    | StringLiteral _ -> ctx.Atom(expression.expressionType, BuiltIn.AtomTypeIds.string)
    | Application(_, fn, argument) ->
        ctx.Function(fn.expressionType, argument.expressionType, expression.expressionType)
        traverseExpression ctx fn
        traverseExpression ctx argument
    | Binding(identifier, parameters, body) ->
        ctx.DefinedInCurrentScope(identifier)

        let identifierType = ctx.GetIdentifierType(identifier)

        if parameters.Length = 0 then
            ctx.Identical(identifierType, body.expressionType)
        else
            ctx.PushScope(identifier)

            let rec loop currentFunctionType parameters =
                match parameters with
                | [ parameter ] ->
                    ctx.DefinedInCurrentScope(parameter)

                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.Function(currentFunctionType, parameterType, body.expressionType)
                | parameter :: parametersRest ->
                    ctx.DefinedInCurrentScope(parameter)

                    let parameterType = ctx.GetIdentifierType(parameter)
                    let subFunctionType =
                        TypeReference($"binding sub-function of ({identifier}) on parameter ({parameter})")

                    ctx.Function(currentFunctionType, parameterType, subFunctionType)
                    loop subFunctionType parametersRest
                | _ -> failwith "Invalid state"

            loop identifierType parameters

        ctx.Atom(expression.expressionType, BuiltIn.AtomTypeIds.unit)

        traverseExpression ctx body

        if parameters.Length > 0 then
            ctx.PopScope()

    | Sequence expressions ->
        expressions |> List.iter (traverseExpression ctx)

        match List.tryLast expressions with
        | Some lastExpression -> ctx.Identical(expression.expressionType, lastExpression.expressionType)
        | None -> ctx.Atom(expression.expressionType, BuiltIn.AtomTypeIds.unit)

    | InvalidToken _ -> failwith "TODO"

let private traverseProgram (ctx: Context) (program: Program) =
    match program with
    | Program expression -> traverseExpression ctx expression

type TypeReferenceScope =
    { containedTypeReferences: TypeReference list
      childScopes: Map<TypeReference, TypeReferenceScope> }

let collectInfoFromAst (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph, scopeTree: ScopeTree) (ast: Program) =
    let ctx = Context(identifierTypes, graph, scopeTree)

    traverseProgram ctx ast
