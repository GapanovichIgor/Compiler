module TypeSolver.Solver

open System.Collections.Generic
open Ast
open Common

let private builtIns =
    [
      //
      // BuiltIn.Identifiers.println, BuiltIn.IdentifierTypes.println
      // BuiltIn.Identifiers.intToStr, BuiltIn.IdentifierTypes.intToStr
      // BuiltIn.Identifiers.intToStrFmt, BuiltIn.IdentifierTypes.intToStrFmt
      // BuiltIn.Identifiers.floatToStr, BuiltIn.IdentifierTypes.floatToStr
      // BuiltIn.Identifiers.opAdd, BuiltIn.IdentifierTypes.opAdd
      // BuiltIn.Identifiers.opSubtract, BuiltIn.IdentifierTypes.opSubtract
      // BuiltIn.Identifiers.opMultiply, BuiltIn.IdentifierTypes.opMultiply
      // BuiltIn.Identifiers.opDivide, BuiltIn.IdentifierTypes.opDivide
      //
      ]

let private addBuiltIns (identifierTypes: Dictionary<Identifier, TypeReference>) (graph: TypeGraph) =
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
            | VariableType _ -> ()
            | FunctionType(param, result) ->
                let paramTr = TypeReference()
                let resultTr = TypeReference()
                graph.Function(typeRef, paramTr, resultTr)

                add paramTr param
                add resultTr result

        let identifierType = getIdentifierTypeRef identifier
        add identifierType type_

    builtIns |> List.iter add

let getTypeInformation (ast: Program) : TypeInformation =
    let identifierTypes = Dictionary()
    let graph = TypeGraph()

    addBuiltIns identifierTypes graph

    let typeScopes = AstTraverser.collectInfoFromAst (identifierTypes, graph) ast

    let typeReferenceTypes = graph.GetResult()

    let identifierTypes =
        identifierTypes :> seq<_>
        |> Seq.map (fun kv -> kv.Key, typeReferenceTypes[kv.Value])
        |> Map.ofSeq

    let typeScopes =
        typeScopes :> seq<_>
        |> Seq.map (fun kv ->
            let scopeRef = kv.Key

            let variableTypeIds =
                kv.Value
                |> Seq.choose (fun tr ->
                    match typeReferenceTypes[tr] with
                    | VariableType variableTypeId -> Some variableTypeId
                    | _ -> None)
                |> List.ofSeq

            scopeRef, variableTypeIds)
        |> Map.ofSeq

    { identifierTypes = identifierTypes
      typeReferenceTypes = typeReferenceTypes
      typeScopes = typeScopes }
