namespace TypeSolver

open System.Collections.Generic
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type internal FunctionNodeRelations() =
    let dict = Dictionary<Node, Node * Node>()

    member _.RemoveWhereFunction(fn: Node) : unit = dict.Remove(fn) |> ignore

    member _.RemoveWhereParameter(param: Node) : unit =
        for kv in dict |> List.ofSeq do
            let p, _ = kv.Value

            if p = param then
                dict.Remove(kv.Key) |> ignore

    member _.RemoveWhereResult(result: Node) : unit =
        for kv in dict |> List.ofSeq do
            let _, r = kv.Value

            if r = result then
                dict.Remove(kv.Key) |> ignore

    member _.IsFunction(node: Node) : bool = dict.ContainsKey(node)

    member _.Set(fn: Node, param: Node, result: Node) : bool =
        match dict.TryGetValue(fn) with
        | true, (p, r) ->
            if p = param && r = result then
                false
            else
                failwith "Node has more than one function relation"
        | false, _ ->
            if dict |> Seq.exists (fun kv -> kv.Value = (param, result)) then
                failwith "Another similar function already exists"

            dict.Add(fn, (param, result))
            true

    member _.TryGetFunctionOfParamResult(param: Node, result: Node) : Node option =
        dict
        |> Seq.choose (fun kv -> if kv.Value = (param, result) then Some kv.Key else None)
        |> Seq.tryHead

    member _.TryGetParamResultOfFunction(fn: Node) : (Node * Node) option =
        match dict.TryGetValue(fn) with
        | true, (param, result) -> Some(param, result)
        | false, _ -> None

    member _.GetFunctionsOfParameter(param: Node) : (Node * Node * Node) list =
        dict
        |> Seq.choose (fun kv ->
            let p, r = kv.Value
            if p = param then Some(kv.Key, p, r) else None)
        |> List.ofSeq

    member _.GetFunctionsOfResult(result: Node) : (Node * Node * Node) list =
        dict
        |> Seq.choose (fun kv ->
            let p, r = kv.Value
            if r = result then Some(kv.Key, p, r) else None)
        |> List.ofSeq

    override _.ToString() =
        dict
        |> Seq.map (fun kv ->
            let p, r = kv.Value
            $"{kv.Key} : {p} -> {r}")
        |> String.concat "\n"
