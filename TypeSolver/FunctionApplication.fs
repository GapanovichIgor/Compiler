namespace TypeSolver

open Common

type internal FunctionApplication =
    { applicationReference: ApplicationReference
      definedFunctionType: TypeReference
      resultFunctionType: TypeReference }

