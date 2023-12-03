﻿namespace Common

open System
open System.Diagnostics

[<DebuggerDisplay("{ToString()}")>]
type AtomTypeId private (id: Guid, hint: string) =
    let mutable hashCode = id.GetHashCode()

    new() =
        let id = Guid.NewGuid()
        let hint = $"AtomTypeId({id})"
        AtomTypeId(id, hint)

    new(hint) =
        let id = Guid.NewGuid()
        AtomTypeId(id, hint)

    member val private Id = id

    override _.GetHashCode() = hashCode

    override _.Equals(other) =
        match other with
        | :? AtomTypeId as other -> other.Id = id
        | _ -> false

    interface IComparable with
        member this.CompareTo(other) =
            match other with
            | :? AtomTypeId as other -> other.Id.CompareTo(id)
            | _ -> 1

    override _.ToString() = hint
