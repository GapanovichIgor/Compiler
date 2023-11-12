namespace Compiler

module Option =
    let require errorMsg =
        Option.defaultWith (fun () -> failwith errorMsg)
