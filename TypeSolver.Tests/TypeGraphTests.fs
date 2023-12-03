module TypeSolver.Tests.TypeGraphTests

open NUnit.Framework
open Common
open TypeSolver

let private atomTypeId1 = AtomTypeId("Atom1")
let private atomTypeId2 = AtomTypeId("Atom2")

/// (x : Atom1) solved
[<Test>]
let atomTypeSolved () =
    let typeReference = TypeReference()

    let graph = TypeGraph()
    graph.Atom(typeReference, atomTypeId1)

    let typeMap = graph.GetResult()

    typeMap[typeReference] = AtomType atomTypeId1 |> Assert.True

/// { f : a -> b | a = Atom1 | b = Atom2 } => { f : Atom1 -> Atom2 }
[<Test>]
let functionTypeSolved () =
    let func = TypeReference("func")
    let param = TypeReference("param")
    let result = TypeReference("result")

    let graph = TypeGraph()
    graph.Atom(param, atomTypeId1)
    graph.Atom(result, atomTypeId2)
    graph.Function(func, param, result)

    let typeMap = graph.GetResult()

    typeMap[func] = FunctionType(AtomType atomTypeId1, AtomType atomTypeId2)
    |> Assert.True

[<Test>]
let functionTypeSolvedInverse () =
    let func = TypeReference("func")
    let param = TypeReference("param")
    let result = TypeReference("result")

    let graph = TypeGraph()
    graph.Function(func, param, result)
    graph.Atom(param, atomTypeId1)
    graph.Atom(result, atomTypeId2)

    let typeMap = graph.GetResult()

    typeMap[func] = FunctionType(AtomType atomTypeId1, AtomType atomTypeId2)
    |> Assert.True

/// { f : a -> b } solved
[<Test>]
let polymorphicFunctionSolved () =
    let func = TypeReference("func")
    let param = TypeReference("param")
    let result = TypeReference("result")

    let graph = TypeGraph()
    graph.Function(func, param, result)

    let typeMap = graph.GetResult()

    match typeMap[func] with
    | FunctionType(VariableType v1, VariableType v2) when v1 <> v2 -> Assert.Pass()
    | _ -> Assert.Fail()

/// { instance(i, p) | p : Atom1 } => { i : Atom1 }
[<Test>]
let atomInstanceSolved () =
    let prototype = TypeReference("prototype")
    let instance = TypeReference("instance")

    let graph = TypeGraph()

    graph.Atom(prototype, atomTypeId1)
    graph.Instance(prototype, instance)

    let typeMap = graph.GetResult()

    typeMap[instance] = AtomType atomTypeId1 |> Assert.True

[<Test>]
let atomInstanceSolvedInverse () =
    let prototype = TypeReference("prototype")
    let instance = TypeReference("instance")

    let graph = TypeGraph()

    graph.Instance(prototype, instance)
    graph.Atom(prototype, atomTypeId1)

    let typeMap = graph.GetResult()

    typeMap[instance] = AtomType atomTypeId1 |> Assert.True

/// { p : Atom1 -> Atom2 | instance(i, p) } => { i : Atom1 -> Atom2 }
[<Test>]
let functionInstanceSolved () =
    let funcPrototype = TypeReference("funcPrototype")
    let paramPrototype = TypeReference("paramPrototype")
    let resultPrototype = TypeReference("resultPrototype")
    let funcInstance = TypeReference("funcInstance")

    let graph = TypeGraph()
    graph.Function(funcPrototype, paramPrototype, resultPrototype)
    graph.Atom(paramPrototype, atomTypeId1)
    graph.Atom(resultPrototype, atomTypeId2)
    graph.Instance(funcPrototype, funcInstance)

    let typeMap = graph.GetResult()

    typeMap[funcInstance] = FunctionType(AtomType atomTypeId1, AtomType atomTypeId2)
    |> Assert.True

// [<Test>]
// let ``Function instance is solved (inverse)`` () =
//     let funcPrototype = TypeReference("funcPrototype")
//     let paramPrototype = TypeReference("paramPrototype")
//     let resultPrototype = TypeReference("resultPrototype")
//     let funcInstance = TypeReference("funcInstance")
//
//     let graph = Graph2()
//     graph.Instance(funcPrototype, funcInstance)
//     graph.Function(funcPrototype, paramPrototype, resultPrototype)
//     graph.Atom(paramPrototype, atomTypeId1)
//     graph.Atom(resultPrototype, atomTypeId2)
//
//     let typeMap = graph.GetResult()
//
//     typeMap[funcInstance] = FunctionType(AtomType atomTypeId1, AtomType atomTypeId2)
//     |> Assert.True

