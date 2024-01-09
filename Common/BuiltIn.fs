module Common.BuiltIn

module AtomTypeReferences =
    let unit = TypeReference("Unit")
    let int = TypeReference("Int")
    let float = TypeReference("Float")
    let string = TypeReference("String")

module Identifiers =
    let opAdd = Identifier.Create "opAdd"
    let opSubtract = Identifier.Create "opSubtract"
    let opMultiply = Identifier.Create "opMultiply"
    let opDivide = Identifier.Create "opDivide"
    let println = Identifier.Create "println"
    let intToStr = Identifier.Create "intToStr"
    let intToStrFmt = Identifier.Create "intToStrFmt"
    let floatToStr = Identifier.Create "floatToStr"
    let failwith = Identifier.Create "failwith"
