using System;
using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using PuzzleSolvers.Exotic;
using RT.Serialization;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Diagonal sandwich")]
    public class LittleSandwich : SvgDiagonalConstraint
    {
        public override string Description => $"Within this diagonal, there is exactly one {Digit1} and one {Digit2} and the digits sandwiched between them must add up to {Clue}. The {Digit1} and {Digit2} can occur in either order. Other digits may repeat or be absent.";

        public static readonly Example Example = new()
        {
            Constraints = { new LittleSandwich(DiagonalDirection.NorthEast, 6, 5, 1, 9) },
            Cells = { 18, 10, 2 },
            Good = { 1, 5, 9 },
            Bad = { 1, 9, 5 },
            Layout = ExampleLayout.TopLeft3x4
        };

        public int Digit1 { get; private set; }
        public int Digit2 { get; private set; }
        [ClassifyIgnoreIfDefault]
        public bool OmitCrust { get; private set; }

        public LittleSandwich(DiagonalDirection direction, int offset, int clue, int digit1 = 1, int digit2 = 9, bool opposite = false, bool omitCrust = false)
            : base(direction, offset, clue, opposite)
        {
            Digit1 = digit1;
            Digit2 = digit2;
            OmitCrust = omitCrust;
        }
        private LittleSandwich() { }  // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new LittleSandwichConstraint(AffectedCells, Clue, Digit1, Digit2); }
        public override bool Verify(int[] grid)
        {
            var p1 = AffectedCells.IndexOf(ix => grid[ix] == Digit1);
            var p2 = AffectedCells.IndexOf(ix => grid[ix] == Digit2);
            if (p1 == -1 || p2 == -1 || AffectedCells.LastIndexOf(ix => grid[ix] == Digit1) != p1 || AffectedCells.LastIndexOf(ix => grid[ix] == Digit2) != p2)
                return false;
            (p1, p2) = (Math.Min(p1, p2), Math.Max(p1, p2));
            return AffectedCells.Skip(p1 + 1).Take(p2 - p1 - 1).Sum(ix => grid[ix]) == Clue;
        }

        public override string Svg => ArrowSvg + NumberSvg + CrustSvg1 + CrustSvg2;

        private const double svgCrustOfs = .25;

        private string CrustSvg1 => Opposite
            ? Direction switch
            {
                DiagonalDirection.SouthEast => $"<text text-anchor='middle' x='{9 + svgMargin + svgArrLen + svgMargin - svgCrustOfs}' y='{9 - Offset + svgMargin + svgArrLen + svgMargin + .1}' font-size='.2'>{Digit1}</text>",
                DiagonalDirection.SouthWest => $"<text text-anchor='middle' x='{Offset - svgMargin - svgArrLen - svgMargin}' y='{9 + svgMargin + svgArrLen + svgMargin + .05 - svgCrustOfs}' font-size='.2'>{Digit1}</text>",
                DiagonalDirection.NorthWest => $"<text text-anchor='middle' x='{-svgMargin - svgArrLen - svgMargin + svgCrustOfs}' y='{Offset - svgMargin - svgArrLen - svgMargin + .05}' font-size='.2'>{Digit1}</text>",
                DiagonalDirection.NorthEast => $"<text text-anchor='middle' x='{9 - Offset + svgMargin + svgArrLen + svgMargin - svgCrustOfs}' y='{-svgMargin - svgArrLen - svgMargin + .05}' font-size='.2'>{Digit1}</text>",
                _ => null
            }
            : Direction switch
            {
                DiagonalDirection.SouthEast => $"<text text-anchor='middle' x='{Offset - svgMargin - svgArrLen - svgMargin + svgCrustOfs}' y='{-svgMargin - svgArrLen - svgMargin + .05}' font-size='.2'>{Digit1}</text>",
                DiagonalDirection.SouthWest => $"<text text-anchor='middle' x='{9 + svgMargin + svgArrLen + svgMargin - svgCrustOfs}' y='{Offset - svgMargin - svgArrLen - svgMargin + .05}' font-size='.2'>{Digit1}</text>",
                DiagonalDirection.NorthWest => $"<text text-anchor='middle' x='{9 - Offset + svgMargin + svgArrLen + svgMargin - svgCrustOfs}' y='{9 + svgMargin + svgArrLen + svgMargin + .1}' font-size='.2'>{Digit1}</text>",
                DiagonalDirection.NorthEast => $"<text text-anchor='middle' x='{-svgMargin - svgArrLen - svgMargin}' y='{9 - Offset + svgMargin + svgArrLen + svgMargin + .05 - svgCrustOfs}' font-size='.2'>{Digit1}</text>",
                _ => null
            };

        private string CrustSvg2 => Opposite
            ? Direction switch
            {
                DiagonalDirection.SouthEast => $"<text text-anchor='middle' x='{9 + svgMargin + svgArrLen + svgMargin}' y='{9 - Offset + svgMargin + svgArrLen + svgMargin + .05 - svgCrustOfs}' font-size='.2'>{Digit2}</text>",
                DiagonalDirection.SouthWest => $"<text text-anchor='middle' x='{Offset - svgMargin - svgArrLen - svgMargin + svgCrustOfs}' y='{9 + svgMargin + svgArrLen + svgMargin + .1}' font-size='.2'>{Digit2}</text>",
                DiagonalDirection.NorthWest => $"<text text-anchor='middle' x='{-svgMargin - svgArrLen - svgMargin}' y='{Offset - svgMargin - svgArrLen - svgMargin + .05 + svgCrustOfs}' font-size='.2'>{Digit2}</text>",
                DiagonalDirection.NorthEast => $"<text text-anchor='middle' x='{9 - Offset + svgMargin + svgArrLen + svgMargin}' y='{-svgMargin - svgArrLen - svgMargin + .05 + svgCrustOfs}' font-size='.2'>{Digit2}</text>",
                _ => null
            }
            : Direction switch
            {
                DiagonalDirection.SouthEast => $"<text text-anchor='middle' x='{Offset - svgMargin - svgArrLen - svgMargin}' y='{-svgMargin - svgArrLen - svgMargin + .05 + svgCrustOfs}' font-size='.2'>{Digit2}</text>",
                DiagonalDirection.SouthWest => $"<text text-anchor='middle' x='{9 + svgMargin + svgArrLen + svgMargin}' y='{Offset - svgMargin - svgArrLen - svgMargin + .05 + svgCrustOfs}' font-size='.2'>{Digit2}</text>",
                DiagonalDirection.NorthWest => $"<text text-anchor='middle' x='{9 - Offset + svgMargin + svgArrLen + svgMargin}' y='{9 + svgMargin + svgArrLen + svgMargin + .05 - svgCrustOfs}' font-size='.2'>{Digit2}</text>",
                DiagonalDirection.NorthEast => $"<text text-anchor='middle' x='{-svgMargin - svgArrLen - svgMargin + svgCrustOfs}' y='{9 - Offset + svgMargin + svgArrLen + svgMargin + .1}' font-size='.2'>{Digit2}</text>",
                _ => null
            };

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var list = new List<SvgConstraint>();
            void addLittleSandwiches(DiagonalDirection dir, int offset, int[] cells)
            {
                var digitDic = new Dictionary<int, int>();
                foreach (var cell in cells)
                    digitDic.IncSafe(sudoku[cell]);
                for (var i = 0; i < cells.Length; i++)
                    if (digitDic[sudoku[cells[i]]] == 1)
                        for (var j = i + 1; j < cells.Length; j++)
                            if (digitDic[sudoku[cells[j]]] == 1)
                                list.Add(new LittleSandwich(dir, offset,
                                    Enumerable.Range(i + 1, j - i - 1).Sum(c => sudoku[cells[c]]),
                                    Math.Min(sudoku[cells[i]], sudoku[cells[j]]),
                                    Math.Max(sudoku[cells[i]], sudoku[cells[j]])));
            }

            for (var offset = 0; offset < 8; offset++)
                addLittleSandwiches(DiagonalDirection.SouthEast, offset, GetAffectedCells(DiagonalDirection.SouthEast, offset));
            for (var offset = 1; offset < 8; offset++)
                addLittleSandwiches(DiagonalDirection.SouthWest, offset, GetAffectedCells(DiagonalDirection.SouthWest, offset));
            for (var offset = 1; offset < 8; offset++)
                addLittleSandwiches(DiagonalDirection.NorthWest, offset, GetAffectedCells(DiagonalDirection.NorthWest, offset));
            for (var offset = 0; offset < 8; offset++)
                addLittleSandwiches(DiagonalDirection.NorthEast, offset, GetAffectedCells(DiagonalDirection.NorthEast, offset));
            return list;
        }
    }
}
