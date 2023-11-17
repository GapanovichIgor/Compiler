namespace Compiler

type PositionInSource = { startIndex: int; length: int }

module PositionInSource =
    let fromTo positionA positionB =
        { startIndex = positionA.startIndex
          length = positionB.startIndex + positionB.length - positionA.startIndex }
