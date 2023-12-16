module TypeSolver.Solver

open System.Collections.Generic
open Ast
open Common

type private AtomTypeScopeOwner =
    | Global
    | Owner of TypeReference

type private AtomTypeScope =
    { owner: AtomTypeScopeOwner
      parentScope: AtomTypeScope option
      childScopes: List<AtomTypeScope>
      atomTypes: List<AtomTypeId> }

let private addBuiltInIdentifiers (identifierTypes: Dictionary<Identifier, TypeReference>) (graph: TypeGraph) =
    let getIdentifierTypeRef identifier =
        match identifierTypes.TryGetValue(identifier) with
        | true, tr -> tr
        | false, _ ->
            let tr = TypeReference($"identifier '{identifier}'")
            identifierTypes[identifier] <- tr
            tr

    let add (identifier, type_) =
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
        add identifierType type_

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
    |> List.iter add

let private getBuiltInAtomTypeIds () =
    [ BuiltIn.AtomTypeIds.unit
      BuiltIn.AtomTypeIds.int
      BuiltIn.AtomTypeIds.float
      BuiltIn.AtomTypeIds.string ]

let private createQualifiedTypes
    (rootTypeReferenceScope: AstTraverser.TypeReferenceScope)
    (typeReferenceTypes: Map<TypeReference, Type>)
    : Map<TypeReference, Type> =

    let rootScope =
        { owner = Global
          parentScope = None
          childScopes = List()
          atomTypes = List(getBuiltInAtomTypeIds ()) }

    let addAtomTypeIdToScope (atomTypeId: AtomTypeId) (scope: AtomTypeScope) =
        let rec containedInTree atomTypeId scope =
            scope.atomTypes.Contains(atomTypeId)
            || scope.parentScope
               |> Option.map (containedInTree atomTypeId)
               |> Option.defaultValue false

        if not (containedInTree atomTypeId scope) then
            scope.atomTypes.Add(atomTypeId)

    let rec populateScope (atomTypeScope: AtomTypeScope) (typeReferenceScope: AstTraverser.TypeReferenceScope) =
        for tr in typeReferenceScope.containedTypeReferences do
            match typeReferenceTypes[tr] with
            | AtomType atomTypeId -> atomTypeScope |> addAtomTypeIdToScope atomTypeId
            | _ -> ()

        for owner, typeReferenceChildScope in typeReferenceScope.childScopes |> Map.toSeq do
            let childAtomTypeScope =
                { owner = Owner owner
                  parentScope = Some atomTypeScope
                  childScopes = List()
                  atomTypes = List() }

            atomTypeScope.childScopes.Add(childAtomTypeScope)

            populateScope childAtomTypeScope typeReferenceChildScope

    populateScope rootScope rootTypeReferenceScope

    let rec replaceTypes (scope: AtomTypeScope) (typeReferenceTypes: Map<TypeReference, Type>) =
        let typeReferenceTypes =
            match scope.owner with
            | Owner ownerTr when scope.atomTypes.Count > 0 ->
                let typeParameters = scope.atomTypes |> List.ofSeq
                let typeResult = typeReferenceTypes |> Map.find ownerTr
                let qualifiedType = QualifiedType(typeParameters, typeResult)
                typeReferenceTypes |> Map.add ownerTr qualifiedType
            | _ -> typeReferenceTypes

        scope.childScopes
        |> Seq.fold (fun trTypes childScope -> replaceTypes childScope trTypes) typeReferenceTypes

    replaceTypes rootScope typeReferenceTypes

let private getImplicitTypeArguments
    (
        typeReferenceTypes: Map<TypeReference, Type>,
        functionApplications: AstTraverser.FunctionApplication list
    ) : Map<ApplicationReference, Type list> =

    let mutable map = Map.empty

    for app in functionApplications do
        let definedType = typeReferenceTypes[app.definedFunctionType]
        match definedType with
        | QualifiedType (_, definedTypeBody) ->
            let resultType = typeReferenceTypes[app.resultFunctionType]

            let rec getArguments definedType resultType =
                match definedType, resultType with
                | AtomType _, r -> [ r ]
                | FunctionType (dp, dr), FunctionType (rp, rr) ->
                    getArguments dp rp @ getArguments dr rr
                | _ -> failwith "Invalid types"

            let arguments = getArguments definedTypeBody resultType

            map <- map |> Map.add app.applicationReference arguments

        | _ -> ()

    map

let getTypeInformation (ast: Program) : TypeInformation =
    let identifierTypes = Dictionary()
    let graph = TypeGraph()

    addBuiltInIdentifiers identifierTypes graph

    let astTypeInfo = AstTraverser.collectInfoFromAst (identifierTypes, graph) ast

    let typeReferenceTypes = graph.GetResult()

    let typeReferenceTypes =
        typeReferenceTypes |> createQualifiedTypes astTypeInfo.rootScope

    let identifierTypes =
        identifierTypes
        |> Seq.map (fun kv -> kv.Key, typeReferenceTypes[kv.Value])
        |> Map.ofSeq

    let implicitTypeArguments =
        getImplicitTypeArguments (typeReferenceTypes, astTypeInfo.functionApplications)

    { identifierTypes = identifierTypes
      typeReferenceTypes = typeReferenceTypes
      implicitTypeArguments = implicitTypeArguments }
