using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Diagonal sum")]
    public class LittleKiller : SvgDiagonalConstraint
    {
        public override string Description => "The digits along the indicated diagonal must sum to the specified total. (The digits need not necessarily be different.)";

        public static readonly Example Example = new()
        {
            Constraints = { new LittleKiller(DiagonalDirection.NorthEast, 7, 8) },
            Cells = { 1, 9 },
            Good = { 2, 6 },
            Bad = { 3, 6 }
        };

        public LittleKiller(DiagonalDirection direction, int offset, int clue, bool opposite = false) : base(direction, offset, clue, opposite) { }
        private LittleKiller() { }  // for Classify

        public override string Svg => ArrowSvg + NumberSvg;

        protected override IEnumerable<Constraint> getConstraints() { yield return new SumConstraint(Clue, AffectedCells); }
        public override bool Verify(int[] grid) => AffectedCells.Sum(c => grid[c]) == Clue;

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var list = new List<SvgConstraint>();
            for (var offset = 0; offset < 8; offset++)
                list.Add(new LittleKiller(DiagonalDirection.SouthEast, offset, GetAffectedCells(DiagonalDirection.SouthEast, offset).Sum(c => sudoku[c])));
            for (var offset = 1; offset < 8; offset++)
                list.Add(new LittleKiller(DiagonalDirection.SouthWest, offset, GetAffectedCells(DiagonalDirection.SouthWest, offset).Sum(c => sudoku[c])));
            for (var offset = 1; offset < 8; offset++)
                list.Add(new LittleKiller(DiagonalDirection.NorthWest, offset, GetAffectedCells(DiagonalDirection.NorthWest, offset).Sum(c => sudoku[c])));
            for (var offset = 0; offset < 8; offset++)
                list.Add(new LittleKiller(DiagonalDirection.NorthEast, offset, GetAffectedCells(DiagonalDirection.NorthEast, offset).Sum(c => sudoku[c])));
            return list;
        }
    }
}
