module internal rec Compiler.AstGenerated

(*
STATES
   0 { ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Asterisk AtomExpr [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Slash AtomExpr [] | ArithmeticFirstOrderExpr -> ·AtomExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] | AtomExpr -> ParenOpen· Expr ParenClose [] | Expr -> ·ArithmeticSecondOrderExpr [] }
   1 { ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Asterisk AtomExpr [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Slash AtomExpr [] | ArithmeticFirstOrderExpr -> ·AtomExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] | Expr -> ·ArithmeticSecondOrderExpr [] | Program -> ·Expr [] }
   2 { ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Asterisk AtomExpr [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Slash AtomExpr [] | ArithmeticFirstOrderExpr -> ·AtomExpr [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus· ArithmeticFirstOrderExpr [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   3 { ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Asterisk AtomExpr [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Slash AtomExpr [] | ArithmeticFirstOrderExpr -> ·AtomExpr [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus· ArithmeticFirstOrderExpr [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   4 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Asterisk AtomExpr [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Slash AtomExpr [] | ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr· [$ Minus ParenClose Plus] }
   5 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Asterisk AtomExpr [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Slash AtomExpr [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr· [$ Minus ParenClose Plus] }
   6 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Asterisk AtomExpr [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Slash AtomExpr [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr· [$ Minus ParenClose Plus] }
   7 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk· AtomExpr [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   8 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk AtomExpr· [$ Asterisk Minus ParenClose Plus Slash] }
   9 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash· AtomExpr [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   10 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash AtomExpr· [$ Asterisk Minus ParenClose Plus Slash] }
   11 { ArithmeticFirstOrderExpr -> AtomExpr· [$ Asterisk Minus ParenClose Plus Slash] }
   12 { ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr· Minus ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr· Plus ArithmeticFirstOrderExpr [] | Expr -> ArithmeticSecondOrderExpr· [$ ParenClose] }
   13 { AtomExpr -> NumberLiteral· [$ Asterisk Minus ParenClose Plus Slash] }
   14 { AtomExpr -> ParenOpen Expr· ParenClose [] }
   15 { AtomExpr -> ParenOpen Expr ParenClose· [$ Asterisk Minus ParenClose Plus Slash] }
   16 { Program -> Expr· [$] }

PRODUCTIONS
   ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr
   ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr
   ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr
   ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk AtomExpr
   ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash AtomExpr
   ArithmeticFirstOrderExpr -> AtomExpr
   Expr -> ArithmeticSecondOrderExpr
   AtomExpr -> NumberLiteral
   AtomExpr -> ParenOpen Expr ParenClose

ACTION
   State Lookahead     Action
   0     NumberLiteral shift (13)
   0     ParenOpen     shift (0)
   1     NumberLiteral shift (13)
   1     ParenOpen     shift (0)
   2     NumberLiteral shift (13)
   2     ParenOpen     shift (0)
   3     NumberLiteral shift (13)
   3     ParenOpen     shift (0)
   4     $             reduce (ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr)
   4     Asterisk      shift (7)
   4     Minus         reduce (ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr)
   4     ParenClose    reduce (ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr)
   4     Plus          reduce (ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr)
   4     Slash         shift (9)
   5     $             reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr)
   5     Asterisk      shift (7)
   5     Minus         reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr)
   5     ParenClose    reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr)
   5     Plus          reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr)
   5     Slash         shift (9)
   6     $             reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr)
   6     Asterisk      shift (7)
   6     Minus         reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr)
   6     ParenClose    reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr)
   6     Plus          reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr)
   6     Slash         shift (9)
   7     NumberLiteral shift (13)
   7     ParenOpen     shift (0)
   8     $             reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk AtomExpr)
   8     Asterisk      reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk AtomExpr)
   8     Minus         reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk AtomExpr)
   8     ParenClose    reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk AtomExpr)
   8     Plus          reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk AtomExpr)
   8     Slash         reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk AtomExpr)
   9     NumberLiteral shift (13)
   9     ParenOpen     shift (0)
   10    $             reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash AtomExpr)
   10    Asterisk      reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash AtomExpr)
   10    Minus         reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash AtomExpr)
   10    ParenClose    reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash AtomExpr)
   10    Plus          reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash AtomExpr)
   10    Slash         reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash AtomExpr)
   11    $             reduce (ArithmeticFirstOrderExpr -> AtomExpr)
   11    Asterisk      reduce (ArithmeticFirstOrderExpr -> AtomExpr)
   11    Minus         reduce (ArithmeticFirstOrderExpr -> AtomExpr)
   11    ParenClose    reduce (ArithmeticFirstOrderExpr -> AtomExpr)
   11    Plus          reduce (ArithmeticFirstOrderExpr -> AtomExpr)
   11    Slash         reduce (ArithmeticFirstOrderExpr -> AtomExpr)
   12    $             reduce (Expr -> ArithmeticSecondOrderExpr)
   12    Minus         shift (2)
   12    ParenClose    reduce (Expr -> ArithmeticSecondOrderExpr)
   12    Plus          shift (3)
   13    $             reduce (AtomExpr -> NumberLiteral)
   13    Asterisk      reduce (AtomExpr -> NumberLiteral)
   13    Minus         reduce (AtomExpr -> NumberLiteral)
   13    ParenClose    reduce (AtomExpr -> NumberLiteral)
   13    Plus          reduce (AtomExpr -> NumberLiteral)
   13    Slash         reduce (AtomExpr -> NumberLiteral)
   14    ParenClose    shift (15)
   15    $             reduce (AtomExpr -> ParenOpen Expr ParenClose)
   15    Asterisk      reduce (AtomExpr -> ParenOpen Expr ParenClose)
   15    Minus         reduce (AtomExpr -> ParenOpen Expr ParenClose)
   15    ParenClose    reduce (AtomExpr -> ParenOpen Expr ParenClose)
   15    Plus          reduce (AtomExpr -> ParenOpen Expr ParenClose)
   15    Slash         reduce (AtomExpr -> ParenOpen Expr ParenClose)
   16    $             accept

