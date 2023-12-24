namespace TypeSolver

open System.Collections.Generic
open Common

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
            dict.Add(node, atomTypeId)
            true

    member _.Unset(node: Node) : unit = dict.Remove(node) |> ignore

    member _.TryGetAtomTypeId(node: Node) : AtomTypeId option =
        match dict.TryGetValue(node) with
        | true, atomTypeId -> Some atomTypeId
        | false, _ -> None

    member _.IsAtom(node: Node) : bool = dict.ContainsKey(node)
