module internal rec Compiler.AstGenerated

(*
STATES
   0 { ArithmeticOpOrderA -> ·ArithmeticOpOrderA Asterisk NumberLiteralExpression [] | ArithmeticOpOrderA -> ·ArithmeticOpOrderA Slash NumberLiteralExpression [] | ArithmeticOpOrderA -> ·NumberLiteralExpression [] | ArithmeticOpOrderB -> ·ArithmeticOpOrderA [] | ArithmeticOpOrderB -> ·ArithmeticOpOrderB Minus ArithmeticOpOrderA [] | ArithmeticOpOrderB -> ·ArithmeticOpOrderB Plus ArithmeticOpOrderA [] | Expression -> ·ArithmeticOpOrderB [] | NumberLiteralExpression -> ·NumberLiteral [] | Program -> ·Expression [] }
   1 { ArithmeticOpOrderA -> ·ArithmeticOpOrderA Asterisk NumberLiteralExpression [] | ArithmeticOpOrderA -> ·ArithmeticOpOrderA Slash NumberLiteralExpression [] | ArithmeticOpOrderA -> ·NumberLiteralExpression [] | ArithmeticOpOrderB -> ArithmeticOpOrderB Minus· ArithmeticOpOrderA [] | NumberLiteralExpression -> ·NumberLiteral [] }
   2 { ArithmeticOpOrderA -> ·ArithmeticOpOrderA Asterisk NumberLiteralExpression [] | ArithmeticOpOrderA -> ·ArithmeticOpOrderA Slash NumberLiteralExpression [] | ArithmeticOpOrderA -> ·NumberLiteralExpression [] | ArithmeticOpOrderB -> ArithmeticOpOrderB Plus· ArithmeticOpOrderA [] | NumberLiteralExpression -> ·NumberLiteral [] }
   3 { ArithmeticOpOrderA -> ArithmeticOpOrderA· Asterisk NumberLiteralExpression [] | ArithmeticOpOrderA -> ArithmeticOpOrderA· Slash NumberLiteralExpression [] | ArithmeticOpOrderB -> ArithmeticOpOrderA· [$ Minus Plus] }
   4 { ArithmeticOpOrderA -> ArithmeticOpOrderA· Asterisk NumberLiteralExpression [] | ArithmeticOpOrderA -> ArithmeticOpOrderA· Slash NumberLiteralExpression [] | ArithmeticOpOrderB -> ArithmeticOpOrderB Minus ArithmeticOpOrderA· [$ Minus Plus] }
   5 { ArithmeticOpOrderA -> ArithmeticOpOrderA· Asterisk NumberLiteralExpression [] | ArithmeticOpOrderA -> ArithmeticOpOrderA· Slash NumberLiteralExpression [] | ArithmeticOpOrderB -> ArithmeticOpOrderB Plus ArithmeticOpOrderA· [$ Minus Plus] }
   6 { ArithmeticOpOrderA -> ArithmeticOpOrderA Asterisk· NumberLiteralExpression [] | NumberLiteralExpression -> ·NumberLiteral [] }
   7 { ArithmeticOpOrderA -> ArithmeticOpOrderA Asterisk NumberLiteralExpression· [$ Asterisk Minus Plus Slash] }
   8 { ArithmeticOpOrderA -> ArithmeticOpOrderA Slash· NumberLiteralExpression [] | NumberLiteralExpression -> ·NumberLiteral [] }
   9 { ArithmeticOpOrderA -> ArithmeticOpOrderA Slash NumberLiteralExpression· [$ Asterisk Minus Plus Slash] }
   10 { ArithmeticOpOrderA -> NumberLiteralExpression· [$ Asterisk Minus Plus Slash] }
   11 { ArithmeticOpOrderB -> ArithmeticOpOrderB· Minus ArithmeticOpOrderA [] | ArithmeticOpOrderB -> ArithmeticOpOrderB· Plus ArithmeticOpOrderA [] | Expression -> ArithmeticOpOrderB· [$] }
   12 { NumberLiteralExpression -> NumberLiteral· [$ Asterisk Minus Plus Slash] }
   13 { Program -> Expression· [$] }

PRODUCTIONS
   ArithmeticOpOrderB -> ArithmeticOpOrderA
   ArithmeticOpOrderB -> ArithmeticOpOrderB Minus ArithmeticOpOrderA
   ArithmeticOpOrderB -> ArithmeticOpOrderB Plus ArithmeticOpOrderA
   ArithmeticOpOrderA -> ArithmeticOpOrderA Asterisk NumberLiteralExpression
   ArithmeticOpOrderA -> ArithmeticOpOrderA Slash NumberLiteralExpression
   ArithmeticOpOrderA -> NumberLiteralExpression
   Expression -> ArithmeticOpOrderB
   NumberLiteralExpression -> NumberLiteral

ACTION
   State Lookahead     Action
   0     NumberLiteral shift (12)
   1     NumberLiteral shift (12)
   2     NumberLiteral shift (12)
   3     $             reduce (ArithmeticOpOrderB -> ArithmeticOpOrderA)
   3     Asterisk      shift (6)
   3     Minus         reduce (ArithmeticOpOrderB -> ArithmeticOpOrderA)
   3     Plus          reduce (ArithmeticOpOrderB -> ArithmeticOpOrderA)
   3     Slash         shift (8)
   4     $             reduce (ArithmeticOpOrderB -> ArithmeticOpOrderB Minus ArithmeticOpOrderA)
   4     Asterisk      shift (6)
   4     Minus         reduce (ArithmeticOpOrderB -> ArithmeticOpOrderB Minus ArithmeticOpOrderA)
   4     Plus          reduce (ArithmeticOpOrderB -> ArithmeticOpOrderB Minus ArithmeticOpOrderA)
   4     Slash         shift (8)
   5     $             reduce (ArithmeticOpOrderB -> ArithmeticOpOrderB Plus ArithmeticOpOrderA)
   5     Asterisk      shift (6)
   5     Minus         reduce (ArithmeticOpOrderB -> ArithmeticOpOrderB Plus ArithmeticOpOrderA)
   5     Plus          reduce (ArithmeticOpOrderB -> ArithmeticOpOrderB Plus ArithmeticOpOrderA)
   5     Slash         shift (8)
   6     NumberLiteral shift (12)
   7     $             reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Asterisk NumberLiteralExpression)
   7     Asterisk      reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Asterisk NumberLiteralExpression)
   7     Minus         reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Asterisk NumberLiteralExpression)
   7     Plus          reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Asterisk NumberLiteralExpression)
   7     Slash         reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Asterisk NumberLiteralExpression)
   8     NumberLiteral shift (12)
   9     $             reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Slash NumberLiteralExpression)
   9     Asterisk      reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Slash NumberLiteralExpression)
   9     Minus         reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Slash NumberLiteralExpression)
   9     Plus          reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Slash NumberLiteralExpression)
   9     Slash         reduce (ArithmeticOpOrderA -> ArithmeticOpOrderA Slash NumberLiteralExpression)
   10    $             reduce (ArithmeticOpOrderA -> NumberLiteralExpression)
   10    Asterisk      reduce (ArithmeticOpOrderA -> NumberLiteralExpression)
   10    Minus         reduce (ArithmeticOpOrderA -> NumberLiteralExpression)
   10    Plus          reduce (ArithmeticOpOrderA -> NumberLiteralExpression)
   10    Slash         reduce (ArithmeticOpOrderA -> NumberLiteralExpression)
   11    $             reduce (Expression -> ArithmeticOpOrderB)
   11    Minus         shift (1)
   11    Plus          shift (2)
   12    $             reduce (NumberLiteralExpression -> NumberLiteral)
   12    Asterisk      reduce (NumberLiteralExpression -> NumberLiteral)
   12    Minus         reduce (NumberLiteralExpression -> NumberLiteral)
   12    Plus          reduce (NumberLiteralExpression -> NumberLiteral)
   12    Slash         reduce (NumberLiteralExpression -> NumberLiteral)
   13    $             accept

