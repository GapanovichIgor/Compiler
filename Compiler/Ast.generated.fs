module internal rec Compiler.AstGenerated

(*
STATES
   0 { AddExpression -> ·AddExpression Plus SubtractExpression [] | AddExpression -> ·SubtractExpression [] | DivideExpression -> ·DivideExpression Slash NumberLiteralExpression [] | DivideExpression -> ·NumberLiteralExpression [] | Expression -> ·AddExpression [] | MultiplyExpression -> ·DivideExpression [] | MultiplyExpression -> ·MultiplyExpression Asterisk DivideExpression [] | NumberLiteralExpression -> ·NumberLiteral [] | Program -> ·Expression [] | SubtractExpression -> ·MultiplyExpression [] | SubtractExpression -> ·SubtractExpression Minus MultiplyExpression [] }
   1 { AddExpression -> AddExpression· Plus SubtractExpression [] | Expression -> AddExpression· [$] }
   2 { AddExpression -> AddExpression Plus· SubtractExpression [] | DivideExpression -> ·DivideExpression Slash NumberLiteralExpression [] | DivideExpression -> ·NumberLiteralExpression [] | MultiplyExpression -> ·DivideExpression [] | MultiplyExpression -> ·MultiplyExpression Asterisk DivideExpression [] | NumberLiteralExpression -> ·NumberLiteral [] | SubtractExpression -> ·MultiplyExpression [] | SubtractExpression -> ·SubtractExpression Minus MultiplyExpression [] }
   3 { AddExpression -> AddExpression Plus SubtractExpression· [$ Plus] | SubtractExpression -> SubtractExpression· Minus MultiplyExpression [] }
   4 { AddExpression -> SubtractExpression· [$ Plus] | SubtractExpression -> SubtractExpression· Minus MultiplyExpression [] }
   5 { DivideExpression -> ·DivideExpression Slash NumberLiteralExpression [] | DivideExpression -> ·NumberLiteralExpression [] | MultiplyExpression -> ·DivideExpression [] | MultiplyExpression -> ·MultiplyExpression Asterisk DivideExpression [] | NumberLiteralExpression -> ·NumberLiteral [] | SubtractExpression -> SubtractExpression Minus· MultiplyExpression [] }
   6 { DivideExpression -> ·DivideExpression Slash NumberLiteralExpression [] | DivideExpression -> ·NumberLiteralExpression [] | MultiplyExpression -> MultiplyExpression Asterisk· DivideExpression [] | NumberLiteralExpression -> ·NumberLiteral [] }
   7 { DivideExpression -> DivideExpression· Slash NumberLiteralExpression [] | MultiplyExpression -> DivideExpression· [$ Asterisk Minus Plus] }
   8 { DivideExpression -> DivideExpression· Slash NumberLiteralExpression [] | MultiplyExpression -> MultiplyExpression Asterisk DivideExpression· [$ Asterisk Minus Plus] }
   9 { DivideExpression -> DivideExpression Slash· NumberLiteralExpression [] | NumberLiteralExpression -> ·NumberLiteral [] }
   10 { DivideExpression -> DivideExpression Slash NumberLiteralExpression· [$ Asterisk Minus Plus Slash] }
   11 { DivideExpression -> NumberLiteralExpression· [$ Asterisk Minus Plus Slash] }
   12 { MultiplyExpression -> MultiplyExpression· Asterisk DivideExpression [] | SubtractExpression -> MultiplyExpression· [$ Minus Plus] }
   13 { MultiplyExpression -> MultiplyExpression· Asterisk DivideExpression [] | SubtractExpression -> SubtractExpression Minus MultiplyExpression· [$ Minus Plus] }
   14 { NumberLiteralExpression -> NumberLiteral· [$ Asterisk Minus Plus Slash] }
   15 { Program -> Expression· [$] }

PRODUCTIONS
   Expression -> AddExpression
   AddExpression -> AddExpression Plus SubtractExpression
   AddExpression -> SubtractExpression
   MultiplyExpression -> DivideExpression
   MultiplyExpression -> MultiplyExpression Asterisk DivideExpression
   DivideExpression -> DivideExpression Slash NumberLiteralExpression
   DivideExpression -> NumberLiteralExpression
   SubtractExpression -> MultiplyExpression
   SubtractExpression -> SubtractExpression Minus MultiplyExpression
   NumberLiteralExpression -> NumberLiteral

ACTION
   State Lookahead     Action
   0     NumberLiteral shift (14)
   1     $             reduce (Expression -> AddExpression)
   1     Plus          shift (2)
   2     NumberLiteral shift (14)
   3     $             reduce (AddExpression -> AddExpression Plus SubtractExpression)
   3     Minus         shift (5)
   3     Plus          reduce (AddExpression -> AddExpression Plus SubtractExpression)
   4     $             reduce (AddExpression -> SubtractExpression)
   4     Minus         shift (5)
   4     Plus          reduce (AddExpression -> SubtractExpression)
   5     NumberLiteral shift (14)
   6     NumberLiteral shift (14)
   7     $             reduce (MultiplyExpression -> DivideExpression)
   7     Asterisk      reduce (MultiplyExpression -> DivideExpression)
   7     Minus         reduce (MultiplyExpression -> DivideExpression)
   7     Plus          reduce (MultiplyExpression -> DivideExpression)
   7     Slash         shift (9)
   8     $             reduce (MultiplyExpression -> MultiplyExpression Asterisk DivideExpression)
   8     Asterisk      reduce (MultiplyExpression -> MultiplyExpression Asterisk DivideExpression)
   8     Minus         reduce (MultiplyExpression -> MultiplyExpression Asterisk DivideExpression)
   8     Plus          reduce (MultiplyExpression -> MultiplyExpression Asterisk DivideExpression)
   8     Slash         shift (9)
   9     NumberLiteral shift (14)
   10    $             reduce (DivideExpression -> DivideExpression Slash NumberLiteralExpression)
   10    Asterisk      reduce (DivideExpression -> DivideExpression Slash NumberLiteralExpression)
   10    Minus         reduce (DivideExpression -> DivideExpression Slash NumberLiteralExpression)
   10    Plus          reduce (DivideExpression -> DivideExpression Slash NumberLiteralExpression)
   10    Slash         reduce (DivideExpression -> DivideExpression Slash NumberLiteralExpression)
   11    $             reduce (DivideExpression -> NumberLiteralExpression)
   11    Asterisk      reduce (DivideExpression -> NumberLiteralExpression)
   11    Minus         reduce (DivideExpression -> NumberLiteralExpression)
   11    Plus          reduce (DivideExpression -> NumberLiteralExpression)
   11    Slash         reduce (DivideExpression -> NumberLiteralExpression)
   12    $             reduce (SubtractExpression -> MultiplyExpression)
   12    Asterisk      shift (6)
   12    Minus         reduce (SubtractExpression -> MultiplyExpression)
   12    Plus          reduce (SubtractExpression -> MultiplyExpression)
   13    $             reduce (SubtractExpression -> SubtractExpression Minus MultiplyExpression)
   13    Asterisk      shift (6)
   13    Minus         reduce (SubtractExpression -> SubtractExpression Minus MultiplyExpression)
   13    Plus          reduce (SubtractExpression -> SubtractExpression Minus MultiplyExpression)
   14    $             reduce (NumberLiteralExpression -> NumberLiteral)
   14    Asterisk      reduce (NumberLiteralExpression -> NumberLiteral)
   14    Minus         reduce (NumberLiteralExpression -> NumberLiteral)
   14    Plus          reduce (NumberLiteralExpression -> NumberLiteral)
   14    Slash         reduce (NumberLiteralExpression -> NumberLiteral)
   15    $             accept

GOTO
   Source state Symbol                  Destination state
   0            AddExpression           1
   0            DivideExpression        7
   0            Expression              15
   0            MultiplyExpression      12
   0            NumberLiteralExpression 11
   0            SubtractExpression      4
   2            DivideExpression        7
   2            MultiplyExpression      12
   2            NumberLiteralExpression 11
   2            SubtractExpression      3
   5            DivideExpression        7
   5            MultiplyExpression      13
   5            NumberLiteralExpression 11
   6            DivideExpression        8
   6            NumberLiteralExpression 11
   9            NumberLiteralExpression 10

*)

