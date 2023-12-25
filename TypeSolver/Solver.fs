module TypeSolver.Solver

open System.Collections.Generic
open Ast
open Common

let private addBuiltIns (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph, scopeTree: ScopeTree) =
    let getIdentifierTypeRef identifier =
        match identifierTypes.TryGetValue(identifier) with
        | true, tr -> tr
        | false, _ ->
            let tr = TypeReference($"identifier '{identifier}'")
            identifierTypes[identifier] <- tr
            tr

    [
      //
      BuiltIn.Identifiers.println, BuiltIn.IdentifierTypes.println
      // BuiltIn.Identifiers.intToStr, BuiltIn.IdentifierTypes.intToStr
      // BuiltIn.Identifiers.intToStrFmt, BuiltIn.IdentifierTypes.intToStrFmt
      // BuiltIn.Identifiers.floatToStr, BuiltIn.IdentifierTypes.floatToStr
      // BuiltIn.Identifiers.opAdd, BuiltIn.IdentifierTypes.opAdd
      // BuiltIn.Identifiers.opSubtract, BuiltIn.IdentifierTypes.opSubtract
      // BuiltIn.Identifiers.opMultiply, BuiltIn.IdentifierTypes.opMultiply
      // BuiltIn.Identifiers.opDivide, BuiltIn.IdentifierTypes.opDivide
      //
      ]
    |> List.iter (fun (identifier, type_) ->
        let rec add typeRef type_ =
            match type_ with
            | AtomType atomTypeId -> graph.Atom(typeRef, atomTypeId)
            | FunctionType(param, result) ->
                let paramTr = TypeReference()
                let resultTr = TypeReference()
                graph.Function(typeRef, paramTr, resultTr)

                add paramTr param
                add resultTr result
            | QualifiedType _ -> failwith "TODO"

        let identifierType = getIdentifierTypeRef identifier
        add identifierType type_)

    [
      //
      BuiltIn.AtomTypeIds.unit
      BuiltIn.AtomTypeIds.int
      BuiltIn.AtomTypeIds.float
      BuiltIn.AtomTypeIds.string
      //
      ]
    |> List.iter (fun atomTypeId ->
        let typeReference = TypeReference(atomTypeId.ToString())
        graph.Atom(typeReference, atomTypeId)
        scopeTree.Add(typeReference))

let private createQualifiedTypes
    (
        identifierTypes: IReadOnlyDictionary<Identifier, TypeReference>,
        graphInfo: TypeGraphInfo,
        scopeTree: ScopeTree,
        functionApplications: IReadOnlyList<FunctionApplication>
    ) : Map<TypeReference, Type> =

    failwith "TODO"

let private getImplicitTypeArguments
    (
        typeReferenceTypes: Map<TypeReference, Type>,
        functionApplications: IReadOnlyList<FunctionApplication>
    ) : Map<ApplicationReference, Map<AtomTypeId, Type>> =

    let mutable map = Map.empty

    for app in functionApplications do
        let definedType = typeReferenceTypes[app.definedFunctionType]

        match definedType with
        | QualifiedType(typeParameters, definedTypeBody) ->
            let resultType = typeReferenceTypes[app.resultFunctionType]

            let rec getArguments definedType resultType argumentMap =
                match definedType, resultType with
                | AtomType atomTypeId, argument ->
                    if typeParameters |> List.contains atomTypeId then
                        match argumentMap |> Map.tryFind atomTypeId with
                        | Some existingArgument when existingArgument <> argument -> failwith "Type argument conflict"
                        | Some _ -> argumentMap
                        | None -> argumentMap |> Map.add atomTypeId argument
                    else
                        argumentMap
                | FunctionType(dp, dr), FunctionType(rp, rr) ->
                    let argumentMap = getArguments dp rp argumentMap
                    let argumentMap = getArguments dr rr argumentMap
                    argumentMap
                | _ -> failwith "Invalid types"

            let arguments = getArguments definedTypeBody resultType Map.empty

            map <- map |> Map.add app.applicationReference arguments

        | _ -> ()

    map

let getTypeInformation (ast: Program) : TypeInformation =
    let identifierTypes = Dictionary()
    let graph = TypeGraph()
    let scopeTree = ScopeTree()
    let functionApplications = List()

    addBuiltIns (identifierTypes, graph, scopeTree)

    AstTraverser.collectInfoFromAst (identifierTypes, graph, scopeTree, functionApplications) ast

    let graphInfo = graph.GetResult()

    let typeReferenceTypes = createQualifiedTypes (identifierTypes, graphInfo, scopeTree, functionApplications)

    let identifierTypes =
        identifierTypes
        |> Seq.map (fun kv -> kv.Key, typeReferenceTypes[kv.Value])
        |> Map.ofSeq

    let implicitTypeArguments =
        getImplicitTypeArguments (typeReferenceTypes, functionApplications)

    { identifierTypes = identifierTypes
      typeReferenceTypes = typeReferenceTypes
      implicitTypeArguments = implicitTypeArguments }
