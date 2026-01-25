# SpiralMatrixSharp

[![Build status](https://ci.appveyor.com/api/projects/status/lsg9edneqh3nfpit/branch/master?svg=true)](https://ci.appveyor.com/project/bangjunyoung/spiralmatrixsharp/branch/master)

Arguably the world's most advanced spiral matrix generator written in F#.

## Examples

There are two API functions, `generate` and `traverse`:

``` fsharp
> [|1 .. 12|] |> SpiralMatrix.generate Clockwise TopLeft 4 3;;
val it : int [,] = [[1; 2; 3]
                    [10; 11; 4]
                    [9; 12; 5]
                    [8; 7; 6]]

> [25 .. -1 .. 1] |> SpiralMatrix.generate Counterclockwise TopRight 5 5;;
val it : int [,] = [[21; 22; 23; 24; 25]
                    [20; 7; 8; 9; 10]
                    [19; 6; 1; 2; 11]
                    [18; 5; 4; 3; 12]
                    [17; 16; 15; 14; 13]]

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

They can be called from C# as follows:

``` csharp
using System;
using System.Linq;
using SpiralMatrixSharp;

class SpiralMatrixCSharpDemo {
    static void Main() {
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
    }
}
```
