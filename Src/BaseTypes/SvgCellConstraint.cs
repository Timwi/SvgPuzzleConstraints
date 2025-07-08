using System.Collections.Generic;

namespace SvgPuzzleConstraints
{
    public abstract class SvgCellConstraint : SvgConstraint
    {
        public int Cell { get; private set; }

        public SvgCellConstraint(int cell) { Cell = cell; }
        protected SvgCellConstraint() { }    // for Classify

        public override bool ClashesWith(SvgConstraint other) => other is SvgCellConstraint;
        public sealed override bool IncludesCell(int cell) => cell == Cell;

        protected static IEnumerable<int> findCellsInDirection(int cell, CellDirection dir)
        {
            var x = cell % 9;
            var y = cell / 9;
            for (; inRange(x) && inRange(y); x += dx(dir), y += dy(dir))
                yield return x + 9 * y;
        }
    }
}
