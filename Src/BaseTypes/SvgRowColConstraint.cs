using RT.Util;

namespace SvgPuzzleConstraints
{
    public abstract class SvgRowColConstraint : SvgConstraint
    {
        public bool IsCol { get; private set; }
        public int RowCol { get; private set; }

        public sealed override bool IncludesCell(int cell) => false;
        public sealed override bool IncludesRowCol(bool isCol, int rowCol, bool topLeft) => isCol == IsCol && rowCol == RowCol && topLeft == ShownTopLeft;

        /// <summary>
        ///     Determines whether the constraint is visually shown on the top of a column/left of a row (<c>true</c>) or the
        ///     bottom of a column/right of a row (<c>false</c>).</summary>
        public abstract bool ShownTopLeft { get; }

        public SvgRowColConstraint(bool isCol, int rowCol) { IsCol = isCol; RowCol = rowCol; }
        protected SvgRowColConstraint() { }    // for Classify

        public override bool ClashesWith(SvgConstraint other) => other is SvgRowColConstraint cc && IsCol == cc.IsCol && RowCol == cc.RowCol && ShownTopLeft == cc.ShownTopLeft;
        protected int[] GetAffectedCells(bool reverse) => Ut.NewArray(9, x => IsCol ? (RowCol + 9 * (reverse ? 8 - x : x)) : ((reverse ? 8 - x : x) + 9 * RowCol));
    }
}
