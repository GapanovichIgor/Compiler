namespace TypeSolver

open Common

type TypeInformation =
    { identifierTypes: Map<Identifier, Type>
      typeReferenceTypes: Map<TypeReference, Type>
      implicitTypeArguments: Map<ApplicationId, Map<AtomTypeId, Type>> }