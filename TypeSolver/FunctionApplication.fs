namespace TypeSolver

open Common

type FunctionApplication =
    { applicationReference: ApplicationId
      definedFunctionType: TypeReference
      resultFunctionType: TypeReference }

