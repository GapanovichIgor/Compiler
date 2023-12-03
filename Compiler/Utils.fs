namespace Compiler

module Option =
    let require errorMsg =
        Option.defaultWith (fun () -> failwith errorMsg)

[<AutoOpen>]
module AutoOpen =
    let (|Is|_|) v1 v2 = if v1 = v2 then Some() else None