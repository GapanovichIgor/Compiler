module internal rec Compiler.Parser

(*
STATES
   0 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> BlockOpen· Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression BindingBody [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   1 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | AtomExpression -> ParenOpening· Expression ParenClosing [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression BindingBody [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   2 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | BindingBody -> NewLine· Expression [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression BindingBody [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   3 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | BindingBody -> Semicolon· Expression [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression BindingBody [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   4 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression BindingBody [] | Expression -> Let Identifier Equals· Expression BindingBody [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   5 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | Expression -> ·ArithmeticSecondOrderExpression [] | Expression -> ·Let Identifier Equals Expression BindingBody [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] | Program -> ·Expression [] }
   6 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus· ArithmeticFirstOrderExpression [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   7 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus· ArithmeticFirstOrderExpression [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   8 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk· Application [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   9 { Application -> ·Application AtomExpression [] | Application -> ·AtomExpression [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash· Application [] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   10 { Application -> Application· AtomExpression [] | ArithmeticFirstOrderExpression -> Application· [$ Asterisk BlockClose Minus NewLine ParenClose Plus Semicolon Slash] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   11 { Application -> Application· AtomExpression [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application· [$ Asterisk BlockClose Minus NewLine ParenClose Plus Semicolon Slash] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   12 { Application -> Application· AtomExpression [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application· [$ Asterisk BlockClose Minus NewLine ParenClose Plus Semicolon Slash] | AtomExpression -> ·BlockOpen Expression BlockClose [] | AtomExpression -> ·DoubleQuotedString [] | AtomExpression -> ·Identifier [] | AtomExpression -> ·NumberLiteral [] | AtomExpression -> ·ParenOpening Expression ParenClosing [] | ParenOpening -> ·ParenOpen [] | ParenOpening -> ·ParenOpen NewLine [] }
   13 { Application -> Application AtomExpression· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   14 { Application -> AtomExpression· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   15 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression· [$ BlockClose Minus NewLine ParenClose Plus Semicolon] }
   16 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression· [$ BlockClose Minus NewLine ParenClose Plus Semicolon] }
   17 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression· [$ BlockClose Minus NewLine ParenClose Plus Semicolon] }
   18 { ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression· Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression· Plus ArithmeticFirstOrderExpression [] | Expression -> ArithmeticSecondOrderExpression· [$ BlockClose NewLine ParenClose Semicolon] }
   19 { AtomExpression -> BlockOpen Expression· BlockClose [] }
   20 { AtomExpression -> BlockOpen Expression BlockClose· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   21 { AtomExpression -> DoubleQuotedString· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   22 { AtomExpression -> Identifier· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   23 { AtomExpression -> NumberLiteral· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   24 { AtomExpression -> ParenOpening Expression· ParenClosing [] | ParenClosing -> ·NewLine ParenClose [] | ParenClosing -> ·ParenClose [] }
   25 { AtomExpression -> ParenOpening Expression ParenClosing· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   26 { BindingBody -> ·NewLine Expression [] | BindingBody -> ·Semicolon Expression [] | Expression -> Let Identifier Equals Expression· BindingBody [] }
   27 { BindingBody -> NewLine Expression· [$ BlockClose NewLine ParenClose Semicolon] }
   28 { BindingBody -> Semicolon Expression· [$ BlockClose NewLine ParenClose Semicolon] }
   29 { Expression -> Let· Identifier Equals Expression BindingBody [] }
   30 { Expression -> Let Identifier· Equals Expression BindingBody [] }
   31 { Expression -> Let Identifier Equals Expression BindingBody· [$ BlockClose NewLine ParenClose Semicolon] }
   32 { ParenClosing -> NewLine· ParenClose [] }
   33 { ParenClosing -> NewLine ParenClose· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   34 { ParenClosing -> ParenClose· [$ Asterisk BlockClose BlockOpen DoubleQuotedString Identifier Minus NewLine NumberLiteral ParenClose ParenOpen Plus Semicolon Slash] }
   35 { ParenOpening -> ParenOpen· [BlockOpen DoubleQuotedString Identifier Let NumberLiteral ParenOpen] | ParenOpening -> ParenOpen· NewLine [] }
   36 { ParenOpening -> ParenOpen NewLine· [BlockOpen DoubleQuotedString Identifier Let NumberLiteral ParenOpen] }
   37 { Program -> Expression· [$] }

PRODUCTIONS
   ArithmeticFirstOrderExpression -> Application
   ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application
   ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application
   Application -> Application AtomExpression
   Application -> AtomExpression
   ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression
   ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression
   ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression
   Expression -> ArithmeticSecondOrderExpression
   AtomExpression -> BlockOpen Expression BlockClose
   AtomExpression -> DoubleQuotedString
   AtomExpression -> Identifier
   AtomExpression -> NumberLiteral
   AtomExpression -> ParenOpening Expression ParenClosing
   BindingBody -> NewLine Expression
   BindingBody -> Semicolon Expression
   Expression -> Let Identifier Equals Expression BindingBody
   ParenClosing -> NewLine ParenClose
   ParenClosing -> ParenClose
   ParenOpening -> ParenOpen
   ParenOpening -> ParenOpen NewLine

ACTION
   State Lookahead          Action
   0     BlockOpen          shift (0)
   0     DoubleQuotedString shift (21)
   0     Identifier         shift (22)
   0     Let                shift (29)
   0     NumberLiteral      shift (23)
   0     ParenOpen          shift (35)
   1     BlockOpen          shift (0)
   1     DoubleQuotedString shift (21)
   1     Identifier         shift (22)
   1     Let                shift (29)
   1     NumberLiteral      shift (23)
   1     ParenOpen          shift (35)
   2     BlockOpen          shift (0)
   2     DoubleQuotedString shift (21)
   2     Identifier         shift (22)
   2     Let                shift (29)
   2     NumberLiteral      shift (23)
   2     ParenOpen          shift (35)
   3     BlockOpen          shift (0)
   3     DoubleQuotedString shift (21)
   3     Identifier         shift (22)
   3     Let                shift (29)
   3     NumberLiteral      shift (23)
   3     ParenOpen          shift (35)
   4     BlockOpen          shift (0)
   4     DoubleQuotedString shift (21)
   4     Identifier         shift (22)
   4     Let                shift (29)
   4     NumberLiteral      shift (23)
   4     ParenOpen          shift (35)
   5     BlockOpen          shift (0)
   5     DoubleQuotedString shift (21)
   5     Identifier         shift (22)
   5     Let                shift (29)
   5     NumberLiteral      shift (23)
   5     ParenOpen          shift (35)
   6     BlockOpen          shift (0)
   6     DoubleQuotedString shift (21)
   6     Identifier         shift (22)
   6     NumberLiteral      shift (23)
   6     ParenOpen          shift (35)
   7     BlockOpen          shift (0)
   7     DoubleQuotedString shift (21)
   7     Identifier         shift (22)
   7     NumberLiteral      shift (23)
   7     ParenOpen          shift (35)
   8     BlockOpen          shift (0)
   8     DoubleQuotedString shift (21)
   8     Identifier         shift (22)
   8     NumberLiteral      shift (23)
   8     ParenOpen          shift (35)
   9     BlockOpen          shift (0)
   9     DoubleQuotedString shift (21)
   9     Identifier         shift (22)
   9     NumberLiteral      shift (23)
   9     ParenOpen          shift (35)
   10    $                  reduce (ArithmeticFirstOrderExpression -> Application)
   10    Asterisk           reduce (ArithmeticFirstOrderExpression -> Application)
   10    BlockClose         reduce (ArithmeticFirstOrderExpression -> Application)
   10    BlockOpen          shift (0)
   10    DoubleQuotedString shift (21)
   10    Identifier         shift (22)
   10    Minus              reduce (ArithmeticFirstOrderExpression -> Application)
   10    NewLine            reduce (ArithmeticFirstOrderExpression -> Application)
   10    NumberLiteral      shift (23)
   10    ParenClose         reduce (ArithmeticFirstOrderExpression -> Application)
   10    ParenOpen          shift (35)
   10    Plus               reduce (ArithmeticFirstOrderExpression -> Application)
   10    Semicolon          reduce (ArithmeticFirstOrderExpression -> Application)
   10    Slash              reduce (ArithmeticFirstOrderExpression -> Application)
   11    $                  reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    Asterisk           reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    BlockClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    BlockOpen          shift (0)
   11    DoubleQuotedString shift (21)
   11    Identifier         shift (22)
   11    Minus              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    NewLine            reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    NumberLiteral      shift (23)
   11    ParenClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    ParenOpen          shift (35)
   11    Plus               reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    Semicolon          reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    Slash              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   12    $                  reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    Asterisk           reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    BlockClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    BlockOpen          shift (0)
   12    DoubleQuotedString shift (21)
   12    Identifier         shift (22)
   12    Minus              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    NewLine            reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    NumberLiteral      shift (23)
   12    ParenClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    ParenOpen          shift (35)
   12    Plus               reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    Semicolon          reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    Slash              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   13    $                  reduce (Application -> Application AtomExpression)
   13    Asterisk           reduce (Application -> Application AtomExpression)
   13    BlockClose         reduce (Application -> Application AtomExpression)
   13    BlockOpen          reduce (Application -> Application AtomExpression)
   13    DoubleQuotedString reduce (Application -> Application AtomExpression)
   13    Identifier         reduce (Application -> Application AtomExpression)
   13    Minus              reduce (Application -> Application AtomExpression)
   13    NewLine            reduce (Application -> Application AtomExpression)
   13    NumberLiteral      reduce (Application -> Application AtomExpression)
   13    ParenClose         reduce (Application -> Application AtomExpression)
   13    ParenOpen          reduce (Application -> Application AtomExpression)
   13    Plus               reduce (Application -> Application AtomExpression)
   13    Semicolon          reduce (Application -> Application AtomExpression)
   13    Slash              reduce (Application -> Application AtomExpression)
   14    $                  reduce (Application -> AtomExpression)
   14    Asterisk           reduce (Application -> AtomExpression)
   14    BlockClose         reduce (Application -> AtomExpression)
   14    BlockOpen          reduce (Application -> AtomExpression)
   14    DoubleQuotedString reduce (Application -> AtomExpression)
   14    Identifier         reduce (Application -> AtomExpression)
   14    Minus              reduce (Application -> AtomExpression)
   14    NewLine            reduce (Application -> AtomExpression)
   14    NumberLiteral      reduce (Application -> AtomExpression)
   14    ParenClose         reduce (Application -> AtomExpression)
   14    ParenOpen          reduce (Application -> AtomExpression)
   14    Plus               reduce (Application -> AtomExpression)
   14    Semicolon          reduce (Application -> AtomExpression)
   14    Slash              reduce (Application -> AtomExpression)
   15    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   15    Asterisk           shift (8)
   15    BlockClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   15    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   15    NewLine            reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   15    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   15    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   15    Semicolon          reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   15    Slash              shift (9)
   16    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   16    Asterisk           shift (8)
   16    BlockClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   16    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   16    NewLine            reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   16    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   16    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   16    Semicolon          reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   16    Slash              shift (9)
   17    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   17    Asterisk           shift (8)
   17    BlockClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   17    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   17    NewLine            reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   17    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   17    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   17    Semicolon          reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   17    Slash              shift (9)
   18    $                  reduce (Expression -> ArithmeticSecondOrderExpression)
   18    BlockClose         reduce (Expression -> ArithmeticSecondOrderExpression)
   18    Minus              shift (6)
   18    NewLine            reduce (Expression -> ArithmeticSecondOrderExpression)
   18    ParenClose         reduce (Expression -> ArithmeticSecondOrderExpression)
   18    Plus               shift (7)
   18    Semicolon          reduce (Expression -> ArithmeticSecondOrderExpression)
   19    BlockClose         shift (20)
   20    $                  reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    Asterisk           reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    BlockClose         reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    BlockOpen          reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    DoubleQuotedString reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    Identifier         reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    Minus              reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    NewLine            reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    NumberLiteral      reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    ParenClose         reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    ParenOpen          reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    Plus               reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    Semicolon          reduce (AtomExpression -> BlockOpen Expression BlockClose)
   20    Slash              reduce (AtomExpression -> BlockOpen Expression BlockClose)
   21    $                  reduce (AtomExpression -> DoubleQuotedString)
   21    Asterisk           reduce (AtomExpression -> DoubleQuotedString)
   21    BlockClose         reduce (AtomExpression -> DoubleQuotedString)
   21    BlockOpen          reduce (AtomExpression -> DoubleQuotedString)
   21    DoubleQuotedString reduce (AtomExpression -> DoubleQuotedString)
   21    Identifier         reduce (AtomExpression -> DoubleQuotedString)
   21    Minus              reduce (AtomExpression -> DoubleQuotedString)
   21    NewLine            reduce (AtomExpression -> DoubleQuotedString)
   21    NumberLiteral      reduce (AtomExpression -> DoubleQuotedString)
   21    ParenClose         reduce (AtomExpression -> DoubleQuotedString)
   21    ParenOpen          reduce (AtomExpression -> DoubleQuotedString)
   21    Plus               reduce (AtomExpression -> DoubleQuotedString)
   21    Semicolon          reduce (AtomExpression -> DoubleQuotedString)
   21    Slash              reduce (AtomExpression -> DoubleQuotedString)
   22    $                  reduce (AtomExpression -> Identifier)
   22    Asterisk           reduce (AtomExpression -> Identifier)
   22    BlockClose         reduce (AtomExpression -> Identifier)
   22    BlockOpen          reduce (AtomExpression -> Identifier)
   22    DoubleQuotedString reduce (AtomExpression -> Identifier)
   22    Identifier         reduce (AtomExpression -> Identifier)
   22    Minus              reduce (AtomExpression -> Identifier)
   22    NewLine            reduce (AtomExpression -> Identifier)
   22    NumberLiteral      reduce (AtomExpression -> Identifier)
   22    ParenClose         reduce (AtomExpression -> Identifier)
   22    ParenOpen          reduce (AtomExpression -> Identifier)
   22    Plus               reduce (AtomExpression -> Identifier)
   22    Semicolon          reduce (AtomExpression -> Identifier)
   22    Slash              reduce (AtomExpression -> Identifier)
   23    $                  reduce (AtomExpression -> NumberLiteral)
   23    Asterisk           reduce (AtomExpression -> NumberLiteral)
   23    BlockClose         reduce (AtomExpression -> NumberLiteral)
   23    BlockOpen          reduce (AtomExpression -> NumberLiteral)
   23    DoubleQuotedString reduce (AtomExpression -> NumberLiteral)
   23    Identifier         reduce (AtomExpression -> NumberLiteral)
   23    Minus              reduce (AtomExpression -> NumberLiteral)
   23    NewLine            reduce (AtomExpression -> NumberLiteral)
   23    NumberLiteral      reduce (AtomExpression -> NumberLiteral)
   23    ParenClose         reduce (AtomExpression -> NumberLiteral)
   23    ParenOpen          reduce (AtomExpression -> NumberLiteral)
   23    Plus               reduce (AtomExpression -> NumberLiteral)
   23    Semicolon          reduce (AtomExpression -> NumberLiteral)
   23    Slash              reduce (AtomExpression -> NumberLiteral)
   24    NewLine            shift (32)
   24    ParenClose         shift (34)
   25    $                  reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    Asterisk           reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    BlockClose         reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    BlockOpen          reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    DoubleQuotedString reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    Identifier         reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    Minus              reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    NewLine            reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    NumberLiteral      reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    ParenClose         reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    ParenOpen          reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    Plus               reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    Semicolon          reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   25    Slash              reduce (AtomExpression -> ParenOpening Expression ParenClosing)
   26    NewLine            shift (2)
   26    Semicolon          shift (3)
   27    $                  reduce (BindingBody -> NewLine Expression)
   27    BlockClose         reduce (BindingBody -> NewLine Expression)
   27    NewLine            reduce (BindingBody -> NewLine Expression)
   27    ParenClose         reduce (BindingBody -> NewLine Expression)
   27    Semicolon          reduce (BindingBody -> NewLine Expression)
   28    $                  reduce (BindingBody -> Semicolon Expression)
   28    BlockClose         reduce (BindingBody -> Semicolon Expression)
   28    NewLine            reduce (BindingBody -> Semicolon Expression)
   28    ParenClose         reduce (BindingBody -> Semicolon Expression)
   28    Semicolon          reduce (BindingBody -> Semicolon Expression)
   29    Identifier         shift (30)
   30    Equals             shift (4)
   31    $                  reduce (Expression -> Let Identifier Equals Expression BindingBody)
   31    BlockClose         reduce (Expression -> Let Identifier Equals Expression BindingBody)
   31    NewLine            reduce (Expression -> Let Identifier Equals Expression BindingBody)
   31    ParenClose         reduce (Expression -> Let Identifier Equals Expression BindingBody)
   31    Semicolon          reduce (Expression -> Let Identifier Equals Expression BindingBody)
   32    ParenClose         shift (33)
   33    $                  reduce (ParenClosing -> NewLine ParenClose)
   33    Asterisk           reduce (ParenClosing -> NewLine ParenClose)
   33    BlockClose         reduce (ParenClosing -> NewLine ParenClose)
   33    BlockOpen          reduce (ParenClosing -> NewLine ParenClose)
   33    DoubleQuotedString reduce (ParenClosing -> NewLine ParenClose)
   33    Identifier         reduce (ParenClosing -> NewLine ParenClose)
   33    Minus              reduce (ParenClosing -> NewLine ParenClose)
   33    NewLine            reduce (ParenClosing -> NewLine ParenClose)
   33    NumberLiteral      reduce (ParenClosing -> NewLine ParenClose)
   33    ParenClose         reduce (ParenClosing -> NewLine ParenClose)
   33    ParenOpen          reduce (ParenClosing -> NewLine ParenClose)
   33    Plus               reduce (ParenClosing -> NewLine ParenClose)
   33    Semicolon          reduce (ParenClosing -> NewLine ParenClose)
   33    Slash              reduce (ParenClosing -> NewLine ParenClose)
   34    $                  reduce (ParenClosing -> ParenClose)
   34    Asterisk           reduce (ParenClosing -> ParenClose)
   34    BlockClose         reduce (ParenClosing -> ParenClose)
   34    BlockOpen          reduce (ParenClosing -> ParenClose)
   34    DoubleQuotedString reduce (ParenClosing -> ParenClose)
   34    Identifier         reduce (ParenClosing -> ParenClose)
   34    Minus              reduce (ParenClosing -> ParenClose)
   34    NewLine            reduce (ParenClosing -> ParenClose)
   34    NumberLiteral      reduce (ParenClosing -> ParenClose)
   34    ParenClose         reduce (ParenClosing -> ParenClose)
   34    ParenOpen          reduce (ParenClosing -> ParenClose)
   34    Plus               reduce (ParenClosing -> ParenClose)
   34    Semicolon          reduce (ParenClosing -> ParenClose)
   34    Slash              reduce (ParenClosing -> ParenClose)
   35    BlockOpen          reduce (ParenOpening -> ParenOpen)
   35    DoubleQuotedString reduce (ParenOpening -> ParenOpen)
   35    Identifier         reduce (ParenOpening -> ParenOpen)
   35    Let                reduce (ParenOpening -> ParenOpen)
   35    NewLine            shift (36)
   35    NumberLiteral      reduce (ParenOpening -> ParenOpen)
   35    ParenOpen          reduce (ParenOpening -> ParenOpen)
   36    BlockOpen          reduce (ParenOpening -> ParenOpen NewLine)
   36    DoubleQuotedString reduce (ParenOpening -> ParenOpen NewLine)
   36    Identifier         reduce (ParenOpening -> ParenOpen NewLine)
   36    Let                reduce (ParenOpening -> ParenOpen NewLine)
   36    NumberLiteral      reduce (ParenOpening -> ParenOpen NewLine)
   36    ParenOpen          reduce (ParenOpening -> ParenOpen NewLine)
   37    $                  accept

GOTO
   Source state Symbol                          Destination state
   0            Application                     10
   0            ArithmeticFirstOrderExpression  15
   0            ArithmeticSecondOrderExpression 18
   0            AtomExpression                  14
   0            Expression                      19
   0            ParenOpening                    1
   1            Application                     10
   1            ArithmeticFirstOrderExpression  15
   1            ArithmeticSecondOrderExpression 18
   1            AtomExpression                  14
   1            Expression                      24
   1            ParenOpening                    1
   2            Application                     10
   2            ArithmeticFirstOrderExpression  15
   2            ArithmeticSecondOrderExpression 18
   2            AtomExpression                  14
   2            Expression                      27
   2            ParenOpening                    1
   3            Application                     10
   3            ArithmeticFirstOrderExpression  15
   3            ArithmeticSecondOrderExpression 18
   3            AtomExpression                  14
   3            Expression                      28
   3            ParenOpening                    1
   4            Application                     10
   4            ArithmeticFirstOrderExpression  15
   4            ArithmeticSecondOrderExpression 18
   4            AtomExpression                  14
   4            Expression                      26
   4            ParenOpening                    1
   5            Application                     10
   5            ArithmeticFirstOrderExpression  15
   5            ArithmeticSecondOrderExpression 18
   5            AtomExpression                  14
   5            Expression                      37
   5            ParenOpening                    1
   6            Application                     10
   6            ArithmeticFirstOrderExpression  16
   6            AtomExpression                  14
   6            ParenOpening                    1
   7            Application                     10
   7            ArithmeticFirstOrderExpression  17
   7            AtomExpression                  14
   7            ParenOpening                    1
   8            Application                     11
   8            AtomExpression                  14
   8            ParenOpening                    1
   9            Application                     12
   9            AtomExpression                  14
   9            ParenOpening                    1
   10           AtomExpression                  13
   10           ParenOpening                    1
   11           AtomExpression                  13
   11           ParenOpening                    1
   12           AtomExpression                  13
   12           ParenOpening                    1
   24           ParenClosing                    25
   26           BindingBody                     31

*)

type Asterisk = unit
type BlockClose = unit
type BlockOpen = unit
type DoubleQuotedString = string
type Equals = unit
type Identifier = string
type InvalidToken = string
type Let = unit
type Minus = unit
type NewLine = unit
type NumberLiteral = int * int option
type ParenClose = unit
type ParenOpen = unit
type Plus = unit
type Semicolon = unit
type Slash = unit

type Application =
    | Application of Application * AtomExpression
    | Fallthrough of AtomExpression

type ArithmeticFirstOrderExpression =
    | Divide of ArithmeticFirstOrderExpression * Slash * Application
    | Fallthrough of Application
    | Multiply of ArithmeticFirstOrderExpression * Asterisk * Application

type ArithmeticSecondOrderExpression =
    | Add of ArithmeticSecondOrderExpression * Plus * ArithmeticFirstOrderExpression
    | Fallthrough of ArithmeticFirstOrderExpression
    | Subtract of ArithmeticSecondOrderExpression * Minus * ArithmeticFirstOrderExpression

type AtomExpression =
    | Block of BlockOpen * Expression * BlockClose
    | DoubleQuotedString of DoubleQuotedString
    | Identifier of Identifier
    | Number of NumberLiteral
    | Paren of ParenOpening * Expression * ParenClosing

type BindingBody =
    | NewLine of NewLine * Expression
    | Semicolon of Semicolon * Expression

type Expression =
    | Binding of Let * Identifier * Equals * Expression * BindingBody
    | Value of ArithmeticSecondOrderExpression

type ParenClosing =
    | Simple of ParenClose
    | WithNewLine of NewLine * ParenClose

type ParenOpening =
    | Simple of ParenOpen
    | WithNewLine of ParenOpen * NewLine

type Program =
    | Program of Expression

type InputItem =
    | Asterisk of Asterisk
    | BlockClose of BlockClose
    | BlockOpen of BlockOpen
    | DoubleQuotedString of DoubleQuotedString
    | Equals of Equals
    | Identifier of Identifier
    | InvalidToken of InvalidToken
    | Let of Let
    | Minus of Minus
    | NewLine of NewLine
    | NumberLiteral of NumberLiteral
    | ParenClose of ParenClose
    | ParenOpen of ParenOpen
    | Plus of Plus
    | Semicolon of Semicolon
    | Slash of Slash

type Unexpected =
    | EndOfStream
    | InputItem of InputItem

type ExpectedItem =
    | EndOfStream
    | Asterisk
    | BlockClose
    | BlockOpen
    | DoubleQuotedString
    | Equals
    | Identifier
    | InvalidToken
    | Let
    | Minus
    | NewLine
    | NumberLiteral
    | ParenClose
    | ParenOpen
    | Plus
    | Semicolon
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

    stateStack.Push(5)

    let mutable lookahead, lookaheadIsEof =
        if inputEnumerator.MoveNext()
        then (inputEnumerator.Current, false)
        else (Unchecked.defaultof<InputItem>, true)

    let mutable keepGoing = true
    while keepGoing do
        match stateStack.Peek() with
        | 0 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 1 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 2 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 3 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 4 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 5 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 6 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
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
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 7 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
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
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 8 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
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
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 9 ->
            match lookahead with
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
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
                stateStack.Push(23)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 10 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 11 ->
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 12 ->
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(0)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(21)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(23)
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(35)
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
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
                    | 0 -> 15
                    | 1 -> 15
                    | 2 -> 15
                    | 3 -> 15
                    | 4 -> 15
                    | 5 -> 15
                    | 6 -> 16
                    | 7 -> 17
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 13 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> AtomExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 14 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> AtomExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 10
                    | 1 -> 10
                    | 2 -> 10
                    | 3 -> 10
                    | 4 -> 10
                    | 5 -> 10
                    | 6 -> 10
                    | 7 -> 10
                    | 8 -> 11
                    | 9 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 15 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(8)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 16 ->
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(8)
            | InputItem.BlockClose _ ->
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 17 ->
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(8)
            | InputItem.BlockClose _ ->
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
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
                    | 0 -> 18
                    | 1 -> 18
                    | 2 -> 18
                    | 3 -> 18
                    | 4 -> 18
                    | 5 -> 18
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
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 18 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = Expression.Value arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = Expression.Value arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(6)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = Expression.Value arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = Expression.Value arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(7)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = Expression.Value arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Semicolon]
                keepGoing <- false
        | 19 ->
            match lookahead with
            | InputItem.BlockClose x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(20)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockClose]
                keepGoing <- false
        | 20 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = AtomExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 21 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = AtomExpression.DoubleQuotedString arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 22 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = AtomExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 23 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = AtomExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 24 ->
            match lookahead with
            | InputItem.NewLine x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenClose x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(34)
            | _ ->
                // error
                expected <- [ExpectedItem.NewLine; ExpectedItem.ParenClose]
                keepGoing <- false
        | 25 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClosing
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpening
                let reductionResult = AtomExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 14
                    | 6 -> 14
                    | 7 -> 14
                    | 8 -> 14
                    | 9 -> 14
                    | 10 -> 13
                    | 11 -> 13
                    | 12 -> 13
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 26 ->
            match lookahead with
            | InputItem.NewLine x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | InputItem.Semicolon x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(3)
            | _ ->
                // error
                expected <- [ExpectedItem.NewLine; ExpectedItem.Semicolon]
                keepGoing <- false
        | 27 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = BindingBody.NewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = BindingBody.NewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = BindingBody.NewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = BindingBody.NewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = BindingBody.NewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.NewLine; ExpectedItem.ParenClose; ExpectedItem.Semicolon]
                keepGoing <- false
        | 28 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> Semicolon
                let reductionResult = BindingBody.Semicolon (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> Semicolon
                let reductionResult = BindingBody.Semicolon (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> Semicolon
                let reductionResult = BindingBody.Semicolon (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> Semicolon
                let reductionResult = BindingBody.Semicolon (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> Semicolon
                let reductionResult = BindingBody.Semicolon (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 26 -> 31
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.NewLine; ExpectedItem.ParenClose; ExpectedItem.Semicolon]
                keepGoing <- false
        | 29 ->
            match lookahead with
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | _ ->
                // error
                expected <- [ExpectedItem.Identifier]
                keepGoing <- false
        | 30 ->
            match lookahead with
            | InputItem.Equals x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(4)
            | _ ->
                // error
                expected <- [ExpectedItem.Equals]
                keepGoing <- false
        | 31 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> BindingBody
                let arg4 = lhsStack.Pop() :?> Expression
                let arg3 = lhsStack.Pop() :?> Equals
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = Expression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> BindingBody
                let arg4 = lhsStack.Pop() :?> Expression
                let arg3 = lhsStack.Pop() :?> Equals
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = Expression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> BindingBody
                let arg4 = lhsStack.Pop() :?> Expression
                let arg3 = lhsStack.Pop() :?> Equals
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = Expression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> BindingBody
                let arg4 = lhsStack.Pop() :?> Expression
                let arg3 = lhsStack.Pop() :?> Equals
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = Expression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> BindingBody
                let arg4 = lhsStack.Pop() :?> Expression
                let arg3 = lhsStack.Pop() :?> Equals
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = Expression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 19
                    | 1 -> 24
                    | 2 -> 27
                    | 3 -> 28
                    | 4 -> 26
                    | 5 -> 37
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.NewLine; ExpectedItem.ParenClose; ExpectedItem.Semicolon]
                keepGoing <- false
        | 32 ->
            match lookahead with
            | InputItem.ParenClose x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(33)
            | _ ->
                // error
                expected <- [ExpectedItem.ParenClose]
                keepGoing <- false
        | 33 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> ParenClose
                let arg1 = lhsStack.Pop() :?> NewLine
                let reductionResult = ParenClosing.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 34 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Semicolon _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenClose
                let reductionResult = ParenClosing.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 24 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Minus; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Semicolon; ExpectedItem.Slash]
                keepGoing <- false
        | 35 ->
            match lookahead with
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Let _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NewLine x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(36)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.Simple arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NewLine; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 36 ->
            match lookahead with
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> NewLine
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> NewLine
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> NewLine
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Let _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> NewLine
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> NewLine
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> NewLine
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = ParenOpening.WithNewLine (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 1
                    | 1 -> 1
                    | 2 -> 1
                    | 3 -> 1
                    | 4 -> 1
                    | 5 -> 1
                    | 6 -> 1
                    | 7 -> 1
                    | 8 -> 1
                    | 9 -> 1
                    | 10 -> 1
                    | 11 -> 1
                    | 12 -> 1
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen]
                keepGoing <- false
        | 37 ->
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
