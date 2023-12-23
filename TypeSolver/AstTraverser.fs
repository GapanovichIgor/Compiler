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

    member this.PushScope(owner: TypeReference) =
        // let newScope: TypeScopeMut =
        //     { containedTypeReferences = List()
        //       childScopes = Dictionary() }
        //
        // this.scopeOwnerStack.Peek().childScopes.Add(owner, newScope)
        // this.scopeOwnerStack.Push(newScope)
        this.scopeOwnerStack.Push(owner)

    member this.PopScope() = this.scopeOwnerStack.Pop() |> ignore

    member this.AddToScope(typeReference: TypeReference) =
        // this.scopeOwnerStack.Peek().containedTypeReferences.Add(typeReference)
        if this.scopeOwnerStack.Count > 0 then
            this.graph.Scoped(this.scopeOwnerStack.Peek(), typeReference)

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
        let fnInstanceType = TypeReference($"instance of {fn.expressionType}")
        ctx.graph.Instance(fn.expressionType, fnInstanceType)

        ctx.functionApplications.Add
            { applicationReference = applicationReference
              definedFunctionType = fn.expressionType
              resultFunctionType = fnInstanceType }

        ctx.graph.Function(fnInstanceType, argument.expressionType, expression.expressionType)

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
                    ctx.graph.Function(currentFunctionType, parameterType, body.expressionType)
                | parameter :: parametersRest ->
                    let parameterType = ctx.GetIdentifierType(parameter)
                    ctx.AddToScope(parameterType)

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
    { (*rootScope: TypeReferenceScope*)
      functionApplications: FunctionApplication list }

let collectInfoFromAst (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph) (ast: Program) : AstTypeInfo =
    // let globalScope: TypeScopeMut =
    //     { containedTypeReferences = List()
    //       childScopes = Dictionary() }

    let ctx =
        { identifierTypes = identifierTypes
          graph = graph
          scopeOwnerStack = Stack [ ]
          functionApplications = List() }

    traverseProgram ctx ast

    if ctx.scopeOwnerStack.Count <> 0 then
        failwith "Scope stack imbalance"

    // let rootScope = ctx.scopeOwnerStack.Pop()
    //
    // let rec convertScope (s: TypeScopeMut) =
    //     { containedTypeReferences = s.containedTypeReferences |> List.ofSeq
    //       childScopes = s.childScopes |> Seq.map (fun kv -> kv.Key, convertScope kv.Value) |> Map.ofSeq }

    // let rootScope = convertScope rootScope

    { (*rootScope = rootScope*)
      functionApplications = ctx.functionApplications |> List.ofSeq }
