namespace SvgPuzzleConstraints
{
    public abstract class SvgGlobalConstraint : SvgConstraint
    {
        public sealed override string Svg => $"<g class='global'><rect x='0' y='0' width='1' height='1' rx='.1' ry='.1' fill='white' stroke='black' stroke-width='.05' />{SvgIcon}</g>";
        protected abstract string SvgIcon { get; }
        public override bool IncludesCell(int cell) => true;
        public override bool ClashesWith(SvgConstraint other) => false;
        protected SvgGlobalConstraint() { }    // for Classify
    }
}
