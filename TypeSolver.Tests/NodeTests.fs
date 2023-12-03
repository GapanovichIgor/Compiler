module TypeSolver.Tests.NodeTests

// open Microsoft.FSharp.Quotations
// open NUnit.Framework
// open Common
// open TypeSolver
//
// let assertProperties (properties: Expr<bool> list) =
//     failwith ""
//
// let private relateFunction (fn: Node) (parameter: Node) (result: Node) =
//     fn.AddRelation(NodeRelation.Function(parameter, result))
//     parameter.AddRelation(FunctionParameter fn)
//     result.AddRelation(FunctionResult fn)
//
// [<Test>]
// let atomTypeSolved () =
//     let atomTypeId = AtomTypeId()
//     let node = Node()
//
//     node.DefineShape(Atom atomTypeId)
//
//     let resultType = node.GetResultType()
//
//     resultType = AtomType atomTypeId |> Assert.True
//
//     assertProperties [
//         <@ resultType = AtomType atomTypeId @>
//     ]
//
// [<Test>]
// let variableTypeSolved () =
//     let node = Node()
//
//     let resultType = node.GetResultType()
//
//     match resultType with
//     | VariableType _ -> Assert.Pass()
//     | _ -> Assert.Fail()
//
// [<Test>]
// let functionTypeSolved () =
//     let atomTypeId1 = AtomTypeId()
//     let atomTypeId2 = AtomTypeId()
//
//     let functionNode = Node()
//     let parameterNode = Node()
//     let resultNode = Node()
//
//     relateFunction functionNode parameterNode resultNode
//
//     parameterNode.DefineShape(Atom atomTypeId1)
//     resultNode.DefineShape(Atom atomTypeId2)
//
//     let resultType = functionNode.GetResultType()
//
//     resultType = FunctionType(AtomType atomTypeId1, AtomType atomTypeId2)
//     |> Assert.True
//
// [<Test>]
// let polymorphicFunctionTypeSolved () =
//     let functionNode = Node()
//     let parameterNode = Node()
//     let resultNode = Node()
//
//     relateFunction functionNode parameterNode resultNode
//
//     let resultType = functionNode.GetResultType()
//
//     match resultType with
//     | FunctionType(VariableType v1, VariableType v2) when v1 <> v2 -> Assert.Pass()
//     | _ -> Assert.Fail()
//
// [<Test>]
// let functionInstanceSolved () =
//     let functionDefinition = Node()
//     let parameterDefinition = Node()
//     let resultDefinition = Node()
//
//     let atomTypeId1 = AtomTypeId()
//     let atomTypeId2 = AtomTypeId()
//
//     parameterDefinition.DefineShape(Atom atomTypeId1)
//     resultDefinition.DefineShape(Atom atomTypeId2)
//     relateFunction functionDefinition parameterDefinition resultDefinition
//
//     let functionInstanceNode = functionDefinition.CreateInstance()
//     let argumentNode = Node()
//     let applicationResultNode = Node()
//
//     relateFunction functionInstanceNode argumentNode applicationResultNode
//
//     let resultType = functionInstanceNode.GetResultType()
//
//     resultType = FunctionType(AtomType atomTypeId1, AtomType atomTypeId2)
//     |> Assert.True
//
// [<Test>]
// let polymorphicFunctionInstanceSolved () =
//     let functionDefinitionNode = Node()
//     let parameterDefinitionNode = Node()
//     let resultDefinitionNode = Node()
//
//     let functionInstanceNode = functionDefinitionNode.CreateInstance()
//     parameterDefinitionNode.AddRelation(Relay resultDefinitionNode)
//     resultDefinitionNode.AddRelation(Relay parameterDefinitionNode)
//     relateFunction functionDefinitionNode parameterDefinitionNode resultDefinitionNode
//
//     let argumentNode = Node()
//     let applicationResultNode = Node()
//
//     relateFunction functionInstanceNode argumentNode applicationResultNode
//
//     let atomTypeId = AtomTypeId()
//     argumentNode.DefineShape(Atom atomTypeId)
//
//     let functionInstanceResultType = functionInstanceNode.GetResultType()
//
//     functionInstanceResultType = FunctionType(AtomType atomTypeId, AtomType atomTypeId)
//     |> Assert.True
//
// [<Test>]
// let twoInstancesOfPolymorphicFunction () =
//     let functionDefinitionNode = Node()
//     let parameterDefinitionNode = Node()
//     let resultDefinitionNode = Node()
//
//     parameterDefinitionNode.AddRelation(Relay resultDefinitionNode)
//     resultDefinitionNode.AddRelation(Relay parameterDefinitionNode)
//     relateFunction functionDefinitionNode parameterDefinitionNode resultDefinitionNode
//
//     let instance1Node = functionDefinitionNode.CreateInstance()
//     let argument1Node = Node()
//     let applicationResult1Node = Node()
//
//     relateFunction instance1Node argument1Node applicationResult1Node
//
//     let atomType1Id = AtomTypeId()
//     argument1Node.DefineShape(Atom atomType1Id)
//
//     let instance2Node = functionDefinitionNode.CreateInstance()
//     let argument2Node = Node()
//     let applicationResult2Node = Node()
//
//     relateFunction instance2Node argument2Node applicationResult2Node
//
//     let atomType2Id = AtomTypeId()
//     argument2Node.DefineShape(Atom atomType2Id)
//
//     let instance1ResultType = instance1Node.GetResultType()
//     let instance2ResultType = instance2Node.GetResultType()
//
//     instance1ResultType = FunctionType(AtomType atomType1Id, AtomType atomType1Id)
//     |> Assert.True
//
//     instance2ResultType = FunctionType(AtomType atomType2Id, AtomType atomType2Id)
//     |> Assert.True
