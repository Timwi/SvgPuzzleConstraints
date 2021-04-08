using System;
using System.Collections.Generic;
using PuzzleSolvers;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("No-consecutive")]
    public class GlobalNoConsecutive : SvgGlobalConstraint
    {
        public override string Description => "Orthogonally adjacent cells cannot contain digits that are numerically consecutive (difference of 1).";

        public GlobalNoConsecutive() : base() { }

        protected override IEnumerable<Constraint> getConstraints() { yield return new NoConsecutiveConstraint(9, 9, includeDiagonals: false); }

        public override bool Verify(int[] grid)
        {
            for (var cell = 0; cell < 81; cell++)
                foreach (var c in PuzzleUtil.Orthogonal(cell, 9, 9))
                    if (Math.Abs(grid[c] - grid[cell]) == 1)
                        return false;
            return true;
        }

        protected override string SvgIcon => $@"<path d='m 0 .5 .1 -.1 .1 .1 -.1 .1z M .4 .1 l .1 -.1 .1 .1 -.1 .1z M .8 .5 l .1 -.1 .1 .1 -.1 .1z M .4 .9 l .1 -.1 .1 .1 -.1 .1z' />";
    }
}
