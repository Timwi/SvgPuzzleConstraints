using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Anti-knight")]
    public class AntiKnight : SvgCellConstraint
    {
        public override string Description => "The same digit can’t be a knight’s move in chess away from this digit.";
        public static readonly Example Example = new Example
        {
            Constraints = { new AntiKnight(19) },
            Cells = { 12, 19 },
            Good = { 7, 2 },
            Bad = { 2, 2 }
        };

        public AntiKnight(int cell) : base(cell) { }
        private AntiKnight() { }    // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new AntiKnightConstraint(9, 9, enforcedCells: new[] { Cell }); }
        public override string Svg => $@"<path fill='rgba(0, 0, 0, .2)' d='{SvgPaths.Knight}' transform='translate({Cell % 9}, {Cell / 9})' />";

        public override bool Verify(int[] grid)
        {
            foreach (var c in AntiKnightConstraint.KnightsMoves(Cell, 9, 9, false))
                if (grid[c] == grid[Cell])
                    return false;
            return true;
        }

        public static IList<SvgConstraint> Generate(int[] sudoku) => Enumerable.Range(0, 81)
            .Where(cell => AntiKnightConstraint.KnightsMoves(cell, 9, 9, false).All(c => sudoku[c] != sudoku[cell]))
            .Select(cell => new AntiKnight(cell))
            .ToArray();
    }
}
