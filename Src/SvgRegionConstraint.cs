using System.Linq;

namespace SvgPuzzleConstraints
{
    public abstract class SvgRegionConstraint : SvgConstraint
    {
        public int[] Cells { get; private set; }

        public SvgRegionConstraint(int[] cells) { Cells = cells; }
        protected SvgRegionConstraint() { }    // for Classify

        public sealed override bool IncludesCell(int cell) => Cells.Contains(cell);
    }
}