/// { p : a -> b | instance(i, p) | i : Atom1 -> Atom2 } solved
[<Test>]
let polymorphicFunctionInstanceSolved () =
    let funcPrototype = TypeReference("funcPrototype")
    let funcInstance = TypeReference("funcInstance")
    let paramInstance = TypeReference("paramInstance")
    let resultInstance = TypeReference("resultInstance")

    let graph = TypeGraph()
    graph.Function(funcPrototype, TypeReference(), TypeReference())
    graph.Function(funcInstance, paramInstance, resultInstance)
    graph.Atom(paramInstance, atomTypeId1)
    graph.Atom(resultInstance, atomTypeId2)
    graph.Instance(funcPrototype, funcInstance)

    let typeMap = graph.GetResult()

    typeMap[funcInstance] = FunctionType(AtomType atomTypeId1, AtomType atomTypeId2)
    |> Assert.True

    match typeMap[funcPrototype] with
    | FunctionType(VariableType _, VariableType _) -> ()
    | _ -> Assert.Fail()

[<Test>]
let polymorphicFunctionInstanceSolvedInverse () =
    let funcPrototype = TypeReference("funcPrototype")
    let funcInstance = TypeReference("funcInstance")
    let paramInstance = TypeReference("paramInstance")
    let resultInstance = TypeReference("resultInstance")

    let graph = TypeGraph()
    graph.Instance(funcPrototype, funcInstance)
    graph.Function(funcInstance, paramInstance, resultInstance)
    graph.Atom(paramInstance, atomTypeId1)
    graph.Atom(resultInstance, atomTypeId2)
    graph.Function(funcPrototype, TypeReference(), TypeReference())

    let typeMap = graph.GetResult()

    typeMap[funcInstance] = FunctionType(AtomType atomTypeId1, AtomType atomTypeId2)
    |> Assert.True

    match typeMap[funcPrototype] with
    | FunctionType(VariableType _, VariableType _) -> ()
    | _ -> Assert.Fail()

/// { p : a -> a | instance(i, p) | i : Atom1 -> b } => { i : Atom1 -> Atom1 }
[<Test>]
let instanceResultInferredToBeSameAsParam () =
    let funcPrototype = TypeReference("funcPrototype")
    let paramAndResultPrototype = TypeReference("paramAndResultPrototype")
    let funcInstance = TypeReference("funcInstance")
    let paramInstance = TypeReference("paramInstance")
    let resultInstance = TypeReference("resultInstance")

    let graph = TypeGraph()
    graph.Function(funcPrototype, paramAndResultPrototype, paramAndResultPrototype)
    graph.Function(funcInstance, paramInstance, resultInstance)
    graph.Atom(paramInstance, atomTypeId1)
    graph.Instance(funcPrototype, funcInstance)

    let typeMap = graph.GetResult()

    typeMap[resultInstance] = AtomType atomTypeId1 |> Assert.True

/// { p : a -> a | instance(i, p) | i : b -> Atom1 } => { i : Atom1 -> Atom1 }
[<Test>]
let instanceParamInferredToBeSameAsResult () =
    let funcPrototype = TypeReference("funcPrototype")
    let paramAndResultPrototype = TypeReference("paramAndResultPrototype")
    let funcInstance = TypeReference("funcInstance")
    let paramInstance = TypeReference("paramInstance")
    let resultInstance = TypeReference("resultInstance")

    let graph = TypeGraph()
    graph.Function(funcPrototype, paramAndResultPrototype, paramAndResultPrototype)
    graph.Function(funcInstance, paramInstance, resultInstance)
    graph.Atom(resultInstance, atomTypeId1)
    graph.Instance(funcPrototype, funcInstance)

    let typeMap = graph.GetResult()

    typeMap[paramInstance] = AtomType atomTypeId1 |> Assert.True

    typeMap[funcInstance] = FunctionType(AtomType atomTypeId1, AtomType atomTypeId1)
    |> Assert.True

