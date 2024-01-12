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
    | Ok ast ->Solver.getTypeInformation ast
    | Error _ -> failwith "Failed to parse"

let private runLines (codeLines: string list) =
    codeLines
    |> String.concat "\n"
    |> run

let private getType (typeReference: TypeReference) (typeInfo: TypeInformation) =
    typeInfo.typeReferenceTypes |> Map.find typeReference

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
    let intType = typeInfo |> getType BuiltIn.AtomTypeReferences.int

    Assert (xType = intType)

[<Test>]
let valueBindingToIdentifier () =
    let typeInfo =
        runLines [
            "let x = 5"
            "let y = x"
        ]

    let xType = typeInfo |> getIdentifierType "x"
    let yType = typeInfo |> getIdentifierType "y"
    let intType = typeInfo |> getType BuiltIn.AtomTypeReferences.int

    Assert (xType = intType)
    Assert (yType = intType)

[<Test>]
let genericFunctionBindingAToA () =
    let typeInfo = run "let f x = x"

    let xType = typeInfo |> getIdentifierType "x"
    let fType = typeInfo |> getIdentifierType "f"

    let fTypeParams = fType |> getTypeParameters
    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType

    Assert (xType |> isSomeAtomType)
    Assert (xType = fParamType)
    Assert (xType = fResultType)
    Assert (fTypeParams.Length = 1)
    Assert (xType = AtomType (fTypeParams[0]))

[<Test>]
let genericFunctionBindingAliasAToA () =
    let typeInfo =
        runLines [
            "let f x = x"
            "let g = f"
        ]

    let xType = typeInfo |> getIdentifierType "x"
    let fType = typeInfo |> getIdentifierType "f"
    let gType = typeInfo |> getIdentifierType "g"

    let fTypeParams = fType |> getTypeParameters
    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType

    let gTypeParams = gType |> getTypeParameters
    let gParamType = gType |> getFunctionParameterType
    let gResultType = gType |> getFunctionResultType

    Assert (xType |> isSomeAtomType)
    Assert (xType = fParamType)
    Assert (xType = fResultType)
    Assert (fParamType = gParamType)
    Assert (fResultType = gResultType)
    Assert (fTypeParams.Length = 1)
    Assert (xType = AtomType (fTypeParams[0]))
    Assert (gTypeParams.Length = 1)
    Assert (fTypeParams = gTypeParams)

[<Test>]
let genericFunctionBindingAToInt () =
    let typeInfo = run "let f x = 0"

    let xType = typeInfo |> getIdentifierType "x"
    let fType = typeInfo |> getIdentifierType "f"

    let fTypeParams = fType |> getTypeParameters
    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType
    let intType = typeInfo |> getType BuiltIn.AtomTypeReferences.int

    Assert (xType |> isSomeAtomType)
    Assert (xType = fParamType)
    Assert (xType <> fResultType)
    Assert (fResultType = intType)
    Assert (fTypeParams.Length = 1)
    Assert (xType = AtomType fTypeParams[0])

[<Test>]
let functionBindingIntToA () =
    let typeInfo =
        runLines [
            "let f x ="
            "    let y = intToStr x"
            "    failwith \"test\""
        ]

    let xType = typeInfo |> getIdentifierType "x"
    let fType = typeInfo |> getIdentifierType "f"

    let fTypeParams = fType |> getTypeParameters
    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType
    let intType = typeInfo |> getType BuiltIn.AtomTypeReferences.int

    Assert (xType = intType)
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
    let stringType = typeInfo |> getType BuiltIn.AtomTypeReferences.string
    let unitType = typeInfo |> getType BuiltIn.AtomTypeReferences.unit

    Assert (xType = stringType)
    Assert (xType = fParamType)
    Assert (fResultType = unitType)
    Assert (fType |> isQualifiedType |> not)

[<Test>]
let genericFunctionMultipleApplications () =
    let typeInfo =
        runLines [
            "let f x = x"
            "let a = f 0"
            "let b = f \"a\""
        ]

    let intType = typeInfo |> getType BuiltIn.AtomTypeReferences.int
    let stringType = typeInfo |> getType BuiltIn.AtomTypeReferences.string

    let fType = typeInfo |> getIdentifierType "f"
    let fTypeParameters = fType |> getTypeParameters
    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType
    let xType = typeInfo |> getIdentifierType "x"

    let aType = typeInfo |> getIdentifierType "a"
    let bType = typeInfo |> getIdentifierType "b"

    Assert (xType |> isSomeAtomType)
    Assert (xType = fParamType)
    Assert (xType = fResultType)
    Assert (xType <> intType)
    Assert (xType <> stringType)
    Assert (fTypeParameters.Length = 1)
    Assert (xType = AtomType fTypeParameters[0])
    Assert (aType = intType)
    Assert (bType = stringType)

[<Test>]
let genericFunctionAsParameter () =
    let typeInfo =
        runLines [
            "let f x = x"
            "let g y z = y z"
            "let c = g f 0"
        ]

    let intType = typeInfo |> getType BuiltIn.AtomTypeReferences.int

    let fType = typeInfo |> getIdentifierType "f"
    let fTypeParameters = fType |> getTypeParameters
    let fParamType = fType |> getFunctionParameterType
    let fResultType = fType |> getFunctionResultType
    let xType = typeInfo |> getIdentifierType "x"

    let gType = typeInfo |> getIdentifierType "g"
    let gTypeParameters = gType |> getTypeParameters
    let gResultType = gType |> getFunctionResultType
    let yType = typeInfo |> getIdentifierType "y"
    let yParamType = yType |> getFunctionParameterType
    let yResultType = yType |> getFunctionResultType
    let zType = typeInfo |> getIdentifierType "z"

    let cType = typeInfo |> getIdentifierType "c"

    Assert (xType |> isSomeAtomType)
    Assert (xType <> intType)
    Assert (xType = fParamType)
    Assert (xType = fResultType)
    Assert (fTypeParameters.Length = 1)
    Assert (xType = AtomType fTypeParameters[0])

    Assert (zType |> isSomeAtomType)
    Assert (zType <> intType)
    Assert (zType <> gResultType)
    Assert (yParamType = zType)
    Assert (yType = gResultType)
    Assert (gTypeParameters.Length = 2)
    Assert (yParamType = AtomType gTypeParameters[0])
    Assert (yResultType = AtomType gTypeParameters[1])

    Assert (cType = intType)
