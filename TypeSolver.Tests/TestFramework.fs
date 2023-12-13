module TypeSolver.Tests.TestFramework

let combineAllPermutations<'a> (elements: ('a -> 'a) list) : ('a -> 'a) list =
    let indexes = [ for i in 0 .. elements.Length - 1 -> i ]

    let rec loop permutations position =
        if position = indexes.Length - 1 then
            permutations
        else
            let permutations =
                [ for permutation in permutations do
                      for i in indexes do
                          if not (permutation |> List.contains i) then
                              yield i :: permutation ]

            loop permutations (position + 1)

    let initialPermutations =
        indexes |> List.map List.singleton

    let permutations = loop initialPermutations 0

    permutations
    |> List.map (fun permutation ->
        permutation
        |> Seq.map (fun i -> elements[i])
        |> Seq.reduce (>>))

let multiply (secondParts: ('b -> 'c) list) (firstParts: ('a -> 'b) list) : ('a -> 'c) list =
    List.allPairs firstParts secondParts
    |> List.map (fun (start, map) -> start >> map)

let run (functions: (unit -> unit) list) =
    for f in functions do
        f ()

