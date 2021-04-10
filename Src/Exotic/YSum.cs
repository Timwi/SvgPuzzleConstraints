using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using PuzzleSolvers.Exotic;
using RT.Util;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Y-sum")]
    public class YSum : SvgRowColNumberConstraint
    {
        public override string Description => $"The sum of the first Y digits in this {(IsCol ? "column" : "row")} must add up to {Clue}, where Y is the Xth digit in the {(IsCol ? "column" : "row")} and X is the first.";
        public override double ExtraTop => IsCol && !Reverse ? .5 : 0;
        public override double ExtraRight => !IsCol && Reverse ? .25 : 0;
        public override double ExtraLeft => !IsCol && !Reverse ? .5 : 0;
        public override bool ShownTopLeft => !Reverse;

        public static readonly Example Example = new Example
        {
            Constraints = { new YSum(false, 0, false, 19) },
            Cells = { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
            Good = { 2, 4, 7, 6, 1, 9, 8, 5, 3 },
            Bad = { 7, 4, 2, 6, 1, 9, 8, 5, 3 },
            Reason = "The first digit is 7. The 7th digit is 8. Therefore, we sum the first 8 digits, which gives 7+4+2+6+1+9+8+5 = 42.",
            Wide = true
        };

        public YSum(bool isCol, int rowCol, bool reverse, int clue, RowColDisplay display = RowColDisplay.Default)
            : base(isCol, rowCol, clue, display) { Reverse = reverse; }
        private YSum() { }    // for Classify

        public bool Reverse { get; private set; }

        protected override IEnumerable<Constraint> getConstraints() { yield return new YSumUniquenessConstraint(Clue, GetAffectedCells(Reverse)); }

        public override bool Verify(int[] grid) => GetAffectedCells(Reverse).Select(cell => grid[cell]).ToArray().Apply(numbers => numbers.Take(numbers[numbers[0] - 1]).Sum() == Clue);

        protected override string ElaborateSvg => $@"<g transform='translate({(IsCol ? RowCol : Reverse ? 8.8 : -.8)}, {(IsCol ? (Reverse ? 9 : -.8) : RowCol + .1)})'>
            <text x='.5' y='.325' font-size='.3'>YΣ</text>
            <text x='.5' y='.65' font-size='.3'>{Clue}</text>
        </g>";

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var constraints = new List<SvgConstraint>();
            foreach (var isCol in new[] { false, true })
                foreach (var reverse in new[] { false, true })
                    for (var rowCol = 0; rowCol < 9; rowCol++)
                        constraints.Add(new YSum(isCol, rowCol, reverse, Ut.NewArray(9, x => sudoku[isCol ? (rowCol + 9 * (reverse ? 8 - x : x)) : ((reverse ? 8 - x : x) + 9 * rowCol)]).Apply(numbers => numbers.Take(numbers[numbers[0] - 1]).Sum())));
            return constraints;
        }
    }
}