module Tests

open System.IO
open System.Text
open Common
open NUnit.Framework
open Parser
open TypeSolver

let private run (code: string) =
    let stream = new MemoryStream(Encoding.UTF8.GetBytes(code))
    match Parser.parseAst stream with
    | Ok ast -> Solver.getTypeInformation ast
    | Error _ -> failwith "Failed to parse"

let private identifierType (identifier: string) (typeInfo: TypeInformation) =
    typeInfo.identifierTypes
    |> Map.toSeq
    |> Seq.choose (fun (i, t) ->
        if i.Name = identifier then
            Some t
        else
            None)
    |> Seq.exactlyOne

let private assertIsAtom (atomTypeId: AtomTypeId) (t: Type) =
    match t with
    | AtomType aId when aId = atomTypeId -> ()
    | _ -> Assert.Fail($"The type is not an atom type {atomTypeId}")

[<Test>]
let valueBindingToLiteral () =
    let typeInfo = run "let x = 5"

    typeInfo |> identifierType "x" |> assertIsAtom BuiltIn.AtomTypeIds.int