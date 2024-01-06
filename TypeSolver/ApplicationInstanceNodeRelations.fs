namespace TypeSolver

open System.Collections.Generic
open System.Diagnostics
open Common

[<DebuggerDisplay("{ToString()}")>]
type internal ApplicationInstanceNodeRelations() =
    let applications = Dictionary<ApplicationReference, Dictionary<Node, Node>>()

    let validateCanAdd (prototype, instance) =
        if prototype = instance then
            failwith "Type cannot be a prototype of itself"

        let instanceHasAnotherPrototype =
            applications.Values
            |> Seq.collect id
            |> Seq.exists (fun kv -> kv.Value = instance && kv.Key <> prototype)

        if instanceHasAnotherPrototype then
            failwith "Instance already has another prototype"

    // member _.TryCreateGroup(applicationReference: ApplicationReference, prototype: Node, instance: Node) : bool =
    //     validateCanAdd (prototype, instance)
    //
    //     let thisPrototypeRelationAlreadyExists =
    //         applications.Values
    //         |> Seq.exists (fun instanceMap ->
    //             match instanceMap.TryGetValue(prototype) with
    //             | true, i when i = instance -> true
    //             | _ -> false)
    //
    //     if not thisPrototypeRelationAlreadyExists then
    //         let application = Dictionary<Node, Node>()
    //         applications.Add(applicationReference, application)
    //         true
    //     else
    //         false

    member _.AddToApplication(applicationReference: ApplicationReference, prototype: Node, instance: Node) : unit =
        validateCanAdd (prototype, instance)

        let application =
            match applications.TryGetValue(applicationReference) with
            | true, application -> application
            | false, _ ->
                let application = Dictionary()
                applications.Add(applicationReference, application)
                application

        application[prototype] <- instance

    member _.RemoveFromApplication(applicationReference: ApplicationReference, prototype: Node, instance: Node) : unit =
        match applications.TryGetValue(applicationReference) with
        | false, _ -> failwith "Invalid applicationReference"
        | true, application ->
            match application.TryGetValue(prototype) with
            | true, i when i = instance ->
                application.Remove(prototype) |> ignore
            | _ -> failwith "Invalid prototype instance pair"

    member _.TryGetPrototype(instance: Node) : (ApplicationReference * Node) option =
        applications
        |> Seq.choose (fun groupKv ->
            groupKv.Value
            |> Seq.tryFind (fun kv -> kv.Value = instance)
            |> Option.map (fun kv -> groupKv.Key, kv.Key))
        |> Seq.tryExactlyOne

    member _.TryGetInstance(applicationReference: ApplicationReference, prototype: Node) : Node option =
        match applications.TryGetValue(applicationReference) with
        | false, _ -> None
        | true, application ->
            match application.TryGetValue(prototype) with
            | false, _ -> None
            | true, instance -> Some instance

    member _.GetInstances(prototype: Node) : (ApplicationReference * Node) list =
        applications
        |> Seq.choose (fun kv ->
            let groupId = kv.Key
            let group = kv.Value

            match group.TryGetValue(prototype) with
            | true, instance -> Some(groupId, instance)
            | false, _ -> None)
        |> List.ofSeq

    override _.ToString() =
        applications.Values
        |> Seq.map (fun group -> group |> Seq.map (fun kv -> $"{kv.Key} -> {kv.Value}") |> String.concat "\n")
        |> String.concat "\n---\n"
