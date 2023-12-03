namespace TypeSolver

open Common

type TypeInformation =
    { identifierTypes: Map<Identifier, Type>
      typeReferenceTypes: Map<TypeReference, Type>
      typeScopes: Map<TypeScopeReference, VariableTypeId list> }