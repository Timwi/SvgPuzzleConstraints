using RT.Serialization;

namespace SvgPuzzleConstraints
{
    public abstract class SvgRowColNumberConstraint : SvgRowColConstraint
    {
        public int Clue { get; private set; }
        [ClassifyIgnoreIfDefault]
        public RowColDisplay Display { get; private set; }

        public SvgRowColNumberConstraint(bool isCol, int rowCol, int clue, RowColDisplay display) : base(isCol, rowCol)
        {
            Clue = clue;
            Display = display;
        }
        protected SvgRowColNumberConstraint() { }    // for Classify

        public sealed override string Svg =>
            IsCol && Display == RowColDisplay.TopLeft ? $@"<text x='{RowCol + .5}' y='.65' font-size='.8'>{Clue}</text>" :
            IsCol && Display == RowColDisplay.BottomRight ? $@"<text x='{RowCol + .5}' y='9.1' font-size='.8'>{Clue}</text>" :
            !IsCol && Display == RowColDisplay.TopLeft ? $@"<text x='-.1' y='{RowCol + .8}' font-size='.8' text-anchor='end'>{Clue}</text>" :
            !IsCol && Display == RowColDisplay.BottomRight ? $@"<text x='9.1' y='{RowCol + .8}' font-size='.8' text-anchor='start'>{Clue}</text>" :
            ElaborateSvg;
        protected abstract string ElaborateSvg { get; }

        public enum RowColDisplay
        {
            Default,
            TopLeft,
            BottomRight
        }
    }
}
