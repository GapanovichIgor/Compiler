namespace TypeSolver

open System
open System.Collections.Generic
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type internal InstanceNodeRelations() =
    let groups = Dictionary<Guid, Dictionary<Node, Node>>()

    let getGroup id =
        match groups.TryGetValue(id) with
        | false, _ -> failwith "Invalid group"
        | true, group -> group

    let validateCanAdd (prototype, instance) =
        if prototype = instance then
            failwith "Type cannot be a prototype of itself"

        let instanceHasAnotherPrototype =
            groups.Values
            |> Seq.collect id
            |> Seq.exists (fun kv -> kv.Value = instance && kv.Key <> prototype)

        if instanceHasAnotherPrototype then
            failwith "Instance already has another prototype"

    member _.TryCreateGroup(prototype: Node, instance: Node) : Guid option =
        validateCanAdd (prototype, instance)

        let thisPrototypeRelationAlreadyExists =
            groups.Values
            |> Seq.exists (fun instanceMap ->
                match instanceMap.TryGetValue(prototype) with
                | true, i when i = instance -> true
                | _ -> false)

        if not thisPrototypeRelationAlreadyExists then
            let id = Guid.NewGuid()
            let group = Dictionary<Node, Node>()
            groups.Add(id, group)
            Some id
        else
            None

    member _.AddToGroup(prototype: Node, instance: Node, groupId: Guid) : unit =
        validateCanAdd (prototype, instance)

        let group = getGroup groupId

        group[prototype] <- instance

    member _.RemoveFromGroup(prototype: Node, instance: Node, groupId: Guid) : unit =
        let group = getGroup groupId

        match group.TryGetValue(prototype) with
        | true, i when i = instance ->
            group.Remove(prototype) |> ignore
            // if group.Count = 0 then
            //     groups.Remove(groupId) |> ignore
        | _ -> failwith "Invalid prototype instance pair"

    member _.TryGetPrototype(instance: Node) : (Guid * Node) option =
        groups
        |> Seq.choose (fun groupKv ->
            groupKv.Value
            |> Seq.tryFind (fun kv -> kv.Value = instance)
            |> Option.map (fun kv -> groupKv.Key, kv.Key))
        |> Seq.tryExactlyOne

    member _.TryGetInstance(prototype: Node, group: Guid) : Node option =
        let group = getGroup group

        match group.TryGetValue(prototype) with
        | false, _ -> None
        | true, instance -> Some instance

    member _.GetInstances(prototype: Node) : (Guid * Node) list =
        groups
        |> Seq.choose (fun kv ->
            let groupId = kv.Key
            let group = kv.Value

            match group.TryGetValue(prototype) with
            | true, instance -> Some(groupId, instance)
            | false, _ -> None)
        |> List.ofSeq

    override _.ToString() =
        groups.Values
        |> Seq.map (fun group -> group |> Seq.map (fun kv -> $"{kv.Key} -> {kv.Value}") |> String.concat "\n")
        |> String.concat "\n---\n"
