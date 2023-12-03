module internal rec Compiler.Parser

(*
STATES
   0 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | BindingExpression -> ·ArithmeticSecondOrderExpression [] | BindingExpression -> ·Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression [] | Expression -> ·ExpressionConcatenation [] | ExpressionConcatenation -> ·BindingExpression [] | ExpressionConcatenation -> ·ExpressionConcatenation Break BindingExpression [] | Program -> ·Expression [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   1 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | BindingExpression -> ·ArithmeticSecondOrderExpression [] | BindingExpression -> ·Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression [] | Expression -> ·ExpressionConcatenation [] | ExpressionConcatenation -> ·BindingExpression [] | ExpressionConcatenation -> ·ExpressionConcatenation Break BindingExpression [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> BlockOpen· Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   2 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | BindingExpression -> ·ArithmeticSecondOrderExpression [] | BindingExpression -> ·Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression [] | Expression -> ·ExpressionConcatenation [] | ExpressionConcatenation -> ·BindingExpression [] | ExpressionConcatenation -> ·ExpressionConcatenation Break BindingExpression [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] | TerminalEnclosedExpression -> ParenOpen· Expression ParenClose [] }
   3 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | BindingExpression -> ·ArithmeticSecondOrderExpression [] | BindingExpression -> ·Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression [] | ExpressionConcatenation -> ExpressionConcatenation Break· BindingExpression [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   4 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ·ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ·ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression [] | BindingExpression -> Let Identifier BindingParameters Equals· ArithmeticSecondOrderExpression [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   5 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus· ArithmeticFirstOrderExpression [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   6 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ·Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Asterisk Application [] | ArithmeticFirstOrderExpression -> ·ArithmeticFirstOrderExpression Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus· ArithmeticFirstOrderExpression [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   7 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk· Application [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   8 { Application -> ·Application TerminalEnclosedExpression [] | Application -> ·TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash· Application [] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   9 { Application -> Application· TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> Application· [$ Asterisk BlockClose Break Minus ParenClose Plus Slash] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   10 { Application -> Application· TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application· [$ Asterisk BlockClose Break Minus ParenClose Plus Slash] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   11 { Application -> Application· TerminalEnclosedExpression [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application· [$ Asterisk BlockClose Break Minus ParenClose Plus Slash] | TerminalEnclosedExpression -> ·BlockOpen Expression BlockClose [] | TerminalEnclosedExpression -> ·DoubleQuotedString [] | TerminalEnclosedExpression -> ·Identifier [] | TerminalEnclosedExpression -> ·InvalidToken [] | TerminalEnclosedExpression -> ·NumberLiteral [] | TerminalEnclosedExpression -> ·ParenOpen Expression ParenClose [] }
   12 { Application -> Application TerminalEnclosedExpression· [$ Asterisk BlockClose BlockOpen Break DoubleQuotedString Identifier InvalidToken Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   13 { Application -> TerminalEnclosedExpression· [$ Asterisk BlockClose BlockOpen Break DoubleQuotedString Identifier InvalidToken Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   14 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression· [$ BlockClose Break Minus ParenClose Plus] }
   15 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression· [$ BlockClose Break Minus ParenClose Plus] }
   16 { ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Asterisk Application [] | ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression· Slash Application [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression· [$ BlockClose Break Minus ParenClose Plus] }
   17 { ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression· Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression· Plus ArithmeticFirstOrderExpression [] | BindingExpression -> ArithmeticSecondOrderExpression· [$ BlockClose Break ParenClose] }
   18 { ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression· Minus ArithmeticFirstOrderExpression [] | ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression· Plus ArithmeticFirstOrderExpression [] | BindingExpression -> Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression· [$ BlockClose Break ParenClose] }
   19 { BindingExpression -> Let· Identifier BindingParameters Equals ArithmeticSecondOrderExpression [] }
   20 { BindingExpression -> Let Identifier· BindingParameters Equals ArithmeticSecondOrderExpression [] | BindingParameters -> · [Equals Identifier] | BindingParameters -> ·BindingParameters Identifier [] }
   21 { BindingExpression -> Let Identifier BindingParameters· Equals ArithmeticSecondOrderExpression [] | BindingParameters -> BindingParameters· Identifier [] }
   22 { BindingParameters -> BindingParameters Identifier· [Equals Identifier] }
   23 { Expression -> ExpressionConcatenation· [$ BlockClose ParenClose] | ExpressionConcatenation -> ExpressionConcatenation· Break BindingExpression [] }
   24 { ExpressionConcatenation -> BindingExpression· [$ BlockClose Break ParenClose] }
   25 { ExpressionConcatenation -> ExpressionConcatenation Break BindingExpression· [$ BlockClose Break ParenClose] }
   26 { Program -> Expression· [$] }
   27 { TerminalEnclosedExpression -> BlockOpen Expression· BlockClose [] }
   28 { TerminalEnclosedExpression -> BlockOpen Expression BlockClose· [$ Asterisk BlockClose BlockOpen Break DoubleQuotedString Identifier InvalidToken Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   29 { TerminalEnclosedExpression -> DoubleQuotedString· [$ Asterisk BlockClose BlockOpen Break DoubleQuotedString Identifier InvalidToken Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   30 { TerminalEnclosedExpression -> Identifier· [$ Asterisk BlockClose BlockOpen Break DoubleQuotedString Identifier InvalidToken Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   31 { TerminalEnclosedExpression -> InvalidToken· [$ Asterisk BlockClose BlockOpen Break DoubleQuotedString Identifier InvalidToken Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   32 { TerminalEnclosedExpression -> NumberLiteral· [$ Asterisk BlockClose BlockOpen Break DoubleQuotedString Identifier InvalidToken Minus NumberLiteral ParenClose ParenOpen Plus Slash] }
   33 { TerminalEnclosedExpression -> ParenOpen Expression· ParenClose [] }
   34 { TerminalEnclosedExpression -> ParenOpen Expression ParenClose· [$ Asterisk BlockClose BlockOpen Break DoubleQuotedString Identifier InvalidToken Minus NumberLiteral ParenClose ParenOpen Plus Slash] }

PRODUCTIONS
   ArithmeticFirstOrderExpression -> Application
   ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application
   ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application
   Application -> Application TerminalEnclosedExpression
   Application -> TerminalEnclosedExpression
   ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression
   ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression
   ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression
   BindingExpression -> ArithmeticSecondOrderExpression
   BindingExpression -> Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression
   BindingParameters -> 
   BindingParameters -> BindingParameters Identifier
   Expression -> ExpressionConcatenation
   ExpressionConcatenation -> BindingExpression
   ExpressionConcatenation -> ExpressionConcatenation Break BindingExpression
   TerminalEnclosedExpression -> BlockOpen Expression BlockClose
   TerminalEnclosedExpression -> DoubleQuotedString
   TerminalEnclosedExpression -> Identifier
   TerminalEnclosedExpression -> InvalidToken
   TerminalEnclosedExpression -> NumberLiteral
   TerminalEnclosedExpression -> ParenOpen Expression ParenClose

ACTION
   State Lookahead          Action
   0     BlockOpen          shift (1)
   0     DoubleQuotedString shift (29)
   0     Identifier         shift (30)
   0     InvalidToken       shift (31)
   0     Let                shift (19)
   0     NumberLiteral      shift (32)
   0     ParenOpen          shift (2)
   1     BlockOpen          shift (1)
   1     DoubleQuotedString shift (29)
   1     Identifier         shift (30)
   1     InvalidToken       shift (31)
   1     Let                shift (19)
   1     NumberLiteral      shift (32)
   1     ParenOpen          shift (2)
   2     BlockOpen          shift (1)
   2     DoubleQuotedString shift (29)
   2     Identifier         shift (30)
   2     InvalidToken       shift (31)
   2     Let                shift (19)
   2     NumberLiteral      shift (32)
   2     ParenOpen          shift (2)
   3     BlockOpen          shift (1)
   3     DoubleQuotedString shift (29)
   3     Identifier         shift (30)
   3     InvalidToken       shift (31)
   3     Let                shift (19)
   3     NumberLiteral      shift (32)
   3     ParenOpen          shift (2)
   4     BlockOpen          shift (1)
   4     DoubleQuotedString shift (29)
   4     Identifier         shift (30)
   4     InvalidToken       shift (31)
   4     NumberLiteral      shift (32)
   4     ParenOpen          shift (2)
   5     BlockOpen          shift (1)
   5     DoubleQuotedString shift (29)
   5     Identifier         shift (30)
   5     InvalidToken       shift (31)
   5     NumberLiteral      shift (32)
   5     ParenOpen          shift (2)
   6     BlockOpen          shift (1)
   6     DoubleQuotedString shift (29)
   6     Identifier         shift (30)
   6     InvalidToken       shift (31)
   6     NumberLiteral      shift (32)
   6     ParenOpen          shift (2)
   7     BlockOpen          shift (1)
   7     DoubleQuotedString shift (29)
   7     Identifier         shift (30)
   7     InvalidToken       shift (31)
   7     NumberLiteral      shift (32)
   7     ParenOpen          shift (2)
   8     BlockOpen          shift (1)
   8     DoubleQuotedString shift (29)
   8     Identifier         shift (30)
   8     InvalidToken       shift (31)
   8     NumberLiteral      shift (32)
   8     ParenOpen          shift (2)
   9     $                  reduce (ArithmeticFirstOrderExpression -> Application)
   9     Asterisk           reduce (ArithmeticFirstOrderExpression -> Application)
   9     BlockClose         reduce (ArithmeticFirstOrderExpression -> Application)
   9     BlockOpen          shift (1)
   9     Break              reduce (ArithmeticFirstOrderExpression -> Application)
   9     DoubleQuotedString shift (29)
   9     Identifier         shift (30)
   9     InvalidToken       shift (31)
   9     Minus              reduce (ArithmeticFirstOrderExpression -> Application)
   9     NumberLiteral      shift (32)
   9     ParenClose         reduce (ArithmeticFirstOrderExpression -> Application)
   9     ParenOpen          shift (2)
   9     Plus               reduce (ArithmeticFirstOrderExpression -> Application)
   9     Slash              reduce (ArithmeticFirstOrderExpression -> Application)
   10    $                  reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   10    Asterisk           reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   10    BlockClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   10    BlockOpen          shift (1)
   10    Break              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   10    DoubleQuotedString shift (29)
   10    Identifier         shift (30)
   10    InvalidToken       shift (31)
   10    Minus              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   10    NumberLiteral      shift (32)
   10    ParenClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   10    ParenOpen          shift (2)
   10    Plus               reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   10    Slash              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Asterisk Application)
   11    $                  reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   11    Asterisk           reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   11    BlockClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   11    BlockOpen          shift (1)
   11    Break              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   11    DoubleQuotedString shift (29)
   11    Identifier         shift (30)
   11    InvalidToken       shift (31)
   11    Minus              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   11    NumberLiteral      shift (32)
   11    ParenClose         reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   11    ParenOpen          shift (2)
   11    Plus               reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   11    Slash              reduce (ArithmeticFirstOrderExpression -> ArithmeticFirstOrderExpression Slash Application)
   12    $                  reduce (Application -> Application TerminalEnclosedExpression)
   12    Asterisk           reduce (Application -> Application TerminalEnclosedExpression)
   12    BlockClose         reduce (Application -> Application TerminalEnclosedExpression)
   12    BlockOpen          reduce (Application -> Application TerminalEnclosedExpression)
   12    Break              reduce (Application -> Application TerminalEnclosedExpression)
   12    DoubleQuotedString reduce (Application -> Application TerminalEnclosedExpression)
   12    Identifier         reduce (Application -> Application TerminalEnclosedExpression)
   12    InvalidToken       reduce (Application -> Application TerminalEnclosedExpression)
   12    Minus              reduce (Application -> Application TerminalEnclosedExpression)
   12    NumberLiteral      reduce (Application -> Application TerminalEnclosedExpression)
   12    ParenClose         reduce (Application -> Application TerminalEnclosedExpression)
   12    ParenOpen          reduce (Application -> Application TerminalEnclosedExpression)
   12    Plus               reduce (Application -> Application TerminalEnclosedExpression)
   12    Slash              reduce (Application -> Application TerminalEnclosedExpression)
   13    $                  reduce (Application -> TerminalEnclosedExpression)
   13    Asterisk           reduce (Application -> TerminalEnclosedExpression)
   13    BlockClose         reduce (Application -> TerminalEnclosedExpression)
   13    BlockOpen          reduce (Application -> TerminalEnclosedExpression)
   13    Break              reduce (Application -> TerminalEnclosedExpression)
   13    DoubleQuotedString reduce (Application -> TerminalEnclosedExpression)
   13    Identifier         reduce (Application -> TerminalEnclosedExpression)
   13    InvalidToken       reduce (Application -> TerminalEnclosedExpression)
   13    Minus              reduce (Application -> TerminalEnclosedExpression)
   13    NumberLiteral      reduce (Application -> TerminalEnclosedExpression)
   13    ParenClose         reduce (Application -> TerminalEnclosedExpression)
   13    ParenOpen          reduce (Application -> TerminalEnclosedExpression)
   13    Plus               reduce (Application -> TerminalEnclosedExpression)
   13    Slash              reduce (Application -> TerminalEnclosedExpression)
   14    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   14    Asterisk           shift (7)
   14    BlockClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   14    Break              reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   14    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   14    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   14    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticFirstOrderExpression)
   14    Slash              shift (8)
   15    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   15    Asterisk           shift (7)
   15    BlockClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   15    Break              reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   15    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   15    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   15    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Minus ArithmeticFirstOrderExpression)
   15    Slash              shift (8)
   16    $                  reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   16    Asterisk           shift (7)
   16    BlockClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   16    Break              reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   16    Minus              reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   16    ParenClose         reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   16    Plus               reduce (ArithmeticSecondOrderExpression -> ArithmeticSecondOrderExpression Plus ArithmeticFirstOrderExpression)
   16    Slash              shift (8)
   17    $                  reduce (BindingExpression -> ArithmeticSecondOrderExpression)
   17    BlockClose         reduce (BindingExpression -> ArithmeticSecondOrderExpression)
   17    Break              reduce (BindingExpression -> ArithmeticSecondOrderExpression)
   17    Minus              shift (5)
   17    ParenClose         reduce (BindingExpression -> ArithmeticSecondOrderExpression)
   17    Plus               shift (6)
   18    $                  reduce (BindingExpression -> Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression)
   18    BlockClose         reduce (BindingExpression -> Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression)
   18    Break              reduce (BindingExpression -> Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression)
   18    Minus              shift (5)
   18    ParenClose         reduce (BindingExpression -> Let Identifier BindingParameters Equals ArithmeticSecondOrderExpression)
   18    Plus               shift (6)
   19    Identifier         shift (20)
   20    Equals             reduce (BindingParameters -> )
   20    Identifier         reduce (BindingParameters -> )
   21    Equals             shift (4)
   21    Identifier         shift (22)
   22    Equals             reduce (BindingParameters -> BindingParameters Identifier)
   22    Identifier         reduce (BindingParameters -> BindingParameters Identifier)
   23    $                  reduce (Expression -> ExpressionConcatenation)
   23    BlockClose         reduce (Expression -> ExpressionConcatenation)
   23    Break              shift (3)
   23    ParenClose         reduce (Expression -> ExpressionConcatenation)
   24    $                  reduce (ExpressionConcatenation -> BindingExpression)
   24    BlockClose         reduce (ExpressionConcatenation -> BindingExpression)
   24    Break              reduce (ExpressionConcatenation -> BindingExpression)
   24    ParenClose         reduce (ExpressionConcatenation -> BindingExpression)
   25    $                  reduce (ExpressionConcatenation -> ExpressionConcatenation Break BindingExpression)
   25    BlockClose         reduce (ExpressionConcatenation -> ExpressionConcatenation Break BindingExpression)
   25    Break              reduce (ExpressionConcatenation -> ExpressionConcatenation Break BindingExpression)
   25    ParenClose         reduce (ExpressionConcatenation -> ExpressionConcatenation Break BindingExpression)
   26    $                  accept
   27    BlockClose         shift (28)
   28    $                  reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    Asterisk           reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    BlockClose         reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    BlockOpen          reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    Break              reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    DoubleQuotedString reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    Identifier         reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    InvalidToken       reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    Minus              reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    NumberLiteral      reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    ParenClose         reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    ParenOpen          reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    Plus               reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   28    Slash              reduce (TerminalEnclosedExpression -> BlockOpen Expression BlockClose)
   29    $                  reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    Asterisk           reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    BlockClose         reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    BlockOpen          reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    Break              reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    DoubleQuotedString reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    Identifier         reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    InvalidToken       reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    Minus              reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    NumberLiteral      reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    ParenClose         reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    ParenOpen          reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    Plus               reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   29    Slash              reduce (TerminalEnclosedExpression -> DoubleQuotedString)
   30    $                  reduce (TerminalEnclosedExpression -> Identifier)
   30    Asterisk           reduce (TerminalEnclosedExpression -> Identifier)
   30    BlockClose         reduce (TerminalEnclosedExpression -> Identifier)
   30    BlockOpen          reduce (TerminalEnclosedExpression -> Identifier)
   30    Break              reduce (TerminalEnclosedExpression -> Identifier)
   30    DoubleQuotedString reduce (TerminalEnclosedExpression -> Identifier)
   30    Identifier         reduce (TerminalEnclosedExpression -> Identifier)
   30    InvalidToken       reduce (TerminalEnclosedExpression -> Identifier)
   30    Minus              reduce (TerminalEnclosedExpression -> Identifier)
   30    NumberLiteral      reduce (TerminalEnclosedExpression -> Identifier)
   30    ParenClose         reduce (TerminalEnclosedExpression -> Identifier)
   30    ParenOpen          reduce (TerminalEnclosedExpression -> Identifier)
   30    Plus               reduce (TerminalEnclosedExpression -> Identifier)
   30    Slash              reduce (TerminalEnclosedExpression -> Identifier)
   31    $                  reduce (TerminalEnclosedExpression -> InvalidToken)
   31    Asterisk           reduce (TerminalEnclosedExpression -> InvalidToken)
   31    BlockClose         reduce (TerminalEnclosedExpression -> InvalidToken)
   31    BlockOpen          reduce (TerminalEnclosedExpression -> InvalidToken)
   31    Break              reduce (TerminalEnclosedExpression -> InvalidToken)
   31    DoubleQuotedString reduce (TerminalEnclosedExpression -> InvalidToken)
   31    Identifier         reduce (TerminalEnclosedExpression -> InvalidToken)
   31    InvalidToken       reduce (TerminalEnclosedExpression -> InvalidToken)
   31    Minus              reduce (TerminalEnclosedExpression -> InvalidToken)
   31    NumberLiteral      reduce (TerminalEnclosedExpression -> InvalidToken)
   31    ParenClose         reduce (TerminalEnclosedExpression -> InvalidToken)
   31    ParenOpen          reduce (TerminalEnclosedExpression -> InvalidToken)
   31    Plus               reduce (TerminalEnclosedExpression -> InvalidToken)
   31    Slash              reduce (TerminalEnclosedExpression -> InvalidToken)
   32    $                  reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    Asterisk           reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    BlockClose         reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    BlockOpen          reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    Break              reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    DoubleQuotedString reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    Identifier         reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    InvalidToken       reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    Minus              reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    NumberLiteral      reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    ParenClose         reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    ParenOpen          reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    Plus               reduce (TerminalEnclosedExpression -> NumberLiteral)
   32    Slash              reduce (TerminalEnclosedExpression -> NumberLiteral)
   33    ParenClose         shift (34)
   34    $                  reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    Asterisk           reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    BlockClose         reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    BlockOpen          reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    Break              reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    DoubleQuotedString reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    Identifier         reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    InvalidToken       reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    Minus              reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    NumberLiteral      reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    ParenClose         reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    ParenOpen          reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    Plus               reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)
   34    Slash              reduce (TerminalEnclosedExpression -> ParenOpen Expression ParenClose)

GOTO
   Source state Symbol                          Destination state
   0            Application                     9
   0            ArithmeticFirstOrderExpression  14
   0            ArithmeticSecondOrderExpression 17
   0            BindingExpression               24
   0            Expression                      26
   0            ExpressionConcatenation         23
   0            TerminalEnclosedExpression      13
   1            Application                     9
   1            ArithmeticFirstOrderExpression  14
   1            ArithmeticSecondOrderExpression 17
   1            BindingExpression               24
   1            Expression                      27
   1            ExpressionConcatenation         23
   1            TerminalEnclosedExpression      13
   2            Application                     9
   2            ArithmeticFirstOrderExpression  14
   2            ArithmeticSecondOrderExpression 17
   2            BindingExpression               24
   2            Expression                      33
   2            ExpressionConcatenation         23
   2            TerminalEnclosedExpression      13
   3            Application                     9
   3            ArithmeticFirstOrderExpression  14
   3            ArithmeticSecondOrderExpression 17
   3            BindingExpression               25
   3            TerminalEnclosedExpression      13
   4            Application                     9
   4            ArithmeticFirstOrderExpression  14
   4            ArithmeticSecondOrderExpression 18
   4            TerminalEnclosedExpression      13
   5            Application                     9
   5            ArithmeticFirstOrderExpression  15
   5            TerminalEnclosedExpression      13
   6            Application                     9
   6            ArithmeticFirstOrderExpression  16
   6            TerminalEnclosedExpression      13
   7            Application                     10
   7            TerminalEnclosedExpression      13
   8            Application                     11
   8            TerminalEnclosedExpression      13
   9            TerminalEnclosedExpression      12
   10           TerminalEnclosedExpression      12
   11           TerminalEnclosedExpression      12
   20           BindingParameters               21

*)

type Asterisk = Common.PositionInSource
type BlockClose = Common.PositionInSource
type BlockOpen = Common.PositionInSource
type Break = Common.PositionInSource
type DoubleQuotedString = string * Common.PositionInSource
type Equals = Common.PositionInSource
type Identifier = string * Common.PositionInSource
type InvalidToken = string * Common.PositionInSource
type Let = Common.PositionInSource
type Minus = Common.PositionInSource
type NumberLiteral = int * int option * Common.PositionInSource
type ParenClose = Common.PositionInSource
type ParenOpen = Common.PositionInSource
type Plus = Common.PositionInSource
type Slash = Common.PositionInSource

type Application =
    | Application of Application * TerminalEnclosedExpression
    | Fallthrough of TerminalEnclosedExpression

type ArithmeticFirstOrderExpression =
    | Divide of ArithmeticFirstOrderExpression * Slash * Application
    | Fallthrough of Application
    | Multiply of ArithmeticFirstOrderExpression * Asterisk * Application

type ArithmeticSecondOrderExpression =
    | Add of ArithmeticSecondOrderExpression * Plus * ArithmeticFirstOrderExpression
    | Fallthrough of ArithmeticFirstOrderExpression
    | Subtract of ArithmeticSecondOrderExpression * Minus * ArithmeticFirstOrderExpression

type BindingExpression =
    | Binding of Let * Identifier * BindingParameters * Equals * ArithmeticSecondOrderExpression
    | Fallthrough of ArithmeticSecondOrderExpression

type BindingParameters =
    | Cons of BindingParameters * Identifier
    | Empty

type Expression =
    | Expression of ExpressionConcatenation

type ExpressionConcatenation =
    | Concat of ExpressionConcatenation * Break * BindingExpression
    | Fallthrough of BindingExpression

type Program =
    | Program of Expression

type TerminalEnclosedExpression =
    | Block of BlockOpen * Expression * BlockClose
    | Identifier of Identifier
    | InvalidToken of InvalidToken
    | Number of NumberLiteral
    | Paren of ParenOpen * Expression * ParenClose
    | String of DoubleQuotedString

type InputItem =
    | Asterisk of Asterisk
    | BlockClose of BlockClose
    | BlockOpen of BlockOpen
    | Break of Break
    | DoubleQuotedString of DoubleQuotedString
    | Equals of Equals
    | Identifier of Identifier
    | InvalidToken of InvalidToken
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
    | BlockClose
    | BlockOpen
    | Break
    | DoubleQuotedString
    | Equals
    | Identifier
    | InvalidToken
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
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 1 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 2 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 3 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.Let x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(19)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Let; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 4 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 5 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 6 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 7 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 8 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockOpen; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.NumberLiteral; ExpectedItem.ParenOpen ]
                keepGoing <- false
        | 9 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = ArithmeticFirstOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 10 ->
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.Break _ ->
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 11 ->
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(1)
            | InputItem.Break _ ->
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(29)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(30)
            | InputItem.InvalidToken x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(31)
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(32)
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(2)
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
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
                    | 0 -> 14
                    | 1 -> 14
                    | 2 -> 14
                    | 3 -> 14
                    | 4 -> 14
                    | 5 -> 15
                    | 6 -> 16
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 12 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.InvalidToken _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let arg1 = lhsStack.Pop() :?> Application
                let reductionResult = Application.Application (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 13 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.InvalidToken _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> TerminalEnclosedExpression
                let reductionResult = Application.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 9
                    | 1 -> 9
                    | 2 -> 9
                    | 3 -> 9
                    | 4 -> 9
                    | 5 -> 9
                    | 6 -> 9
                    | 7 -> 10
                    | 8 -> 11
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 14 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticFirstOrderExpression
                let reductionResult = ArithmeticSecondOrderExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.Break; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 15 ->
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.Break; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 16 ->
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                    | 0 -> 17
                    | 1 -> 17
                    | 2 -> 17
                    | 3 -> 17
                    | 4 -> 18
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
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.Break; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 17 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = BindingExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 24
                    | 1 -> 24
                    | 2 -> 24
                    | 3 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = BindingExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 24
                    | 1 -> 24
                    | 2 -> 24
                    | 3 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = BindingExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 24
                    | 1 -> 24
                    | 2 -> 24
                    | 3 -> 25
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
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let reductionResult = BindingExpression.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 24
                    | 1 -> 24
                    | 2 -> 24
                    | 3 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(6)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.Break; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus ]
                keepGoing <- false
        | 18 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let arg4 = lhsStack.Pop() :?> Equals
                let arg3 = lhsStack.Pop() :?> BindingParameters
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = BindingExpression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 24
                    | 1 -> 24
                    | 2 -> 24
                    | 3 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let arg4 = lhsStack.Pop() :?> Equals
                let arg3 = lhsStack.Pop() :?> BindingParameters
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = BindingExpression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 24
                    | 1 -> 24
                    | 2 -> 24
                    | 3 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let arg4 = lhsStack.Pop() :?> Equals
                let arg3 = lhsStack.Pop() :?> BindingParameters
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = BindingExpression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 24
                    | 1 -> 24
                    | 2 -> 24
                    | 3 -> 25
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
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg5 = lhsStack.Pop() :?> ArithmeticSecondOrderExpression
                let arg4 = lhsStack.Pop() :?> Equals
                let arg3 = lhsStack.Pop() :?> BindingParameters
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> Let
                let reductionResult = BindingExpression.Binding (arg1, arg2, arg3, arg4, arg5)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 24
                    | 1 -> 24
                    | 2 -> 24
                    | 3 -> 25
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(6)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.Break; ExpectedItem.Minus; ExpectedItem.ParenClose; ExpectedItem.Plus ]
                keepGoing <- false
        | 19 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.Identifier ]
                keepGoing <- false
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(20)
            | _ ->
                // error
                expected <- [ ExpectedItem.Identifier ]
                keepGoing <- false
        | 20 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.Equals; ExpectedItem.Identifier ]
                keepGoing <- false
            | InputItem.Equals _ ->
                // reduce
                let reductionResult = BindingParameters.Empty
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 20 -> 21
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                let reductionResult = BindingParameters.Empty
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 20 -> 21
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.Equals; ExpectedItem.Identifier ]
                keepGoing <- false
        | 21 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.Equals; ExpectedItem.Identifier ]
                keepGoing <- false
            | InputItem.Equals x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(4)
            | InputItem.Identifier x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(22)
            | _ ->
                // error
                expected <- [ ExpectedItem.Equals; ExpectedItem.Identifier ]
                keepGoing <- false
        | 22 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.Equals; ExpectedItem.Identifier ]
                keepGoing <- false
            | InputItem.Equals _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> BindingParameters
                let reductionResult = BindingParameters.Cons (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 20 -> 21
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg2 = lhsStack.Pop() :?> Identifier
                let arg1 = lhsStack.Pop() :?> BindingParameters
                let reductionResult = BindingParameters.Cons (arg1, arg2)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 20 -> 21
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.Equals; ExpectedItem.Identifier ]
                keepGoing <- false
        | 23 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ExpressionConcatenation
                let reductionResult = Expression.Expression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 26
                    | 1 -> 27
                    | 2 -> 33
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ExpressionConcatenation
                let reductionResult = Expression.Expression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 26
                    | 1 -> 27
                    | 2 -> 33
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(3)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> ExpressionConcatenation
                let reductionResult = Expression.Expression arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 26
                    | 1 -> 27
                    | 2 -> 33
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.Break; ExpectedItem.ParenClose ]
                keepGoing <- false
        | 24 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> BindingExpression
                let reductionResult = ExpressionConcatenation.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 23
                    | 1 -> 23
                    | 2 -> 23
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> BindingExpression
                let reductionResult = ExpressionConcatenation.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 23
                    | 1 -> 23
                    | 2 -> 23
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> BindingExpression
                let reductionResult = ExpressionConcatenation.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 23
                    | 1 -> 23
                    | 2 -> 23
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> BindingExpression
                let reductionResult = ExpressionConcatenation.Fallthrough arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 23
                    | 1 -> 23
                    | 2 -> 23
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.Break; ExpectedItem.ParenClose ]
                keepGoing <- false
        | 25 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BindingExpression
                let arg2 = lhsStack.Pop() :?> Break
                let arg1 = lhsStack.Pop() :?> ExpressionConcatenation
                let reductionResult = ExpressionConcatenation.Concat (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 23
                    | 1 -> 23
                    | 2 -> 23
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BindingExpression
                let arg2 = lhsStack.Pop() :?> Break
                let arg1 = lhsStack.Pop() :?> ExpressionConcatenation
                let reductionResult = ExpressionConcatenation.Concat (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 23
                    | 1 -> 23
                    | 2 -> 23
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BindingExpression
                let arg2 = lhsStack.Pop() :?> Break
                let arg1 = lhsStack.Pop() :?> ExpressionConcatenation
                let reductionResult = ExpressionConcatenation.Concat (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 23
                    | 1 -> 23
                    | 2 -> 23
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BindingExpression
                let arg2 = lhsStack.Pop() :?> Break
                let arg1 = lhsStack.Pop() :?> ExpressionConcatenation
                let reductionResult = ExpressionConcatenation.Concat (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 23
                    | 1 -> 23
                    | 2 -> 23
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.BlockClose; ExpectedItem.Break; ExpectedItem.ParenClose ]
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
                expected <- [ ExpectedItem.EndOfStream ]
                keepGoing <- false
        | 27 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.BlockClose ]
                keepGoing <- false
            | InputItem.BlockClose x ->
                // shift
                lhsStack.Push(x)
                if inputEnumerator.MoveNext() then
                    lookahead <- inputEnumerator.Current
                else
                    lookaheadIsEof <- true
                stateStack.Push(28)
            | _ ->
                // error
                expected <- [ ExpectedItem.BlockClose ]
                keepGoing <- false
        | 28 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.InvalidToken _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> BlockClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> BlockOpen
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Block (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 29 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.InvalidToken _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> DoubleQuotedString
                let reductionResult = TerminalEnclosedExpression.String arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 30 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.InvalidToken _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> Identifier
                let reductionResult = TerminalEnclosedExpression.Identifier arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 31 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.InvalidToken _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> InvalidToken
                let reductionResult = TerminalEnclosedExpression.InvalidToken arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 32 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Asterisk _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.DoubleQuotedString _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Identifier _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.InvalidToken _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Minus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.NumberLiteral _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.ParenOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Plus _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Slash _ ->
                // reduce
                stateStack.Pop() |> ignore
                let arg1 = lhsStack.Pop() :?> NumberLiteral
                let reductionResult = TerminalEnclosedExpression.Number arg1
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | 33 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // error
                expected <- [ ExpectedItem.ParenClose ]
                keepGoing <- false
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
                expected <- [ ExpectedItem.ParenClose ]
                keepGoing <- false
        | 34 ->
            match lookahead with
            | _ when lookaheadIsEof ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockClose _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.BlockOpen _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.Break _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | InputItem.InvalidToken _ ->
                // reduce
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                stateStack.Pop() |> ignore
                let arg3 = lhsStack.Pop() :?> ParenClose
                let arg2 = lhsStack.Pop() :?> Expression
                let arg1 = lhsStack.Pop() :?> ParenOpen
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
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
                let reductionResult = TerminalEnclosedExpression.Paren (arg1, arg2, arg3)
                lhsStack.Push(reductionResult)
                let nextState =
                    match stateStack.Peek() with
                    | 0 -> 13
                    | 1 -> 13
                    | 2 -> 13
                    | 3 -> 13
                    | 4 -> 13
                    | 5 -> 13
                    | 6 -> 13
                    | 7 -> 13
                    | 8 -> 13
                    | 9 -> 12
                    | 10 -> 12
                    | 11 -> 12
                    | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."
                stateStack.Push(nextState)
            | _ ->
                // error
                expected <- [ ExpectedItem.EndOfStream; ExpectedItem.Asterisk; ExpectedItem.BlockClose; ExpectedItem.BlockOpen; ExpectedItem.Break; ExpectedItem.DoubleQuotedString; ExpectedItem.Identifier; ExpectedItem.InvalidToken; ExpectedItem.Minus; ExpectedItem.NumberLiteral; ExpectedItem.ParenClose; ExpectedItem.ParenOpen; ExpectedItem.Plus; ExpectedItem.Slash ]
                keepGoing <- false
        | _ -> failwith "Parser is in an invalid state. This is a bug in the parser generator."

    if accepted
    then Ok result
    else Error {
        unexpected = if lookaheadIsEof then Unexpected.EndOfStream else Unexpected.InputItem lookahead
        expected = expected
    }
