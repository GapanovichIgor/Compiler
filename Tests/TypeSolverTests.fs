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

let private getIdentifierType (identifier: string) (typeInfo: TypeInformation) =
    let types =
        typeInfo.identifierTypes
        |> Map.toSeq
        |> Seq.choose (fun (i, t) -> if i.Name = identifier then Some t else None)
        |> List.ofSeq

    match types with
    | [] -> failwith $"Type is not defined for identifier {identifier}"
    | [ t ] -> t
    | _ -> failwith $"Found more than one identifier {identifier}"

let rec private getFunctionParameterType (t: Type) =
    match t with
    | FunctionType (p, _) -> p
    | QualifiedType (_ , body) -> getFunctionParameterType body
    | _ -> failwith "Type is not a function"

let rec private getFunctionResultType (t: Type) =
    match t with
    | FunctionType (_, r) -> r
    | QualifiedType (_ , body) -> getFunctionResultType body
    | _ -> failwith "Type is not a function"

let private getTypeParameters (t: Type) =
    match t with
    | QualifiedType (ps, _) -> ps
    | _ -> failwith "Type is not a qualified type"

let private isSomeAtomType (t: Type) =
    match t with
    | AtomType _ -> true
    | _ -> false

let private isQualifiedType (t: Type) =
    match t with
    | QualifiedType _ -> true
    | _ -> false

let private Assert (cond: bool) =
    Assert.IsTrue(cond)

[<Test>]
let valueBindingToLiteral () =
    let typeInfo = run "let x = 5"

    let xType = typeInfo |> getIdentifierType "x"

    Assert (xType = BuiltIn.Types.int)

[<Test>]
let valueBindingToIdentifier () =
    let typeInfo =
        "let x = 5\n\
         let y = x"
        |> run

    let xType = typeInfo |> getIdentifierType "x"
    let yType = typeInfo |> getIdentifierType "y"

    Assert (xType = BuiltIn.Types.int)
    Assert (yType = BuiltIn.Types.int)

[<Test>]
let genericFunctionBindingAToA () =
    let typeInfo = run "let id x = x"

    let xType = typeInfo |> getIdentifierType "x"
    let idType = typeInfo |> getIdentifierType "id"

    let idTypeParams = idType |> getTypeParameters
    let idParamType = idType |> getFunctionParameterType
    let idResultType = idType |> getFunctionResultType

    Assert (xType |> isSomeAtomType)
    Assert (xType = idParamType)
    Assert (xType = idResultType)
    Assert (idTypeParams.Length = 1)
    Assert (xType = AtomType (idTypeParams[0]))

[<Test>]
let genericFunctionBindingAToInt () =
    let typeInfo = run "let f x = 0"

    let xType = typeInfo |> getIdentifierType "x"
    let fType = typeInfo |> getIdentifierType "f"

    let fTypeParams = fType |> getTypeParameters
    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType

    Assert (xType |> isSomeAtomType)
    Assert (xType = fParamType)
    Assert (xType <> fResultType)
    Assert (fResultType = BuiltIn.Types.int)
    Assert (fTypeParams.Length = 1)
    Assert (xType = AtomType fTypeParams[0])

[<Test>]
let functionBindingIntToA () =
    let typeInfo =
        "\
let f x =
    let y = intToStr x
    failwith \"test\""
        |> run

    let xType = typeInfo |> getIdentifierType "x"
    let fType = typeInfo |> getIdentifierType "f"

    let fTypeParams = fType |> getTypeParameters
    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType

    Assert (xType = BuiltIn.Types.int)
    Assert (xType = fParamType)
    Assert (xType <> fResultType)
    Assert (fResultType |> isSomeAtomType)
    Assert (fTypeParams.Length = 1)
    Assert (fResultType = AtomType fTypeParams[0])

[<Test>]
let functionBindingStringToUnit () =
    let typeInfo = run "let f x = println x"

    let xType = typeInfo |> getIdentifierType "x"
    let fType = typeInfo |> getIdentifierType "f"

    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType

    Assert (xType = BuiltIn.Types.string)
    Assert (xType = fParamType)
    Assert (fResultType = BuiltIn.Types.unit)
    Assert (fType |> isQualifiedType |> not)