GOTO
   Source state Symbol                    Destination state
   0            ArithmeticFirstOrderExpr  4
   0            ArithmeticSecondOrderExpr 12
   0            AtomExpr                  11
   0            Expr                      14
   1            ArithmeticFirstOrderExpr  4
   1            ArithmeticSecondOrderExpr 12
   1            AtomExpr                  11
   1            Expr                      16
   2            ArithmeticFirstOrderExpr  5
   2            AtomExpr                  11
   3            ArithmeticFirstOrderExpr  6
   3            AtomExpr                  11
   7            AtomExpr                  8
   9            AtomExpr                  10

*)

type Asterisk = unit
type Minus = unit
type NumberLiteral = int * int option
type ParenClose = unit
type ParenOpen = unit
type Plus = unit
type Slash = unit

type ArithmeticFirstOrderExpr =
    | Divide of ArithmeticFirstOrderExpr * Slash * AtomExpr
    | Fallthrough of AtomExpr
    | Multiply of ArithmeticFirstOrderExpr * Asterisk * AtomExpr

type ArithmeticSecondOrderExpr =
    | Add of ArithmeticSecondOrderExpr * Plus * ArithmeticFirstOrderExpr
    | Fallthrough of ArithmeticFirstOrderExpr
    | Subtract of ArithmeticSecondOrderExpr * Minus * ArithmeticFirstOrderExpr

type AtomExpr =
    | Number of NumberLiteral
    | Paren of ParenOpen * Expr * ParenClose

type Expr =
    | Expr of ArithmeticSecondOrderExpr

type Program =
    | Program of Expr

type InputItem =
    | Asterisk of Asterisk
    | Minus of Minus
    | NumberLiteral of NumberLiteral
    | ParenClose of ParenClose
    | ParenOpen of ParenOpen
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
    | ParenClose
    | ParenOpen
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

    stateStack.Push(1)

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
                stateStack.Push(13)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
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
                stateStack.Push(13)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
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
                stateStack.Push(13)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 3 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(13)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 4 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(7)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
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
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 5 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(7)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
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
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 6 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(7)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
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
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 7 ->
            match lookahead with
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(13)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 8 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
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
                stateStack.Push(13)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | _ ->
                // error
                expected <- [ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 10 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> AtomExpr
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 11 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 4
                    | 1 -> 4
                    | 2 -> 5
                    | 3 -> 6
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 12 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = Expr.Expr arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = Expr.Expr arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(3)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus]
                keepGoing <- false
        | 13 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 14 ->
            match lookahead with
            | InputItem.ParenClose x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | _ ->
                // error
                expected <- [ExpectedItem.ParenClose]
                keepGoing <- false
        | 15 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expr
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expr
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expr
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expr
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expr
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expr
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 11
                    | 3 -> 11
                    | 7 -> 8
                    | 9 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 16 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // accept
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Expr
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
