module TypeSolver.Solver

open System
open System.Collections.Generic
open Ast
open Common

let private globalScope = Guid.NewGuid()

let private globalScopeMonomorphicTypes =
    [ BuiltIn.AtomTypeReferences.int
      BuiltIn.AtomTypeReferences.float
      BuiltIn.AtomTypeReferences.string
      BuiltIn.AtomTypeReferences.unit ]

let private addBuiltIns (identifierTypes: Dictionary<Identifier, TypeReference>, graph: TypeGraph, scopeTree: ScopeTree) =
    let createIdentifierTypeRef identifier =
        let tr = TypeReference($"id({identifier})")
        identifierTypes.Add(identifier, tr)
        tr

    graph.Function(createIdentifierTypeRef(BuiltIn.Identifiers.println), BuiltIn.AtomTypeReferences.string, BuiltIn.AtomTypeReferences.unit)
    graph.Function(createIdentifierTypeRef(BuiltIn.Identifiers.intToStr), BuiltIn.AtomTypeReferences.int, BuiltIn.AtomTypeReferences.string)

    let intToStrFmtSubFn = TypeReference()
    graph.Function(createIdentifierTypeRef(BuiltIn.Identifiers.intToStrFmt), BuiltIn.AtomTypeReferences.string, intToStrFmtSubFn)
    graph.Function(intToStrFmtSubFn, BuiltIn.AtomTypeReferences.int, BuiltIn.AtomTypeReferences.string)

    graph.Function(createIdentifierTypeRef(BuiltIn.Identifiers.floatToStr), BuiltIn.AtomTypeReferences.float, BuiltIn.AtomTypeReferences.string)

    let intOp = TypeReference()
    let intOpSubFn = TypeReference()
    graph.Function(intOp, BuiltIn.AtomTypeReferences.int, intOpSubFn)
    graph.Function(intOpSubFn, BuiltIn.AtomTypeReferences.int, BuiltIn.AtomTypeReferences.int)

    graph.Identical(createIdentifierTypeRef(BuiltIn.Identifiers.opAdd), intOp)
    graph.Identical(createIdentifierTypeRef(BuiltIn.Identifiers.opSubtract), intOp)
    graph.Identical(createIdentifierTypeRef(BuiltIn.Identifiers.opMultiply), intOp)
    graph.Identical(createIdentifierTypeRef(BuiltIn.Identifiers.opDivide), intOp)

    // [ BuiltIn.Identifiers.println, BuiltIn.IdentifierTypes.println
    //   BuiltIn.Identifiers.intToStr, BuiltIn.IdentifierTypes.intToStr
    //   BuiltIn.Identifiers.intToStrFmt, BuiltIn.IdentifierTypes.intToStrFmt
    //   BuiltIn.Identifiers.floatToStr, BuiltIn.IdentifierTypes.floatToStr
    //   BuiltIn.Identifiers.opAdd, BuiltIn.IdentifierTypes.opAdd
    //   BuiltIn.Identifiers.opSubtract, BuiltIn.IdentifierTypes.opSubtract
    //   BuiltIn.Identifiers.opMultiply, BuiltIn.IdentifierTypes.opMultiply
    //   BuiltIn.Identifiers.opDivide, BuiltIn.IdentifierTypes.opDivide
    //   BuiltIn.Identifiers.failwith, BuiltIn.IdentifierTypes.failwith ]
    // |> List.iter (fun (identifier, type_) ->
    //     let rec add typeRef type_ =
    //         match type_ with
    //         | AtomType atomTypeId -> graph.Atom(typeRef, atomTypeId)
    //         | FunctionType(param, result) ->
    //             let paramTr = TypeReference()
    //             let resultTr = TypeReference()
    //             graph.Function(typeRef, paramTr, resultTr)
    //
    //             add paramTr param
    //             add resultTr result
    //         | QualifiedType (_, body) ->
    //             add typeRef body
    //
    //     let identifierType = getIdentifierTypeRef identifier
    //     add identifierType type_)

    // [ BuiltIn.AtomTypeIds.unit
    //   BuiltIn.AtomTypeIds.int
    //   BuiltIn.AtomTypeIds.float
    //   BuiltIn.AtomTypeIds.string ]
    // |> List.iter (fun atomTypeId ->
    //     let typeReference = TypeReference(atomTypeId.ToString())
    //     graph.Atom(typeReference, atomTypeId)
    //     scopeTree.Add(typeReference))

