module Compiler.Diagnostics

type ProblemLevel =
    | LevelWarning
    | LevelError

type Problem =
    { level: ProblemLevel
      positionInSource: PositionInSource
      description: string }

type Diagnostics =
    { problems: Problem list }

    member this.hasProblems = this.problems |> List.isEmpty |> not

    member this.hasErrors = this.problems |> List.exists (fun p -> p.level = LevelError)
