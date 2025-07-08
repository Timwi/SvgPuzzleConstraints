using System.Collections.Generic;
using System.Linq;
using RT.Util;

namespace SvgPuzzleConstraints
{
    public class Example
    {
        public List<SvgConstraint> Constraints = [];
        public List<int> Cells = [];
        public List<int> Good = [];
        public List<int> Bad = [];
        public ExampleLayout Layout = ExampleLayout.TopLeft3x3;
        public string Reason;

        public Dictionary<int, int?> GoodGivens => Enumerable.Range(0, Cells.Count).ToDictionary(ix => Cells[ix], ix => Good[ix].Nullable());
        public Dictionary<int, int?> BadGivens => Enumerable.Range(0, Cells.Count).ToDictionary(ix => Cells[ix], ix => Bad[ix].Nullable());
    }
}
