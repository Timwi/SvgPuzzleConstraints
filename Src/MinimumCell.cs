using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Minimum cell")]
    public class MinimumCell : SvgCellConstraint
    {
        public override string Description => "This digit must be less than the digits orthogonally adjacent to it.";

        public static readonly Example Example = new()
        {
            Constraints = { new MinimumCell(11) },
            Cells = { 2, 10, 11, 12, 20 },
            Good = { 9, 5, 3, 6, 8 },
            Bad = { 9, 5, 3, 1, 8 },
            Reason = "1 is greater than 3."
        };

        public MinimumCell(int cell) : base(cell) { }
        private MinimumCell() { }    // for Classify

        protected override IEnumerable<Constraint> getConstraints() => PuzzleUtil.Orthogonal(Cell).Select(adj => new LessThanConstraint([Cell, adj]));
        public override string Svg => $"<path transform='translate({Cell % 9}, {Cell / 9})' d='M.5.2.7.05H.3Zm.3.3.15.2V.3ZM.5.8.3.95h.4ZM.2.5.05.3v.4z' opacity='.2' />";

        public override bool Verify(int[] grid)
        {
            foreach (var c in PuzzleUtil.Orthogonal(Cell))
                if (grid[c] <= grid[Cell])
                    return false;
            return true;
        }

        public static IList<SvgConstraint> Generate(int[] sudoku) => Enumerable.Range(0, 81)
            .Where(cell => PuzzleUtil.Orthogonal(cell).All(c => sudoku[cell] < sudoku[c]))
            .Select(cell => new MinimumCell(cell))
            .ToArray();
    }
}
