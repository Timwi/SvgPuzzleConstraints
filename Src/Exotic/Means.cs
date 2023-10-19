using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PuzzleSolvers;
using PuzzleSolvers.Exotic;
using RT.Util;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Means")]
    public class Means : SvgCellConstraint
    {
        public override string Description => $"Consider all three-cell lines centered on this cell. Exactly {NumArithmetic} of these must have the center cell be the arithmetic mean ((a+b)/2) of the flanking cells, and exactly {NumGeometric} of them the geometric mean (√(ab)).";
        public static readonly Example Example = new Example
        {
            Constraints = { new Means(11, 1, 1) },
            Cells = { 1, 2, 3, 10, 11, 12, 19, 20, 21 },
            Good = { 1, 7, 4, 5, 3, 2, 2, 8, 9 },
            Bad = { 1, 7, 4, 5, 3, 1, 2, 8, 7 },
            Reason = "This example contains two arithmetic means (2-3-4 and 1-3-5) and no geometric means."
        };

        public int NumArithmetic { get; private set; }
        public int NumGeometric { get; private set; }

        public Means(int cell, int numArith, int numGeom) : base(cell)
        {
            if (cell % 9 == 0 || cell % 9 == 8 || cell / 9 == 0 || cell / 9 == 8)
                throw new ArgumentException("Means constraint is not allowed on the edge or corner of the grid.");
            NumArithmetic = numArith;
            NumGeometric = numGeom;
        }
        private Means() { }    // for Classify

        protected override IEnumerable<Constraint> getConstraints()
        {
            var triplets = getTriplets(Cell);
            yield return new TripletValidityConstraint(triplets, NumArithmetic, isArith);
            yield return new TripletValidityConstraint(triplets, NumGeometric, isGeom);
        }

        private static bool isArith(int a, int r, int b) => a + b == 2 * r;
        private static bool isGeom(int a, int r, int b) => a * b == r * r;

        public override string Svg
        {
            get
            {
                var triangle = $"<path d='M{svgX(Cell) - .4} {svgY(Cell) + .4} {svgX(Cell)} {svgY(Cell) - .4} {svgX(Cell) + .4} {svgY(Cell) + .4}z' fill='rgba(0, 0, 0, .1)' />";
                if (NumArithmetic + NumGeometric == 0)
                    return triangle;
                var group = new StringBuilder();
                var groupWidth = 0d;
                for (var a = 0; a < NumArithmetic; a++)
                {
                    group.Append($"<circle cx='{a * 1.25 + .5}' cy='.5' r='.5' fill='rgba(0, 0, 0, .1)' />");
                    groupWidth += 1.25;
                }
                for (var g = 0; g < NumGeometric; g++)
                {
                    group.Append($"<path d='M{NumArithmetic * 1.25 + g * 1.25} 0h1v1h-1z' fill='rgba(0, 0, 0, .1)' />");
                    groupWidth += 1.25;
                }
                if (NumArithmetic + NumGeometric == 1)
                {
                    return $"{triangle}<g transform='translate({svgX(Cell) - .35 / 2}, {svgY(Cell)}) scale(.35)'>{group}</g>";
                }

                groupWidth -= .25;
                var scaleFactor = .7 / groupWidth;
                return $"{triangle}<g transform='translate({svgX(Cell) - .35}, {svgY(Cell) + .35 - scaleFactor}) scale({scaleFactor})'>{group}</g>";
            }
        }

        public override bool Verify(int[] grid)
        {
            var (numArith, numGeom) = countArithGeom(grid, Cell);
            return numArith == NumArithmetic && numGeom == NumGeometric;
        }

        private static (int numArith, int numGeom) countArithGeom(int[] grid, int centerCell)
        {
            var numArith = 0;
            var numGeom = 0;
            foreach (var triplet in getTriplets(centerCell))
            {
                if (isArith(grid[triplet[0]], grid[triplet[1]], grid[triplet[2]]))
                    numArith++;
                if (isGeom(grid[triplet[0]], grid[triplet[1]], grid[triplet[2]]))
                    numGeom++;
            }
            return (numArith, numGeom);
        }

        private static int[][] getTriplets(int centerCell) => Ut.NewArray(
            new[] { centerCell - 10, centerCell, centerCell + 10 },
            new[] { centerCell - 9, centerCell, centerCell + 9 },
            new[] { centerCell - 8, centerCell, centerCell + 8 },
            new[] { centerCell - 1, centerCell, centerCell + 1 });

        public static IList<SvgConstraint> Generate(int[] sudoku) => Enumerable.Range(0, 81).Where(c => c % 9 != 0 && c % 9 != 8 && c / 9 != 0 && c / 9 != 8)
            .Select(c => (cell: c, inf: countArithGeom(sudoku, c)))
            .Select(tup => new Means(tup.cell, tup.inf.numArith, tup.inf.numGeom))
            .ToArray();
    }
}
