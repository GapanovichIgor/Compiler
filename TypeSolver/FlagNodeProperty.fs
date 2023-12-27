namespace TypeSolver

open System.Collections.Generic
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type internal FlagNodeProperty() =
    let set = HashSet()

    member _.Set(node: Node) =
        set.Add(node) |> ignore

    member _.Unset(node: Node) =
        set.Remove(node) |> ignore

    member _.IsSet(node: Node) =
        set.Contains(node)

    override _.ToString() =
        set
        |> Seq.map string
        |> String.concat ", "