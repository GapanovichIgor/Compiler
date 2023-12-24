module internal rec TypeSolver.AstTraverser

open System.Collections.Generic
open Common
open Ast

type private TypeScopeMut =
    { containedTypeReferences: List<TypeReference>
      childScopes: Dictionary<TypeReference, TypeScopeMut> }

type private Context =
    { identifierTypes: Dictionary<Identifier, TypeReference>
      graph: TypeGraph
      scopeOwnerStack: Stack<TypeReference>
      functionApplications: List<FunctionApplication> }

    member this.GetIdentifierType(i: Identifier) =
        match this.identifierTypes.TryGetValue(i) with
        | true, tr -> tr
        | false, _ ->
            let tr = TypeReference($"identifier '{i}'")
            this.identifierTypes.Add(i, tr)
            tr

    member this.PushScope(owner: TypeReference) = this.scopeOwnerStack.Push(owner)

    member this.PopScope() = this.scopeOwnerStack.Pop() |> ignore

    member this.AddToScope(typeReference: TypeReference) =
        if this.scopeOwnerStack.Count > 0 then
            this.graph.Scoped(this.scopeOwnerStack.Peek(), typeReference)

    member this.Application(applicationReference: ApplicationReference, fnType: TypeReference, argumentType: TypeReference, resultType: TypeReference) =
        let fnInstanceType = TypeReference($"instance of {fnType}")

        this.graph.Instance(fnType, fnInstanceType)

        this.functionApplications.Add
            { applicationReference = applicationReference
              definedFunctionType = fnType
              resultFunctionType = fnInstanceType }

        this.graph.Function(fnInstanceType, argumentType, resultType)

    member this.NonGeneralizable(typeReference: TypeReference) =
        this.graph.NonGeneralizable(typeReference)

let private traverseExpression (ctx: Context) (expression: Expression) =
    match expression.expressionShape with
    | IdentifierReference identifier ->
        let identifierTypeRef = ctx.GetIdentifierType(identifier)
        ctx.graph.Identical(expression.expressionType, identifierTypeRef)
    | NumberLiteral(_, fractionalPart) ->
        let numberType =
            match fractionalPart with
            | Some _ -> BuiltIn.AtomTypeIds.float
            | None -> BuiltIn.AtomTypeIds.int

        ctx.graph.Atom(expression.expressionType, numberType)
    | StringLiteral _ -> ctx.graph.Atom(expression.expressionType, BuiltIn.AtomTypeIds.string)
    | Application(applicationReference, fn, argument) ->
        ctx.Application(applicationReference, fn.expressionType, argument.expressionType, expression.expressionType)
        traverseExpression ctx fn
        traverseExpression ctx argument
    | Binding(identifier, parameters, body) ->
        let identifierType = ctx.GetIdentifierType(identifier)

        ctx.AddToScope(identifierType)

        if parameters.Length = 0 then
            ctx.graph.Identical(identifierType, body.expressionType)
        else
            ctx.PushScope(identifierType)

            let rec loop currentFunctionType parameters =
                match parameters with
                | [ parameter ] ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.AddToScope(parameterType)
                    ctx.NonGeneralizable(parameterType)
                    ctx.graph.Function(currentFunctionType, parameterType, body.expressionType)
                | parameter :: parametersRest ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.AddToScope(parameterType)
                    ctx.NonGeneralizable(parameterType)

                    let subFunctionType =
                        TypeReference($"binding sub-function of ({identifier}) on parameter ({parameter})")

                    ctx.graph.Function(currentFunctionType, parameterType, subFunctionType)
                    loop subFunctionType parametersRest
                | _ -> failwith "Invalid state"

            loop identifierType parameters

        ctx.graph.Atom(expression.expressionType, BuiltIn.AtomTypeIds.unit)

        traverseExpression ctx body

        if parameters.Length > 0 then
            ctx.PopScope()

    | Sequence expressions ->
        expressions |> List.iter (traverseExpression ctx)

        match List.tryLast expressions with
        | Some lastExpression -> ctx.graph.Identical(expression.expressionType, lastExpression.expressionType)
        | None -> ctx.graph.Atom(expression.expressionType, BuiltIn.AtomTypeIds.unit)

    | InvalidToken _ -> failwith "TODO"

let private traverseProgram (ctx: Context) (program: Program) =
    match program with
    | Program expression -> traverseExpression ctx expression

type TypeReferenceScope =
    { containedTypeReferences: TypeReference list
      childScopes: Map<TypeReference, TypeReferenceScope> }

type FunctionApplication =
    { applicationReference: ApplicationReference
      definedFunctionType: TypeReference
      resultFunctionType: TypeReference }

type AstTypeInfo =
    { functionApplications: FunctionApplication list }

let collectInfoFromAst (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph) (ast: Program) : AstTypeInfo =
    let ctx =
        { identifierTypes = identifierTypes
          graph = graph
          scopeOwnerStack = Stack []
          functionApplications = List() }

    traverseProgram ctx ast

    if ctx.scopeOwnerStack.Count <> 0 then
        failwith "Scope stack imbalance"

    { functionApplications = ctx.functionApplications |> List.ofSeq }
