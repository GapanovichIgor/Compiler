module internal rec Compiler.Parser

(*
STATES
   0 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] | AtomExpr -> ParenOpen· Expression ParenClose [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression In Expression [] }
   1 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression In Expression [] | Expression -> Let Identifier Equals· Expression In Expression [] }
   2 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression In Expression [] | Expression -> Let Identifier Equals Expression In· Expression [] }
   3 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression In Expression [] | Program -> ·Expression [] }
   4 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus· ArithmeticFirstOrderExpression [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] }
   5 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus· ArithmeticFirstOrderExpression [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] }
   6 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk· Application [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] }
   7 { Application -> ·Application AtomExpr [] | Application -> ·AtomExpr [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash· Application [] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] }
   8 { Application -> Application· AtomExpr [] | ArithmeticFirstOrderExpression -> Application· [$ Asterisk In Minus ParenClose Plus Slash] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] }
   9 { Application -> Application· AtomExpr [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application· [$ Asterisk In Minus ParenClose Plus Slash] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] }
   10 { Application -> Application· AtomExpr [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application· [$ Asterisk In Minus ParenClose Plus Slash] | AtomExpr -> ·DoubleQuotedString [] | AtomExpr -> ·Identifier [] | AtomExpr -> ·NumberLiteral [] | AtomExpr -> ·ParenOpen Expression ParenClose [] }
   11 { Application -> Application AtomExpr· [$ Asterisk DoubleQuotedString Identifier In Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   12 { Application -> AtomExpr· [$ Asterisk DoubleQuotedString Identifier In Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   13 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression· [$ In Minus ParenClose Plus] }
   14 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression· [$ In Minus ParenClose Plus] }
   15 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression· [$ In Minus ParenClose Plus] }
   16 { ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression· Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression· Plus ArithmeticFirstOrderExpression [] | Expression -> ArithmeticSecondOrderExpression· [$ In ParenClose] }
   17 { AtomExpr -> DoubleQuotedString· [$ Asterisk DoubleQuotedString Identifier In Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   18 { AtomExpr -> Identifier· [$ Asterisk DoubleQuotedString Identifier In Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   19 { AtomExpr -> NumberLiteral· [$ Asterisk DoubleQuotedString Identifier In Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   20 { AtomExpr -> ParenOpen Expression· ParenClose [] }
   21 { AtomExpr -> ParenOpen Expression ParenClose· [$ Asterisk DoubleQuotedString Identifier In Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   22 { Expression -> Let· Identifier Equals Expression In Expression [] }
   23 { Expression -> Let Identifier· Equals Expression In Expression [] }
   24 { Expression -> Let Identifier Equals Expression· In Expression [] }
   25 { Expression -> Let Identifier Equals Expression In Expression· [$ In ParenClose] }
   26 { Program -> Expression· [$] }

PRODUCTIONS
   ArithmeticFirstOrderExpression -> Application
   ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application
   ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application
   Application -> Application AtomExpr
   Application -> AtomExpr
   ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression
   ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression
   ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression
   Expression -> ArithmeticSecondOrderExpression
   AtomExpr -> DoubleQuotedString
   AtomExpr -> Identifier
   AtomExpr -> NumberLiteral
   AtomExpr -> ParenOpen Expression ParenClose
   Expression -> Let Identifier Equals Expression In Expression

ACTION
   State Lookahead          Action
   0     DoubleQuotedString shift (17)
   0     Identifier         shift (18)
   0     Let                shift (22)
   0     NumberLiteral      shift (19)
   0     ParenOpen          shift (0)
   1     DoubleQuotedString shift (17)
   1     Identifier         shift (18)
   1     Let                shift (22)
   1     NumberLiteral      shift (19)
   1     ParenOpen          shift (0)
   2     DoubleQuotedString shift (17)
   2     Identifier         shift (18)
   2     Let                shift (22)
   2     NumberLiteral      shift (19)
   2     ParenOpen          shift (0)
   3     DoubleQuotedString shift (17)
   3     Identifier         shift (18)
   3     Let                shift (22)
   3     NumberLiteral      shift (19)
   3     ParenOpen          shift (0)
   4     DoubleQuotedString shift (17)
   4     Identifier         shift (18)
   4     NumberLiteral      shift (19)
   4     ParenOpen          shift (0)
   5     DoubleQuotedString shift (17)
   5     Identifier         shift (18)
   5     NumberLiteral      shift (19)
   5     ParenOpen          shift (0)
   6     DoubleQuotedString shift (17)
   6     Identifier         shift (18)
   6     NumberLiteral      shift (19)
   6     ParenOpen          shift (0)
   7     DoubleQuotedString shift (17)
   7     Identifier         shift (18)
   7     NumberLiteral      shift (19)
   7     ParenOpen          shift (0)
   8     $                  reduce (ArithmeticFirstOrderExpression -> Application)
   8     Asterisk           reduce (ArithmeticFirstOrderExpression -> Application)
   8     DoubleQuotedString shift (17)
   8     Identifier         shift (18)
   8     In                 reduce (ArithmeticFirstOrderExpression -> Application)
   8     Minus              reduce (ArithmeticFirstOrderExpression -> Application)
   8     NumberLiteral      shift (19)
   8     ParenClose         reduce (ArithmeticFirstOrderExpression -> Application)
   8     ParenOpen          shift (0)
   8     Plus               reduce (ArithmeticFirstOrderExpression -> Application)
   8     Slash              reduce (ArithmeticFirstOrderExpression -> Application)
   9     $                  reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   9     Asterisk           reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   9     DoubleQuotedString shift (17)
   9     Identifier         shift (18)
   9     In                 reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   9     Minus              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   9     NumberLiteral      shift (19)
   9     ParenClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   9     ParenOpen          shift (0)
   9     Plus               reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   9     Slash              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   10    $                  reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   10    Asterisk           reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   10    DoubleQuotedString shift (17)
   10    Identifier         shift (18)
   10    In                 reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   10    Minus              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   10    NumberLiteral      shift (19)
   10    ParenClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   10    ParenOpen          shift (0)
   10    Plus               reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   10    Slash              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   11    $                  reduce (Application -> Application AtomExpr)
   11    Asterisk           reduce (Application -> Application AtomExpr)
   11    DoubleQuotedString reduce (Application -> Application AtomExpr)
   11    Identifier         reduce (Application -> Application AtomExpr)
   11    In                 reduce (Application -> Application AtomExpr)
   11    Minus              reduce (Application -> Application AtomExpr)
   11    NumberLiteral      reduce (Application -> Application AtomExpr)
   11    ParenClose         reduce (Application -> Application AtomExpr)
   11    ParenOpen          reduce (Application -> Application AtomExpr)
   11    Plus               reduce (Application -> Application AtomExpr)
   11    Slash              reduce (Application -> Application AtomExpr)
   12    $                  reduce (Application -> AtomExpr)
   12    Asterisk           reduce (Application -> AtomExpr)
   12    DoubleQuotedString reduce (Application -> AtomExpr)
   12    Identifier         reduce (Application -> AtomExpr)
   12    In                 reduce (Application -> AtomExpr)
   12    Minus              reduce (Application -> AtomExpr)
   12    NumberLiteral      reduce (Application -> AtomExpr)
   12    ParenClose         reduce (Application -> AtomExpr)
   12    ParenOpen          reduce (Application -> AtomExpr)
   12    Plus               reduce (Application -> AtomExpr)
   12    Slash              reduce (Application -> AtomExpr)
   13    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   13    Asterisk           shift (6)
   13    In                 reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   13    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   13    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   13    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   13    Slash              shift (7)
   14    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   14    Asterisk           shift (6)
   14    In                 reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   14    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   14    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   14    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   14    Slash              shift (7)
   15    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   15    Asterisk           shift (6)
   15    In                 reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   15    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   15    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   15    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   15    Slash              shift (7)
   16    $                  reduce (Expression -> ArithmeticSecondOrderExpression)
   16    In                 reduce (Expression -> ArithmeticSecondOrderExpression)
   16    Minus              shift (4)
   16    ParenClose         reduce (Expression -> ArithmeticSecondOrderExpression)
   16    Plus               shift (5)
   17    $                  reduce (AtomExpr -> DoubleQuotedString)
   17    Asterisk           reduce (AtomExpr -> DoubleQuotedString)
   17    DoubleQuotedString reduce (AtomExpr -> DoubleQuotedString)
   17    Identifier         reduce (AtomExpr -> DoubleQuotedString)
   17    In                 reduce (AtomExpr -> DoubleQuotedString)
   17    Minus              reduce (AtomExpr -> DoubleQuotedString)
   17    NumberLiteral      reduce (AtomExpr -> DoubleQuotedString)
   17    ParenClose         reduce (AtomExpr -> DoubleQuotedString)
   17    ParenOpen          reduce (AtomExpr -> DoubleQuotedString)
   17    Plus               reduce (AtomExpr -> DoubleQuotedString)
   17    Slash              reduce (AtomExpr -> DoubleQuotedString)
   18    $                  reduce (AtomExpr -> Identifier)
   18    Asterisk           reduce (AtomExpr -> Identifier)
   18    DoubleQuotedString reduce (AtomExpr -> Identifier)
   18    Identifier         reduce (AtomExpr -> Identifier)
   18    In                 reduce (AtomExpr -> Identifier)
   18    Minus              reduce (AtomExpr -> Identifier)
   18    NumberLiteral      reduce (AtomExpr -> Identifier)
   18    ParenClose         reduce (AtomExpr -> Identifier)
   18    ParenOpen          reduce (AtomExpr -> Identifier)
   18    Plus               reduce (AtomExpr -> Identifier)
   18    Slash              reduce (AtomExpr -> Identifier)
   19    $                  reduce (AtomExpr -> NumberLiteral)
   19    Asterisk           reduce (AtomExpr -> NumberLiteral)
   19    DoubleQuotedString reduce (AtomExpr -> NumberLiteral)
   19    Identifier         reduce (AtomExpr -> NumberLiteral)
   19    In                 reduce (AtomExpr -> NumberLiteral)
   19    Minus              reduce (AtomExpr -> NumberLiteral)
   19    NumberLiteral      reduce (AtomExpr -> NumberLiteral)
   19    ParenClose         reduce (AtomExpr -> NumberLiteral)
   19    ParenOpen          reduce (AtomExpr -> NumberLiteral)
   19    Plus               reduce (AtomExpr -> NumberLiteral)
   19    Slash              reduce (AtomExpr -> NumberLiteral)
   20    ParenClose         shift (21)
   21    $                  reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    Asterisk           reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    DoubleQuotedString reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    Identifier         reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    In                 reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    Minus              reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    NumberLiteral      reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    ParenClose         reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    ParenOpen          reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    Plus               reduce (AtomExpr -> ParenOpen Expression ParenClose)
   21    Slash              reduce (AtomExpr -> ParenOpen Expression ParenClose)
   22    Identifier         shift (23)
   23    Equals             shift (1)
   24    In                 shift (2)
   25    $                  reduce (Expression -> Let Identifier Equals Expression In Expression)
   25    In                 reduce (Expression -> Let Identifier Equals Expression In Expression)
   25    ParenClose         reduce (Expression -> Let Identifier Equals Expression In Expression)
   26    $                  accept

GOTO
   Source state Symbol                          Destination state
   0            Application                     8
   0            ArithmeticFirstOrderExpression  13
   0            ArithmeticSecondOrderExpression 16
   0            AtomExpr                        12
   0            Expression                      20
   1            Application                     8
   1            ArithmeticFirstOrderExpression  13
   1            ArithmeticSecondOrderExpression 16
   1            AtomExpr                        12
   1            Expression                      24
   2            Application                     8
   2            ArithmeticFirstOrderExpression  13
   2            ArithmeticSecondOrderExpression 16
   2            AtomExpr                        12
   2            Expression                      25
   3            Application                     8
   3            ArithmeticFirstOrderExpression  13
   3            ArithmeticSecondOrderExpression 16
   3            AtomExpr                        12
   3            Expression                      26
   4            Application                     8
   4            ArithmeticFirstOrderExpression  14
   4            AtomExpr                        12
   5            Application                     8
   5            ArithmeticFirstOrderExpression  15
   5            AtomExpr                        12
   6            Application                     9
   6            AtomExpr                        12
   7            Application                     10
   7            AtomExpr                        12
   8            AtomExpr                        11
   9            AtomExpr                        11
   10           AtomExpr                        11

*)

type Asterisk = unit
type DoubleQuotedString = string
type Equals = unit
type Identifier = string
type In = unit
type Let = unit
type Minus = unit
type NumberLiteral = int * int option
type ParenClose = unit
type ParenOpen = unit
type Plus = unit
type Slash = unit

type Application =
    | Application of Application * AtomExpr
    | Fallthrough of AtomExpr

type ArithmeticFirstOrderExpression =
    | Divide of ArithmeticFirstOrderExpression * Slash * Application
    | Fallthrough of Application
    | Multiply of ArithmeticFirstOrderExpression * Asterisk * Application

type ArithmeticSecondOrderExpression =
    | Add of ArithmeticSecondOrderExpression * Plus * ArithmeticFirstOrderExpression
    | Fallthrough of ArithmeticFirstOrderExpression
    | Subtract of ArithmeticSecondOrderExpression * Minus * ArithmeticFirstOrderExpression

type AtomExpr =
    | DoubleQuotedString of DoubleQuotedString
    | Identifier of Identifier
    | Number of NumberLiteral
    | Paren of ParenOpen * Expression * ParenClose

type Expression =
    | LetIn of Let * Identifier * Equals * Expression * In * Expression
    | Value of ArithmeticSecondOrderExpression

type Program =
    | Program of Expression

type InputItem =
    | Asterisk of Asterisk
    | DoubleQuotedString of DoubleQuotedString
    | Equals of Equals
    | Identifier of Identifier
    | In of In
    | Let of Let
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
    | Equals
    | Identifier
    | In
    | Let
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

    stateStack.Push(3)

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
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
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
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
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
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
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
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
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
                expected <- [ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
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
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
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
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
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
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
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
        | 7 ->
            match lookahead with
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
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
        | 8 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
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
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 9 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
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
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Asterisk
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Multiply (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 10 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(17)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(18)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
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
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> Application
                let arg2 = lhsStack.Pop() :?> Slash
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticFirstOrderExpression.Divide (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 14
                    | 5 -> 15
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 11 ->
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpr
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 12 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpr
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
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
                    | 0 -> 8
                    | 1 -> 8
                    | 2 -> 8
                    | 3 -> 8
                    | 4 -> 8
                    | 5 -> 8
                    | 6 -> 9
                    | 7 -> 10
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 13 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
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
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(7)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 14 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
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
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Minus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Subtract (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(7)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 15 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
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
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let arg2 = lhsStack.Pop() :?> Plus
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Add (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 16
                    | 1 -> 16
                    | 2 -> 16
                    | 3 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(7)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 16 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = Expression.Value arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 20
                    | 1 -> 24
                    | 2 -> 25
                    | 3 -> 26
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = Expression.Value arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 20
                    | 1 -> 24
                    | 2 -> 25
                    | 3 -> 26
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(4)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = Expression.Value arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 20
                    | 1 -> 24
                    | 2 -> 25
                    | 3 -> 26
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(5)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus]
                keepGoing <- false
        | 17 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpr.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 18 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpr.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 19 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpr.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
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
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 20 ->
            match lookahead with
            | InputItem.ParenClose x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | _ ->
                // error
                expected <- [ExpectedItem.ParenClose]
                keepGoing <- false
        | 21 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = AtomExpr.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 12
                    | 1 -> 12
                    | 2 -> 12
                    | 3 -> 12
                    | 4 -> 12
                    | 5 -> 12
                    | 6 -> 12
                    | 7 -> 12
                    | 8 -> 11
                    | 9 -> 11
                    | 10 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.In; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash]
                keepGoing <- false
        | 22 ->
            match lookahead with
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
            | _ ->
                // error
                expected <- [ExpectedItem.Identifier]
                keepGoing <- false
        | 23 ->
            match lookahead with
            | InputItem.Equals x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | _ ->
                // error
                expected <- [ExpectedItem.Equals]
                keepGoing <- false
        | 24 ->
            match lookahead with
            | InputItem.In x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ExpectedItem.In]
                keepGoing <- false
        | 25 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg6 = lhsStack.Pop() :?> Expression
                let arg5 = lhsStack.Pop() :?> In
                let arg4 = lhsStack.Pop() :?> Expression
                let arg3 = lhsStack.Pop() :?> Equals
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = Expression.LetIn (arg1, arg2, arg3, arg4, arg5, arg6)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 20
                    | 1 -> 24
                    | 2 -> 25
                    | 3 -> 26
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.In _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg6 = lhsStack.Pop() :?> Expression
                let arg5 = lhsStack.Pop() :?> In
                let arg4 = lhsStack.Pop() :?> Expression
                let arg3 = lhsStack.Pop() :?> Equals
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = Expression.LetIn (arg1, arg2, arg3, arg4, arg5, arg6)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 20
                    | 1 -> 24
                    | 2 -> 25
                    | 3 -> 26
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg6 = lhsStack.Pop() :?> Expression
                let arg5 = lhsStack.Pop() :?> In
                let arg4 = lhsStack.Pop() :?> Expression
                let arg3 = lhsStack.Pop() :?> Equals
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = Expression.LetIn (arg1, arg2, arg3, arg4, arg5, arg6)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 20
                    | 1 -> 24
                    | 2 -> 25
                    | 3 -> 26
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.In; ExpectedItem.ParenClose]
                keepGoing <- false
        | 26 ->
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
