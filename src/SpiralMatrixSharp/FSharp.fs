//
// Copyright 2019 Bang Jun-young
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
// OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
// NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

namespace SpiralMatrixSharp.FSharp

module Array2D =
    let toArray (source: 'T[,]) =
        source
        |> Seq.cast<'T>
        |> Seq.toArray

    let ofArray nrows ncols source =
        if Array.length source <> nrows * ncols then
            invalidArg "source" "must have a length of nrows multiplied by ncols"

        let array2D = Array2D.zeroCreate nrows ncols
        source
        |> Array.iteri (fun i elem -> array2D.[i / ncols, i % ncols] <- elem)

        array2D

module List =
    let pairwiseCyclic source =
        match source with
        | [] -> []
        | head :: _ ->
            let rec loop source' =
                match source' with
                | [] -> []
                | [x] -> [x, head]
                | x :: y :: rest -> (x, y) :: loop (y :: rest)

            loop source

type CycleNode<'T> internal (item: 'T) as this =
    let mutable next = this
    member __.Value = item
    member __.Next
        with get() = next
        and internal set value = next <- value

module Cycle =
    let value (node: CycleNode<_>) = node.Value
    let next (node: CycleNode<_>) = node.Next

    let ofList source =
        source
        |> List.map CycleNode
        |> List.pairwiseCyclic
        |> List.map (fun (x, y) -> x.Next <- y; x)
        |> List.head
