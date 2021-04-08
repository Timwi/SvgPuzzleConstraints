using System.Collections.Generic;
using PuzzleSolvers;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Anti-knight")]
    public class GlobalAntiKnight : SvgGlobalConstraint
    {
        public override string Description => "Two of the same digit can’t be a knight’s move in chess away from each other.";

        public GlobalAntiKnight() : base() { }

        protected override IEnumerable<Constraint> getConstraints() { yield return new AntiKnightConstraint(9, 9); }

        public override bool Verify(int[] grid)
        {
            for (var cell = 0; cell < 81; cell++)
                foreach (var c in AntiKnightConstraint.KnightsMoves(cell, 9, 9, false))
                    if (grid[c] == grid[cell])
                        return false;
            return true;
        }

        protected override string SvgIcon => $@"<path fill='black' d='{SvgPaths.Knight}' transform='translate(.05, .05) scale(.9)' />";
    }
}
