namespace Common

open System
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type Identifier =
    private
        { name: string
          identity: Guid }

    member this.Name = this.name

    override this.ToString() = this.name

    static member Create(name: string) =
        if name.Length = 0 then
            failwith "Identifier can not be empty"
        elif not (Char.IsLetter name[0]) then
            failwith "Identifier must start with a letter"
        elif name |> Seq.exists (Char.IsLetterOrDigit >> not) then
            failwith "Identifier must only contain letters and digits"
        else
            { name = name
              identity = Guid.NewGuid() }

