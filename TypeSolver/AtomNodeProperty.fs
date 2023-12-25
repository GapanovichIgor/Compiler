namespace TypeSolver

open System.Collections.Generic
open System.Diagnostics
open Common

[<DebuggerDisplay("{ToString()}")>]
type internal AtomNodeProperty() =
    let dict = Dictionary<Node, AtomTypeId>()

    member _.Set(node: Node, atomTypeId: AtomTypeId) : bool =
        match dict.TryGetValue(node) with
        | true, existingAtomTypeId ->
            if existingAtomTypeId = atomTypeId then
                false
            else
                failwith "Node is set to be two different atom types"
        | false, _ ->
            if dict.Values |> Seq.contains atomTypeId then
                failwith "Another node was already set to this AtomTypeId"

            dict.Add(node, atomTypeId)
            true

    member _.Unset(node: Node) : unit = dict.Remove(node) |> ignore

    member _.TryGetAtomTypeId(node: Node) : AtomTypeId option =
        match dict.TryGetValue(node) with
        | true, atomTypeId -> Some atomTypeId
        | false, _ -> None

    member _.IsAtom(node: Node) : bool = dict.ContainsKey(node)

    member _.TryGetNodeByAtomTypeId(atomTypeId: AtomTypeId) : Node option =
        dict
        |> Seq.choose (fun kv ->
            if kv.Value = atomTypeId then
                Some kv.Key
            else
                None)
        |> Seq.tryHead

    override _.ToString() =
        dict
        |> Seq.map (fun kv -> $"{kv.Key} : {kv.Value}")
        |> String.concat "\n"