GOTO
   Source state Symbol                  Destination state
   0            ArithmeticOpOrderA      3
   0            ArithmeticOpOrderB      11
   0            Expression              13
   0            NumberLiteralExpression 10
   1            ArithmeticOpOrderA      4
   1            NumberLiteralExpression 10
   2            ArithmeticOpOrderA      5
   2            NumberLiteralExpression 10
   6            NumberLiteralExpression 7
   8            NumberLiteralExpression 9

*)

type Asterisk = unit
type Minus = unit
type NumberLiteral = int * int option
type Plus = unit
type Slash = unit

type ArithmeticOpOrderA =
    | Divide of ArithmeticOpOrderA * Slash * NumberLiteralExpression
    | Fallthrough of NumberLiteralExpression
    | Multiply of ArithmeticOpOrderA * Asterisk * NumberLiteralExpression

type ArithmeticOpOrderB =
    | Add of ArithmeticOpOrderB * Plus * ArithmeticOpOrderA
    | Fallthrough of ArithmeticOpOrderA
    | Subtract of ArithmeticOpOrderB * Minus * ArithmeticOpOrderA

type Expression =
    | Expression of ArithmeticOpOrderB

type NumberLiteralExpression =
    | NumberLiteralExpression of NumberLiteral

type Program =
    | Program of Expression

