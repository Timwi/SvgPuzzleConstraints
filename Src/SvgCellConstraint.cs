namespace SvgPuzzleConstraints
{
    public abstract class SvgCellConstraint : SvgConstraint
    {
        public int Cell { get; private set; }

        public SvgCellConstraint(int cell) { Cell = cell; }
        protected SvgCellConstraint() { }    // for Classify

        public override bool ClashesWith(SvgConstraint other) => other is SvgCellConstraint;
        public sealed override bool IncludesCell(int cell) => cell == Cell;
    }
}
