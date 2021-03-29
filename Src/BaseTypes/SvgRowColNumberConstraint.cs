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
            IsCol && Display == RowColDisplay.TopLeft ? $@"<text x='{RowCol + .5}' y='-.25' font-size='.5'>{Clue}</text>" :
            IsCol && Display == RowColDisplay.BottomRight ? $@"<text x='{RowCol + .5}' y='9.7' font-size='.5'>{Clue}</text>" :
            !IsCol && Display == RowColDisplay.TopLeft ? $@"<text x='-.25' y='{RowCol + .675}' font-size='.5' text-anchor='end'>{Clue}</text>" :
            !IsCol && Display == RowColDisplay.BottomRight ? $@"<text x='9.25' y='{RowCol + .675}' font-size='.5' text-anchor='start'>{Clue}</text>" :
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
