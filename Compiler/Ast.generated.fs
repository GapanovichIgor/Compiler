module internal rec Compiler.AstGenerated

(*
STATES
   0 { AddExpression -> ·AddExpression Plus MultiplyExpression [] | AddExpression -> ·MultiplyExpression [] | Expression -> ·AddExpression [] | MultiplyExpression -> ·MultiplyExpression Asterisk NumberLiteralExpression [] | MultiplyExpression -> ·NumberLiteralExpression [] | NumberLiteralExpression -> ·NumberLiteral [] | Program -> ·Expression [] }
   1 { AddExpression -> AddExpression· Plus MultiplyExpression [] | Expression -> AddExpression· [$] }
   2 { AddExpression -> AddExpression Plus· MultiplyExpression [] | MultiplyExpression -> ·MultiplyExpression Asterisk NumberLiteralExpression [] | MultiplyExpression -> ·NumberLiteralExpression [] | NumberLiteralExpression -> ·NumberLiteral [] }
   3 { AddExpression -> AddExpression Plus MultiplyExpression· [$ Plus] | MultiplyExpression -> MultiplyExpression· Asterisk NumberLiteralExpression [] }
   4 { AddExpression -> MultiplyExpression· [$ Plus] | MultiplyExpression -> MultiplyExpression· Asterisk NumberLiteralExpression [] }
   5 { MultiplyExpression -> MultiplyExpression Asterisk· NumberLiteralExpression [] | NumberLiteralExpression -> ·NumberLiteral [] }
   6 { MultiplyExpression -> MultiplyExpression Asterisk NumberLiteralExpression· [$ Asterisk Plus] }
   7 { MultiplyExpression -> NumberLiteralExpression· [$ Asterisk Plus] }
   8 { NumberLiteralExpression -> NumberLiteral· [$ Asterisk Plus] }
   9 { Program -> Expression· [$] }

PRODUCTIONS
   Expression -> AddExpression
   AddExpression -> AddExpression Plus MultiplyExpression
   AddExpression -> MultiplyExpression
   MultiplyExpression -> MultiplyExpression Asterisk NumberLiteralExpression
   MultiplyExpression -> NumberLiteralExpression
   NumberLiteralExpression -> NumberLiteral

ACTION
   State Lookahead     Action
   0     NumberLiteral shift (8)
   1     $             reduce (Expression -> AddExpression)
   1     Plus          shift (2)
   2     NumberLiteral shift (8)
   3     $             reduce (AddExpression -> AddExpression Plus MultiplyExpression)
   3     Asterisk      shift (5)
   3     Plus          reduce (AddExpression -> AddExpression Plus MultiplyExpression)
   4     $             reduce (AddExpression -> MultiplyExpression)
   4     Asterisk      shift (5)
   4     Plus          reduce (AddExpression -> MultiplyExpression)
   5     NumberLiteral shift (8)
   6     $             reduce (MultiplyExpression -> MultiplyExpression Asterisk NumberLiteralExpression)
   6     Asterisk      reduce (MultiplyExpression -> MultiplyExpression Asterisk NumberLiteralExpression)
   6     Plus          reduce (MultiplyExpression -> MultiplyExpression Asterisk NumberLiteralExpression)
   7     $             reduce (MultiplyExpression -> NumberLiteralExpression)
   7     Asterisk      reduce (MultiplyExpression -> NumberLiteralExpression)
   7     Plus          reduce (MultiplyExpression -> NumberLiteralExpression)
   8     $             reduce (NumberLiteralExpression -> NumberLiteral)
   8     Asterisk      reduce (NumberLiteralExpression -> NumberLiteral)
   8     Plus          reduce (NumberLiteralExpression -> NumberLiteral)
   9     $             accept

GOTO
   Source state Symbol                  Destination state
   0            AddExpression           1
   0            Expression              9
   0            MultiplyExpression      4
   0            NumberLiteralExpression 7
   2            MultiplyExpression      3
   2            NumberLiteralExpression 7
   5            NumberLiteralExpression 6

*)

type Asterisk = unit
type NumberLiteral = int * int option
type Plus = unit

type AddExpression =
    | AddExpression of AddExpression * Plus * MultiplyExpression
    | MultiplyExpression of MultiplyExpression

type Expression =
    | Expression of AddExpression

type MultiplyExpression =
    | MultiplyExpression of MultiplyExpression * Asterisk * NumberLiteralExpression
    | NumberLiteralExpression of NumberLiteralExpression

type NumberLiteralExpression =
    | NumberLiteralExpression of NumberLiteral

type Program =
    | Program of Expression

type InputItem =
    | Asterisk of Asterisk
    | NumberLiteral of NumberLiteral
    | Plus of Plus

type Unexpected =
    | EndOfStream
    | InputItem of InputItem

type ExpectedItem =
    | EndOfStream
    | Asterisk
    | NumberLiteral
    | Plus

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
                stateStack.Push(8)
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
                    | 0 -> 9
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
                stateStack.Push(8)
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
                let arg3 = lhsStack.Pop() :?> MultiplyExpression
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> AddExpression
                let reductionResult = AddExpression.AddExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
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
                let arg3 = lhsStack.Pop() :?> MultiplyExpression
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
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Plus]
                keepGoing <- false
        | 4 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = AddExpression.MultiplyExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
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
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = AddExpression.MultiplyExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Plus]
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
                stateStack.Push(8)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral]
                keepGoing <- false
        | 6 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = MultiplyExpression.MultiplyExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = MultiplyExpression.MultiplyExpression (arg1, arg2, arg3)
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
                let arg3 = lhsStack.Pop() :?> NumberLiteralExpression
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> MultiplyExpression
                let reductionResult = MultiplyExpression.MultiplyExpression (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Plus]
                keepGoing <- false
        | 7 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = MultiplyExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = MultiplyExpression.NumberLiteralExpression arg1
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
                let arg1 = lhsStack.Pop() :?> NumberLiteralExpression
                let reductionResult = MultiplyExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 2 -> 3
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Plus]
                keepGoing <- false
        | 8 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = NumberLiteralExpression.NumberLiteralExpression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 6
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
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 6
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
                    | 0 -> 7
                    | 2 -> 7
                    | 5 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Plus]
                keepGoing <- false
        | 9 ->
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
