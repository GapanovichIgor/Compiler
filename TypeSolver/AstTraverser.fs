﻿module internal rec TypeSolver.AstTraverser

open System.Collections.Generic
open Common
open Ast

type private Context
    (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph, scopeTree: ScopeTree, functionApplications: List<FunctionApplication>) =

    do System.Console.WriteLine(graph.ToString())

    member _.GetIdentifierType(i: Identifier) =
        match identifierTypes.TryGetValue(i) with
        | true, tr -> tr
        | false, _ ->
            let tr = TypeReference($"identifier '{i}'")
            identifierTypes.Add(i, tr)
            tr

    member _.PushScope(identifier: Identifier) = scopeTree.Push(identifier)

    member _.PopScope() = scopeTree.Pop()

    member _.Identical(a: TypeReference, b: TypeReference) =
        graph.Identical(a, b)
        System.Console.WriteLine($"Identical {a} {b}")
        System.Console.WriteLine(graph.ToString())

    member _.Atom(typeReference: TypeReference, atomTypeId: AtomTypeId) =
        graph.Atom(typeReference, atomTypeId)
        System.Console.WriteLine($"Atom {typeReference} {atomTypeId}")
        System.Console.WriteLine(graph.ToString())

    member _.AddToScope(typeReference: TypeReference) = scopeTree.Add(typeReference)

    member _.FunctionDefinition(fnType: TypeReference, parameterType: TypeReference, resultType: TypeReference) =
        graph.Function(fnType, parameterType, resultType)
        System.Console.WriteLine($"FunctionDefinition {fnType} : {parameterType} -> {resultType}")
        System.Console.WriteLine(graph.ToString())

    member _.Application(applicationReference: ApplicationReference, fnType: TypeReference, argumentType: TypeReference, resultType: TypeReference) =
        let fnInstanceType = TypeReference($"instance of {fnType}")

        graph.Instance(fnType, fnInstanceType)

        functionApplications.Add
            { applicationReference = applicationReference
              definedFunctionType = fnType
              resultFunctionType = fnInstanceType }

        graph.Function(fnInstanceType, argumentType, resultType)

        System.Console.WriteLine($"Application {fnType} : {argumentType} -> {resultType}")
        System.Console.WriteLine(graph.ToString())

    member _.NonGeneralizable(typeReference: TypeReference) =
        graph.NonGeneralizable(typeReference)
        System.Console.WriteLine($"NonGeneralizable {typeReference}")
        System.Console.WriteLine(graph.ToString())

let private traverseExpression (ctx: Context) (expression: Expression) =
    match expression.expressionShape with
    | IdentifierReference identifier ->
        let identifierTypeRef = ctx.GetIdentifierType(identifier)
        ctx.Identical(expression.expressionType, identifierTypeRef)
    | NumberLiteral(_, fractionalPart) ->
        let numberType =
            match fractionalPart with
            | Some _ -> BuiltIn.AtomTypeIds.float
            | None -> BuiltIn.AtomTypeIds.int

        ctx.Atom(expression.expressionType, numberType)
    | StringLiteral _ -> ctx.Atom(expression.expressionType, BuiltIn.AtomTypeIds.string)
    | Application(applicationReference, fn, argument) ->
        ctx.Application(applicationReference, fn.expressionType, argument.expressionType, expression.expressionType)
        traverseExpression ctx fn
        traverseExpression ctx argument
    | Binding(identifier, parameters, body) ->
        let identifierType = ctx.GetIdentifierType(identifier)

        ctx.AddToScope(identifierType)

        if parameters.Length = 0 then
            ctx.Identical(identifierType, body.expressionType)
        else
            ctx.PushScope(identifier)

            let rec loop currentFunctionType parameters =
                match parameters with
                | [ parameter ] ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.AddToScope(parameterType)
                    ctx.NonGeneralizable(parameterType)
                    ctx.FunctionDefinition(currentFunctionType, parameterType, body.expressionType)
                | parameter :: parametersRest ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.AddToScope(parameterType)
                    ctx.NonGeneralizable(parameterType)

                    let subFunctionType =
                        TypeReference($"binding sub-function of ({identifier}) on parameter ({parameter})")

                    ctx.FunctionDefinition(currentFunctionType, parameterType, subFunctionType)
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

let collectInfoFromAst
    (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph, scopeTree: ScopeTree, functionApplications: List<FunctionApplication>)
    (ast: Program)
    =

    let ctx = Context(identifierTypes, graph, scopeTree, functionApplications)

    traverseProgram ctx ast