/// { p : a -> a | instance(i1, p) | instance(i2, p) | i1 : Atom1 -> b | i2 : Atom2 -> c } => { b : Atom1 | c : Atom2 }
[<Test>]
let twoInstancesInferResultIndependently () =
    let funcPrototype = TypeReference("funcPrototype")
    let paramAndResultPrototype = TypeReference("paramAndResultPrototype")

    let funcInstance1 = TypeReference("funcInstance1")
    let paramInstance1 = TypeReference("paramInstance1")
    let resultInstance1 = TypeReference("resultInstance1")

    let funcInstance2 = TypeReference("funcInstance2")
    let paramInstance2 = TypeReference("paramInstance2")
    let resultInstance2 = TypeReference("resultInstance2")

    let graph = TypeGraph()
    graph.Function(funcPrototype, paramAndResultPrototype, paramAndResultPrototype)
    graph.Function(funcInstance1, paramInstance1, resultInstance1)
    graph.Function(funcInstance2, paramInstance2, resultInstance2)
    graph.Atom(paramInstance1, atomTypeId1)
    graph.Atom(paramInstance2, atomTypeId2)
    graph.Instance(funcPrototype, funcInstance1)
    graph.Instance(funcPrototype, funcInstance2)

    let typeMap = graph.GetResult()

    typeMap[resultInstance1] = AtomType atomTypeId1 |> Assert.True
    typeMap[funcInstance1] = FunctionType(AtomType atomTypeId1, AtomType atomTypeId1)
    |> Assert.True

    typeMap[resultInstance2] = AtomType atomTypeId2 |> Assert.True
    typeMap[funcInstance2] = FunctionType(AtomType atomTypeId2, AtomType atomTypeId2)
    |> Assert.True

/// { p : a -> a | instance(i, x) | i : Atom1 -> b | x = p } => { i : Atom1 -> Atom1 }
[<Test>]
let instanceRelationMaintainedAfterMergingPrototype () =
    let f = TypeReference("f")
    let x = TypeReference("x")

    let refF = TypeReference("ref f")

    let funcInstance = TypeReference("func instance")
    let paramInstance = TypeReference("param instance")
    let resultInstance = TypeReference("result instance")

    let graph = TypeGraph()

    graph.Function (f, x, x)
    graph.Instance (refF, funcInstance)
    graph.Function (funcInstance, paramInstance, resultInstance)
    graph.Identical (refF, f)
    graph.Atom (paramInstance, atomTypeId1)

    let typeMap = graph.GetResult()

    match typeMap[f] with
    | FunctionType (VariableType v1, VariableType v2) when v1 = v2 -> ()
    | _ -> Assert.Fail()

    typeMap[funcInstance] = FunctionType (AtomType atomTypeId1, AtomType atomTypeId1)
    |> Assert.True

[<Test>]
let test2 () =
    let funcPrototype = TypeReference("funcPrototype")
    let paramAndResultPrototype = TypeReference("paramAndResultPrototype")
    let funcInstance = TypeReference("func instance")
    let paramInstance = TypeReference("param instance")
    let paramResult = TypeReference("param result")

    let graph = TypeGraph()

    graph.Function (funcPrototype, paramAndResultPrototype, paramAndResultPrototype)
    graph.Instance (funcPrototype, funcInstance)
    graph.Function (funcInstance, paramInstance, paramResult)
    graph.Atom (paramInstance, atomTypeId1)

    let typeMap = graph.GetResult()

    match typeMap[funcPrototype] with
    | FunctionType (VariableType v1, VariableType v2) when v1 = v2 -> ()
    | _ -> Assert.Fail()

    typeMap[funcInstance] = FunctionType (AtomType atomTypeId1, AtomType atomTypeId1)
    |> Assert.True

[<Test>]
let test3 () =
    let string = AtomTypeId("String")

    let f = TypeReference("f")
    let x = TypeReference("x")

    let instanceOfF = TypeReference("instance of ref f")

    let stringLiteral = TypeReference("string literal")

    let application = TypeReference("application")

    let graph = TypeGraph()

    graph.Function (f, x, x)
    graph.Instance (f, instanceOfF)
    graph.Function (instanceOfF, stringLiteral, application)
    graph.Atom (stringLiteral, string)

    let typeMap = graph.GetResult()

    match typeMap[f] with
    | FunctionType (VariableType v1, VariableType v2) when v1 = v2 -> ()
    | _ -> Assert.Fail()

    typeMap[instanceOfF] = FunctionType (AtomType string, AtomType string)
    |> Assert.True

[<Test>]
let test4 () =
    let int = AtomTypeId("Int")

    let TRUE = TypeReference("TRUE")
    let a = TypeReference("a")
    let application = TypeReference("application")

    let refA = TypeReference("ref a")

    let instanceRefA = TypeReference("instance of ref a")

    let numberLiteral = TypeReference("number literal")

    let graph = TypeGraph()

    graph.Function(TRUE, a, application)
    graph.Instance(refA, instanceRefA)
    graph.Function(instanceRefA, numberLiteral, application)
    graph.Identical(refA, a)
    graph.Atom(numberLiteral, int)

    let typeMap = graph.GetResult() // TODO a function typed parameter of a function should not create prototype-instance relationship

    ()