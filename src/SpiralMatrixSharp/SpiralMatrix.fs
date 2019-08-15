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

namespace SpiralMatrixSharp

open SpiralMatrixSharp.FSharp

type Direction = Clockwise | Counterclockwise
type InitialPosition = TopLeft | TopRight | BottomLeft | BottomRight

module SpiralMatrix =
    let internal spiralList movers initialPoint points =
        let rec loop mover point points acc =
            if points |> Set.isEmpty then
                acc |> List.rev
            else
                let points', acc' =
                    if points |> Set.contains point then
                        points |> Set.remove point, point :: acc
                    else
                        points, acc

                let mover', point' =
                    let nextPoint = point |> Cycle.value mover
                    if points |> Set.contains nextPoint then
                        mover, nextPoint
                    else
                        Cycle.next mover, point

                loop mover' point' points' acc' 

        loop (Cycle.ofList movers) initialPoint (Set.ofList points) []

    [<CompiledName("Generate")>]
    let generate direction initialPosition nrows ncolumns source =
        let lastRow, lastColumn = nrows - 1, ncolumns - 1

        let move (drow, dcolumn) (row, column) = row + drow, column + dcolumn
        let moveLeft = move (0, -1)
        let moveRight = move (0, 1)
        let moveUp = move (-1, 0)
        let moveDown = move (1, 0)

        let movers, initialPoint =
            match direction, initialPosition with
            | Clockwise, TopLeft ->
                [moveRight; moveDown; moveLeft; moveUp], (0, 0)
            | Clockwise, TopRight ->
                [moveDown; moveLeft; moveUp; moveRight], (0, lastColumn)
            | Clockwise, BottomRight ->
                [moveLeft; moveUp; moveRight; moveDown], (lastRow, lastColumn)
            | Clockwise, BottomLeft ->
                [moveUp; moveRight; moveDown; moveLeft], (lastRow, 0)
            | Counterclockwise, TopLeft ->
                [moveDown; moveRight; moveUp; moveLeft], (0, 0)
            | Counterclockwise, BottomLeft ->
                [moveRight; moveUp; moveLeft; moveDown], (lastRow, 0)
            | Counterclockwise, BottomRight ->
                [moveUp; moveLeft; moveDown; moveRight], (lastRow, lastColumn)
            | Counterclockwise, TopRight ->
                [moveLeft; moveDown; moveRight; moveUp], (0, lastColumn)

        let points = List.allPairs [0 .. lastRow] [0 .. lastColumn]
        let matrix = Array2D.zeroCreate nrows ncolumns

        (source, points |> spiralList movers initialPoint)
        ||> Seq.iter2 (fun elem (row, col) -> matrix.[row, col] <- elem)

        matrix

    [<CompiledName("Traverse")>]
    let traverse direction initialPosition source =
        let nrows, ncolumns = Array2D.length1 source, Array2D.length2 source
        let matrix =
            [1 .. nrows * ncolumns]
            |> generate direction initialPosition nrows ncolumns

        (matrix |> Array2D.toArray, source |> Array2D.toArray)
        ||> Array.zip
        |> Array.sort
        |> Array.map snd
