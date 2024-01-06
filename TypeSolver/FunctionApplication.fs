namespace TypeSolver

open Common

type FunctionApplication =
    { applicationReference: ApplicationReference
      definedFunctionType: TypeReference
      resultFunctionType: TypeReference }