// let private createQualifiedTypes
//     (
//         identifierTypes: IReadOnlyDictionary<Identifier, TypeReference>,
//         graphInfo: TypeGraphInfo,
//         scopeTreeInfo: ScopeTreeInfo
//     ) : Map<TypeReference, Type> =
//
//     let typeReferenceIdentifiers =
//         identifierTypes |> Seq.map (fun kv -> kv.Value, kv.Key) |> Map.ofSeq
//
//     let mutable typeReferenceTypes = Map.empty
//
//     for typeReference, type_ in graphInfo.typeReferenceTypes |> Map.toSeq do
//         let identity = graphInfo.typeReferenceIdentities |> Map.find typeReference
//
//         let identicalTypeReferences =
//             graphInfo.typeReferenceIdentities
//             |> Seq.choose (fun kv -> if kv.Value = identity then Some kv.Key else None)
//             |> List.ofSeq
//
//         let aliasedIdentifiers =
//             identicalTypeReferences
//             |> List.choose (fun tRef -> typeReferenceIdentifiers |> Map.tryFind tRef)
//
//         let scopedAtomTypes =
//             aliasedIdentifiers
//             |> Seq.choose (fun i -> scopeTreeInfo.identifierScopedAtomTypes |> Map.tryFind i)
//             |> Seq.distinct
//             |> List.ofSeq
//             |> function
//                 | [] -> None
//                 | [ s ] -> Some s
//                 | _ -> failwith "Only one of aliased identifiers can have a scope"
//
//         // let scopedAtomTypes =
//         //     match typeReferenceIdentifiers |> Map.tryFind typeReference with
//         //     | None -> None
//         //     | Some mainIdentifier ->
//         //         match scopeTreeInfo.identifierScopedAtomTypes |> Map.tryFind mainIdentifier with
//         //         | Some scope -> Some scope
//         //         | None ->
//         //             let aliasedIdentifiers =
//         //                 identicalTypeReferences
//         //                 |> List.choose (fun tRef -> typeReferenceIdentifiers |> Map.tryFind tRef)
//         //
//         //             aliasedIdentifiers
//         //             |> Seq.choose (fun i -> scopeTreeInfo.identifierScopedAtomTypes |> Map.tryFind i)
//         //             |> List.ofSeq
//         //             |> function
//         //                 | [] -> None
//         //                 | [ s ] -> Some s
//         //                 | _ -> failwith "Only one of aliased identifiers can have a scope"
//
//         let type_ =
//             match scopedAtomTypes with
//             | None -> type_
//             | Some scopedAtomTypes -> QualifiedType(scopedAtomTypes, type_)
//
//         typeReferenceTypes <- typeReferenceTypes |> Map.add typeReference type_
//
//     typeReferenceTypes

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
                let resultType =
                    match resultType with
                    | QualifiedType (_, body) -> body
                    | t -> t

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

    addBuiltIns (identifierTypes, graph, scopeTree)

    AstTraverser.collectInfoFromAst (identifierTypes, graph, scopeTree, globalScope, globalScopeMonomorphicTypes) ast

    let graphInfo = graph.GetResult()

    // let scopeTreeInfo = scopeTree.GetResult(graphInfo.typeReferenceTypes)

    // let typeReferenceTypes = createQualifiedTypes (identifierTypes, graphInfo, scopeTreeInfo)
    let typeReferenceTypes = graphInfo.typeReferenceTypes

    let identifierTypes =
        identifierTypes
        |> Seq.map (fun kv -> kv.Key, typeReferenceTypes[kv.Value])
        |> Map.ofSeq

    let implicitTypeArguments = getImplicitTypeArguments (typeReferenceTypes, [ (*TODO*) ])

    { identifierTypes = identifierTypes
      typeReferenceTypes = typeReferenceTypes
      implicitTypeArguments = implicitTypeArguments }
