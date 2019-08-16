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

module SpiralMatrixSharp.SpiralMatrixTest

open NUnit.Framework

[<Test>]
let ``generate should work as expected`` () =
    let actual=
        [|20 .. -1 .. 1|]
        |> SpiralMatrix.generate Counterclockwise TopRight 5 4
    let expected =
        array2D [|
                [|17; 18; 19; 20|]
                [|16;  5;  6;  7|]
                [|15;  4;  1;  8|]
                [|14;  3;  2;  9|]
                [|13; 12; 11; 10|]
        |]

    Assert.That(actual, Is.EqualTo expected)

[<Test>]
let ``traverse should work as expected`` () =
    let actual =
        array2D [|
            [|17; 18; 19; 20|]
            [|16;  5;  6;  7|]
            [|15;  4;  1;  8|]
            [|14;  3;  2;  9|]
            [|13; 12; 11; 10|]
        |]
        |> SpiralMatrix.traverse Counterclockwise TopRight
    let expected = [|20 .. -1 .. 1|]

    Assert.That(actual, Is.EqualTo expected)

let testParameters =
    ([Clockwise; Counterclockwise],
        [TopLeft; TopRight; BottomRight; BottomLeft])
    ||> List.allPairs
    |> List.map TestCaseData

[<TestCaseSource("testParameters")>]
let ``generate and traverse are inverse operations`` direction initialPosition =
    let nrows, ncols = 4, 3
    let original = [|1 .. nrows * ncols|]
    let roundtripped =
        original
        |> SpiralMatrix.generate direction initialPosition nrows ncols
        |> SpiralMatrix.traverse direction initialPosition

    Assert.That(roundtripped, Is.EquivalentTo original)