type InputItem =
    | Asterisk of Asterisk
    | Minus of Minus
    | NumberLiteral of NumberLiteral
    | Plus of Plus
    | Slash of Slash

type Unexpected =
    | EndOfStream
    | InputItem of InputItem

type ExpectedItem =
    | EndOfStream
    | Asterisk
    | Minus
    | NumberLiteral
    | Plus
    | Slash

type ParseError = {
    unexpected : Unexpected
    expected : list<ExpectedItem>
}

let parse (input: #seq<InputItem>) : Result<Program, ParseError> =
    use inputEnumerator = input.GetEnumerator()
    let lhsStack = System.Collections.Stack(50)
    let stateStack = System.Collections.Generic.Stack<int>(50)
    let mutable result = Unchecked.defaultof<Program>
    let mutable accepted = false
    let mutable expected = Unchecked.defaultof<list<ExpectedItem>>

    stateStack.Push(0)

    let mutable lookahead, lookaheadIsEof =
        if inputEnumerator.MoveNext()
        then (inputEnumerator.Current, false)
        else (Unchecked.defaultof<InputItem>, true)

    let mutable keepGoing = true
    while keepGoing do
        match stateStack.Peek() with
        | 0 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(12)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 1 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(12)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 2 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(12)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 3 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderB.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(6)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderB.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderB.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(8)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 4 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderB
                let reductionResult = ArithmeticOpOrderB.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(6)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderB
                let reductionResult = ArithmeticOpOrderB.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderB
                let reductionResult = ArithmeticOpOrderB.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(8)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 5 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderB
                let reductionResult = ArithmeticOpOrderB.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(6)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderB
                let reductionResult = ArithmeticOpOrderB.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderB
                let reductionResult = ArithmeticOpOrderB.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(8)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 6 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(12)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 7 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 8 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(12)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 9 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderA
                let reductionResult = ArithmeticOpOrderA.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 10 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = ArithmeticOpOrderA.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = ArithmeticOpOrderA.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = ArithmeticOpOrderA.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = ArithmeticOpOrderA.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = ArithmeticOpOrderA.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 3
                    | 1 -> 4
                    | 2 -> 5
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 11 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticOpOrderB
                let reductionResult = Expression.Expression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.Plus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Minus; ExpectedItem.Plus]
                keepGoing <- false
        | 12 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = NumberLiteralExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 6 -> 7
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = NumberLiteralExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 6 -> 7
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = NumberLiteralExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 6 -> 7
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = NumberLiteralExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 6 -> 7
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = NumberLiteralExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 6 -> 7
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 13 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // accept
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Expression
                let reductionResult = Program.Program arg1
                result <- reductionResult
                accepted <- true
                keepGoing <- false
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream]
                keepGoing <- false
        | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."

    if accepted
    then Ok result
    else Error {
        unexpected = if lookaheadIsEof then Unexpected.EndOfStream else Unexpected.InputItem lookahead
        expected = expected
    }
