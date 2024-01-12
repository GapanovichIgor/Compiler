namespace TypeSolver

open System.Collections.Generic
open Common

type internal ApplicationNodeRelations() =
    let dict = Dictionary<ApplicationId, Dictionary<Node, Node>>()

    member _.TryGetSubstitution(appId: ApplicationId, placeholder: Node) : Node option =
        match dict.TryGetValue(appId) with
        | false, _ -> None
        | true, map ->
            match map.TryGetValue(placeholder) with
            | false, _ -> None
            | true, substitution -> Some substitution

    member _.GetSubstitutions(placeholder: Node): (ApplicationId * Node) list =
        dict
        |> Seq.choose (fun kv ->
            match kv.Value.TryGetValue(placeholder) with
            | true, assigned -> Some (kv.Key, assigned)
            | false, _ -> None)
        |> List.ofSeq

    member _.GetPlaceholders(substitution: Node): (ApplicationId * Node) list =
        dict
        |> Seq.collect (fun kv ->
            let appId = kv.Key
            kv.Value
            |> Seq.choose (fun kv ->
                if kv.Value = substitution then
                    Some (appId, kv.Key)
                else
                    None)
            |> List.ofSeq)
        |> List.ofSeq

    member _.SetSubstitution(applicationId: ApplicationId, placeholder: Node, substitution: Node) =
        let map =
            match dict.TryGetValue(applicationId) with
            | true, map -> map
            | false, _ ->
                let map = Dictionary()
                dict.Add(applicationId, map)
                map

        map.Add(placeholder, substitution)

    member _.RemovePlaceholder(applicationId: ApplicationId, placeholder: Node) =
        if not (dict[applicationId].Remove(placeholder)) then
            failwith "Placeholder not found"