type Asterisk = unit
type Minus = unit
type NumberLiteral = int * int option
type Plus = unit
type Slash = unit

type AddExpression =
    | AddExpression of AddExpression * Plus * SubtractExpression
    | SubtractExpression of SubtractExpression

type DivideExpression =
    | DivideExpression of DivideExpression * Slash * NumberLiteralExpression
    | NumberLiteralExpression of NumberLiteralExpression

type Expression =
    | Expression of AddExpression

type MultiplyExpression =
    | DivideExpression of DivideExpression
    | MultiplyExpression of MultiplyExpression * Asterisk * DivideExpression

type NumberLiteralExpression =
    | NumberLiteralExpression of NumberLiteral

type Program =
    | Program of Expression

type SubtractExpression =
    | MultiplyExpression of MultiplyExpression
    | SubtractExpression of SubtractExpression * Minus * MultiplyExpression

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
                stateStack.Push(14)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 1 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AddExpression
                let reductionResult = Expression.Expression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
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
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Plus]
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
                stateStack.Push(14)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 3 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> SubtractExpression
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> AddExpression
                let reductionResult = AddExpression.AddExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(5)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> SubtractExpression
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> AddExpression
                let reductionResult = AddExpression.AddExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Minus; ExpectedItem.Plus]
                keepGoing <- false
        | 4 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> SubtractExpression
                let reductionResult = AddExpression.SubtractExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(5)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> SubtractExpression
                let reductionResult = AddExpression.SubtractExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Minus; ExpectedItem.Plus]
                keepGoing <- false
        | 5 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(14)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
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
                stateStack.Push(14)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 7 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = MultiplyExpression.DivideExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 2 -> 12
                    | 5 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = MultiplyExpression.DivideExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 2 -> 12
                    | 5 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = MultiplyExpression.DivideExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 2 -> 12
                    | 5 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = MultiplyExpression.DivideExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 2 -> 12
                    | 5 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(9)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 8 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> DivideExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = MultiplyExpression.MultiplyExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 2 -> 12
                    | 5 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> DivideExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = MultiplyExpression.MultiplyExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 2 -> 12
                    | 5 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> DivideExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = MultiplyExpression.MultiplyExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 2 -> 12
                    | 5 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> DivideExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = MultiplyExpression.MultiplyExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 2 -> 12
                    | 5 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(9)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 9 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(14)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 10 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = DivideExpression.DivideExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = DivideExpression.DivideExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = DivideExpression.DivideExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = DivideExpression.DivideExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> DivideExpression
                let reductionResult = DivideExpression.DivideExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
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
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = DivideExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = DivideExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = DivideExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = DivideExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = DivideExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 7
                    | 6 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 12 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = SubtractExpression.MultiplyExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
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
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = SubtractExpression.MultiplyExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = SubtractExpression.MultiplyExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus]
                keepGoing <- false
        | 13 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> MultiplyExpression
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> SubtractExpression
                let reductionResult = SubtractExpression.SubtractExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
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
                let arg3 = lhsStack.Pop() :?> MultiplyExpression
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> SubtractExpression
                let reductionResult = SubtractExpression.SubtractExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> MultiplyExpression
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> SubtractExpression
                let reductionResult = SubtractExpression.SubtractExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus]
                keepGoing <- false
        | 14 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = NumberLiteralExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 2 -> 11
                    | 5 -> 11
                    | 6 -> 11
                    | 9 -> 10
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
                    | 0 -> 11
                    | 2 -> 11
                    | 5 -> 11
                    | 6 -> 11
                    | 9 -> 10
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
                    | 0 -> 11
                    | 2 -> 11
                    | 5 -> 11
                    | 6 -> 11
                    | 9 -> 10
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
                    | 0 -> 11
                    | 2 -> 11
                    | 5 -> 11
                    | 6 -> 11
                    | 9 -> 10
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
                    | 0 -> 11
                    | 2 -> 11
                    | 5 -> 11
                    | 6 -> 11
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 15 ->
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
