module internal rec Compiler.AstGenerated

(*
STATES
   0 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpr -> ·Application [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Asterisk Application [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Slash Application [] | ArithmeticSecondOrderExpr -> ·ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] | AtomExpr -> ParenOpen· Expr ParenClose [] | Expr -> ·ArithmeticSecondOrderExpr [] }
   1 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpr -> ·Application [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Asterisk Application [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Slash Application [] | ArithmeticSecondOrderExpr -> ·ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ·ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] | Expr -> ·ArithmeticSecondOrderExpr [] | Program -> ·Expr [] }
   2 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpr -> ·Application [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Asterisk Application [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Slash Application [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus· ArithmeticFirstOrderExpr [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   3 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpr -> ·Application [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Asterisk Application [] | ArithmeticFirstOrderExpr -> ·ArithmeticFirstOrderExpr Slash Application [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus· ArithmeticFirstOrderExpr [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   4 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk· Application [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   5 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash· Application [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   6 { Application -> Application· AtomExpr [] | ArithmeticFirstOrderExpr -> Application· [$ Asterisk Minus ParenClose Plus Slash] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   7 { Application -> Application· AtomExpr [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk Application· [$ Asterisk Minus ParenClose Plus Slash] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   8 { Application -> Application· AtomExpr [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash Application· [$ Asterisk Minus ParenClose Plus Slash] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expr ParenClose [] }
   9 { Application -> Application AtomExpr· [$ Asterisk DoubleQuotedString Identifier Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   10 { Application -> AtomExpr· [$ Asterisk DoubleQuotedString Identifier Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   11 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Asterisk Application [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Slash Application [] | ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr· [$ Minus ParenClose Plus] }
   12 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Asterisk Application [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Slash Application [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr· [$ Minus ParenClose Plus] }
   13 { ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Asterisk Application [] | ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr· Slash Application [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr· [$ Minus ParenClose Plus] }
   14 { ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr· Minus ArithmeticFirstOrderExpr [] | ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr· Plus ArithmeticFirstOrderExpr [] | Expr -> ArithmeticSecondOrderExpr· [$ ParenClose] }
   15 { AtomExpr -> DoubleQuotedString· [$ Asterisk DoubleQuotedString Identifier Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   16 { AtomExpr -> Identifier· [$ Asterisk DoubleQuotedString Identifier Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   17 { AtomExpr -> NumberLiteral· [$ Asterisk DoubleQuotedString Identifier Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   18 { AtomExpr -> ParenOpen Expr· ParenClose [] }
   19 { AtomExpr -> ParenOpen Expr ParenClose· [$ Asterisk DoubleQuotedString Identifier Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   20 { Program -> Expr· [$] }

PRODUCTIONS
   ArithmeticFirstOrderExpr -> Application
   ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk Application
   ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash Application
   Application -> Application AtomExpr
   Application -> AtomExpr
   ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr
   ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr
   ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr
   Expr -> ArithmeticSecondOrderExpr
   AtomExpr -> DoubleQuotedString
   AtomExpr -> Identifier
   AtomExpr -> NumberLiteral
   AtomExpr -> ParenOpen Expr ParenClose

ACTION
   State Lookahead          Action
   0     DoubleQuotedString shift (15)
   0     Identifier         shift (16)
   0     NumberLiteral      shift (17)
   0     ParenOpen          shift (0)
   1     DoubleQuotedString shift (15)
   1     Identifier         shift (16)
   1     NumberLiteral      shift (17)
   1     ParenOpen          shift (0)
   2     DoubleQuotedString shift (15)
   2     Identifier         shift (16)
   2     NumberLiteral      shift (17)
   2     ParenOpen          shift (0)
   3     DoubleQuotedString shift (15)
   3     Identifier         shift (16)
   3     NumberLiteral      shift (17)
   3     ParenOpen          shift (0)
   4     DoubleQuotedString shift (15)
   4     Identifier         shift (16)
   4     NumberLiteral      shift (17)
   4     ParenOpen          shift (0)
   5     DoubleQuotedString shift (15)
   5     Identifier         shift (16)
   5     NumberLiteral      shift (17)
   5     ParenOpen          shift (0)
   6     $                  reduce (ArithmeticFirstOrderExpr -> Application)
   6     Asterisk           reduce (ArithmeticFirstOrderExpr -> Application)
   6     DoubleQuotedString shift (15)
   6     Identifier         shift (16)
   6     Minus              reduce (ArithmeticFirstOrderExpr -> Application)
   6     NumberLiteral      shift (17)
   6     ParenClose         reduce (ArithmeticFirstOrderExpr -> Application)
   6     ParenOpen          shift (0)
   6     Plus               reduce (ArithmeticFirstOrderExpr -> Application)
   6     Slash              reduce (ArithmeticFirstOrderExpr -> Application)
   7     $                  reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk Application)
   7     Asterisk           reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk Application)
   7     DoubleQuotedString shift (15)
   7     Identifier         shift (16)
   7     Minus              reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk Application)
   7     NumberLiteral      shift (17)
   7     ParenClose         reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk Application)
   7     ParenOpen          shift (0)
   7     Plus               reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk Application)
   7     Slash              reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Asterisk Application)
   8     $                  reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash Application)
   8     Asterisk           reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash Application)
   8     DoubleQuotedString shift (15)
   8     Identifier         shift (16)
   8     Minus              reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash Application)
   8     NumberLiteral      shift (17)
   8     ParenClose         reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash Application)
   8     ParenOpen          shift (0)
   8     Plus               reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash Application)
   8     Slash              reduce (ArithmeticFirstOrderExpr -> ArithmeticFirstOrderExpr Slash Application)
   9     $                  reduce (Application -> Application AtomExpr)
   9     Asterisk           reduce (Application -> Application AtomExpr)
   9     DoubleQuotedString reduce (Application -> Application AtomExpr)
   9     Identifier         reduce (Application -> Application AtomExpr)
   9     Minus              reduce (Application -> Application AtomExpr)
   9     NumberLiteral      reduce (Application -> Application AtomExpr)
   9     ParenClose         reduce (Application -> Application AtomExpr)
   9     ParenOpen          reduce (Application -> Application AtomExpr)
   9     Plus               reduce (Application -> Application AtomExpr)
   9     Slash              reduce (Application -> Application AtomExpr)
   10    $                  reduce (Application -> AtomExpr)
   10    Asterisk           reduce (Application -> AtomExpr)
   10    DoubleQuotedString reduce (Application -> AtomExpr)
   10    Identifier         reduce (Application -> AtomExpr)
   10    Minus              reduce (Application -> AtomExpr)
   10    NumberLiteral      reduce (Application -> AtomExpr)
   10    ParenClose         reduce (Application -> AtomExpr)
   10    ParenOpen          reduce (Application -> AtomExpr)
   10    Plus               reduce (Application -> AtomExpr)
   10    Slash              reduce (Application -> AtomExpr)
   11    $                  reduce (ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr)
   11    Asterisk           shift (4)
   11    Minus              reduce (ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr)
   11    ParenClose         reduce (ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr)
   11    Plus               reduce (ArithmeticSecondOrderExpr -> ArithmeticFirstOrderExpr)
   11    Slash              shift (5)
   12    $                  reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr)
   12    Asterisk           shift (4)
   12    Minus              reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr)
   12    ParenClose         reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr)
   12    Plus               reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Minus ArithmeticFirstOrderExpr)
   12    Slash              shift (5)
   13    $                  reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr)
   13    Asterisk           shift (4)
   13    Minus              reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr)
   13    ParenClose         reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr)
   13    Plus               reduce (ArithmeticSecondOrderExpr -> ArithmeticSecondOrderExpr Plus ArithmeticFirstOrderExpr)
   13    Slash              shift (5)
   14    $                  reduce (Expr -> ArithmeticSecondOrderExpr)
   14    Minus              shift (2)
   14    ParenClose         reduce (Expr -> ArithmeticSecondOrderExpr)
   14    Plus               shift (3)
   15    $                  reduce (AtomExpr -> DoubleQuotedString)
   15    Asterisk           reduce (AtomExpr -> DoubleQuotedString)
   15    DoubleQuotedString reduce (AtomExpr -> DoubleQuotedString)
   15    Identifier         reduce (AtomExpr -> DoubleQuotedString)
   15    Minus              reduce (AtomExpr -> DoubleQuotedString)
   15    NumberLiteral      reduce (AtomExpr -> DoubleQuotedString)
   15    ParenClose         reduce (AtomExpr -> DoubleQuotedString)
   15    ParenOpen          reduce (AtomExpr -> DoubleQuotedString)
   15    Plus               reduce (AtomExpr -> DoubleQuotedString)
   15    Slash              reduce (AtomExpr -> DoubleQuotedString)
   16    $                  reduce (AtomExpr -> Identifier)
   16    Asterisk           reduce (AtomExpr -> Identifier)
   16    DoubleQuotedString reduce (AtomExpr -> Identifier)
   16    Identifier         reduce (AtomExpr -> Identifier)
   16    Minus              reduce (AtomExpr -> Identifier)
   16    NumberLiteral      reduce (AtomExpr -> Identifier)
   16    ParenClose         reduce (AtomExpr -> Identifier)
   16    ParenOpen          reduce (AtomExpr -> Identifier)
   16    Plus               reduce (AtomExpr -> Identifier)
   16    Slash              reduce (AtomExpr -> Identifier)
   17    $                  reduce (AtomExpr -> NumberLiteral)
   17    Asterisk           reduce (AtomExpr -> NumberLiteral)
   17    DoubleQuotedString reduce (AtomExpr -> NumberLiteral)
   17    Identifier         reduce (AtomExpr -> NumberLiteral)
   17    Minus              reduce (AtomExpr -> NumberLiteral)
   17    NumberLiteral      reduce (AtomExpr -> NumberLiteral)
   17    ParenClose         reduce (AtomExpr -> NumberLiteral)
   17    ParenOpen          reduce (AtomExpr -> NumberLiteral)
   17    Plus               reduce (AtomExpr -> NumberLiteral)
   17    Slash              reduce (AtomExpr -> NumberLiteral)
   18    ParenClose         shift (19)
   19    $                  reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    Asterisk           reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    DoubleQuotedString reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    Identifier         reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    Minus              reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    NumberLiteral      reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    ParenClose         reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    ParenOpen          reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    Plus               reduce (AtomExpr -> ParenOpen Expr ParenClose)
   19    Slash              reduce (AtomExpr -> ParenOpen Expr ParenClose)
   20    $                  accept

GOTO
   Source state Symbol                    Destination state
   0            Application               6
   0            ArithmeticFirstOrderExpr  11
   0            ArithmeticSecondOrderExpr 14
   0            AtomExpr                  10
   0            Expr                      18
   1            Application               6
   1            ArithmeticFirstOrderExpr  11
   1            ArithmeticSecondOrderExpr 14
   1            AtomExpr                  10
   1            Expr                      20
   2            Application               6
   2            ArithmeticFirstOrderExpr  12
   2            AtomExpr                  10
   3            Application               6
   3            ArithmeticFirstOrderExpr  13
   3            AtomExpr                  10
   4            Application               7
   4            AtomExpr                  10
   5            Application               8
   5            AtomExpr                  10
   6            AtomExpr                  9
   7            AtomExpr                  9
   8            AtomExpr                  9

*)

type Asterisk = unit
type DoubleQuotedString = string
type Identifier = string
type Minus = unit
type NumberLiteral = int * int option
type ParenClose = unit
type ParenOpen = unit
type Plus = unit
type Slash = unit

type Application =
    | Application of Application * AtomExpr
    | Fallthrough of AtomExpr

type ArithmeticFirstOrderExpr =
    | Divide of ArithmeticFirstOrderExpr * Slash * Application
    | Fallthrough of Application
    | Multiply of ArithmeticFirstOrderExpr * Asterisk * Application

type ArithmeticSecondOrderExpr =
    | Add of ArithmeticSecondOrderExpr * Plus * ArithmeticFirstOrderExpr
    | Fallthrough of ArithmeticFirstOrderExpr
    | Subtract of ArithmeticSecondOrderExpr * Minus * ArithmeticFirstOrderExpr

type AtomExpr =
    | DoubleQuotedString of DoubleQuotedString
    | Identifier of Identifier
    | Number of NumberLiteral
    | Paren of ParenOpen * Expr * ParenClose

type Expr =
    | Expr of ArithmeticSecondOrderExpr

type Program =
    | Program of Expr

type InputItem =
    | Asterisk of Asterisk
    | DoubleQuotedString of DoubleQuotedString
    | Identifier of Identifier
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
    | DoubleQuotedString
    | Identifier
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
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 1 ->
            match lookahead with
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 2 ->
            match lookahead with
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 3 ->
            match lookahead with
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 4 ->
            match lookahead with
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 5 ->
            match lookahead with
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 6 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 7 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 8 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(15)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(16)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticFirstOrderExpr.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 11
                    | 1 -> 11
                    | 2 -> 12
                    | 3 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 9 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 10 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 6
                    | 1 -> 6
                    | 2 -> 6
                    | 3 -> 6
                    | 4 -> 7
                    | 5 -> 8
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 11 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(4)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpr
                let reductionResult = ArithmeticSecondOrderExpr.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
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
                    | 0 -> 14
                    | 1 -> 14
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
                    | 0 -> 14
                    | 1 -> 14
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(5)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 12 ->
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
                    | 0 -> 14
                    | 1 -> 14
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(4)
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
                    | 0 -> 14
                    | 1 -> 14
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
                    | 0 -> 14
                    | 1 -> 14
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
                    | 0 -> 14
                    | 1 -> 14
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(5)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 13 ->
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
                    | 0 -> 14
                    | 1 -> 14
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(4)
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
                    | 0 -> 14
                    | 1 -> 14
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
                    | 0 -> 14
                    | 1 -> 14
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
                    | 0 -> 14
                    | 1 -> 14
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(5)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 14 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpr
                let reductionResult = Expr.Expr arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 18
                    | 1 -> 20
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
                    | 0 -> 18
                    | 1 -> 20
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
        | 15 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 16 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 17 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 18 ->
            match lookahead with
            | InputItem.ParenClose x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
            | _ ->
                // error
                expected <- [ExpectedItem.ParenClose]
                keepGoing <- false
        | 19 ->
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
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
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 9
                    | 7 -> 9
                    | 8 -> 9
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 20 ->
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
