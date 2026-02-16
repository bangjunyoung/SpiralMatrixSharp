# SpiralMatrixSharp

[![CI](https://github.com/bangjunyoung/SpiralMatrixSharp/actions/workflows/ci.yml/badge.svg)](https://github.com/bangjunyoung/SpiralMatrixSharp/actions/workflows/ci.yml)

SpiralMatrixSharp is a high-performance library written in F# for generating and traversing 2D matrices
in a spiral pattern. It provides a flexible API that allows you to define the direction (clockwise 
or counterclockwise) and the starting corner.

## Key API Functions

The library is centered around two primary operations:

### `generate`

```fsharp
generate : Direction -> InitialPosition -> (nrows: int) -> (ncolumns: int) -> (source: seq<'T>) -> 'T[,]
```

Transforms a 1D sequence of elements into a 2D spiral matrix based on the specified parameters.

`Direction` can be `Clockwise` or `Counterclockwise`.

`InitialPosition` can be `TopLeft`, `TopRight`, `BottomLeft`, or `BottomRight`.

```fsharp
> [|1 .. 12|] |> SpiralMatrix.generate Clockwise TopLeft 4 3;;
val it : int [,] = [[1; 2; 3]
                    [10; 11; 4]
                    [9; 12; 5]
                    [8; 7; 6]]
```

### `traverse`

```fsharp
traverse : Direction -> InitialPosition -> (source: 'T[,]) -> seq<'T>
```

Flattens a 2D matrix into a 1D sequence by traversing it in a spiral pattern based on the specified parameters.

```fsharp
> let matrix = [|'a' .. 'l'|] |> SpiralMatrix.generate Clockwise TopLeft 4 3;;
val matrix : char [,] = [['a'; 'b'; 'c']
                         ['j'; 'k'; 'd']
                         ['i'; 'l'; 'e']
                         ['h'; 'g'; 'f']]

> matrix |> SpiralMatrix.traverse Clockwise TopLeft;;
val it : char [] =
  [|'a'; 'b'; 'c'; 'd'; 'e'; 'f'; 'g'; 'h'; 'i'; 'j'; 'k'; 'l'|]

> matrix |> SpiralMatrix.traverse Counterclockwise BottomRight;;
val it : char [] =
  [|'f'; 'e'; 'd'; 'c'; 'b'; 'a'; 'j'; 'i'; 'h'; 'g'; 'l'; 'k'|]
```

### Interoperability with C#

`generate` and `traverse` functions are designed to be easily used from C#.
Note that, unlike in F#, you must explicitly specify the `Direction` and `InitialPosition` enum types as shown in the example below:

```csharp
using System;
using System.Linq;
using SpiralMatrixSharp;

var (nrows, ncols) = (5, 4);
var matrix = SpiralMatrix.Generate(
    Direction.Counterclockwise,
    InitialPosition.BottomLeft,
    nrows, ncols,
    Enumerable.Range(1, nrows * ncols));

for (int i = 0; i < matrix.GetLength(0); i++) {
    for (int j = 0; j < matrix.GetLength(1); j++) {
        Console.Write("{0,2}\t", matrix[i, j]);
    }
    Console.WriteLine();
}

var traversed = SpiralMatrix.Traverse(
    Direction.Counterclockwise,
    InitialPosition.BottomLeft,
    matrix);

var aggregated =
    traversed
        .Select(item => item.ToString())
        .Aggregate((acc, item) => acc + " " + item);

Console.WriteLine("<{0}>", aggregated);
```
