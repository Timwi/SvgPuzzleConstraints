using System.Collections.Generic;
using System.Linq;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Clockface")]
    public class Clockface : SvgFourCellConstraint
    {
        public override string Description => $"The digits around the circular arrow must ascend in {(Clockwise ? "clockwise" : "counterclockwise")} order.";
        public static readonly Example Example = new Example
        {
            Constraints = { new Clockface(0, true), new Clockface(11, false) },
            Cells = { 0, 1, 10, 9, 11, 12, 21, 20 },
            Good = { 3, 5, 8, 9, 2, 8, 7, 4 },
            Bad = { 3, 7, 5, 9, 2, 3, 6, 4 }
        };

        public bool Clockwise { get; private set; }

        public Clockface(int topLeftCell, bool clockwise) : base(topLeftCell) { Clockwise = clockwise; }
        private Clockface() { }     // for Classify

        protected override bool verify(int a, int b, int c, int d) => verify(Clockwise, a, b, c, d);
        private static bool verify(bool clockwise, int a, int b, int c, int d) => clockwise
            ? (a < b && b < c && c < d) || (b < c && c < d && d < a) || (c < d && d < a && a < b) || (d < a && a < b && b < c)
            : (a > b && b > c && c > d) || (b > c && c > d && d > a) || (c > d && d > a && a > b) || (d > a && a > b && b > c);

        public override string Svg => $"<circle cx='{x}' cy='{y}' r='.2' fill='white' /><path transform='translate({x}, {y}) scale({(Clockwise ? ".04" : "-.04, .04")})' d='m -4.8,-4 v 1 h 2.16 A 4,4 0 0 0 -4,0 4,4 0 0 0 0,4 4,4 0 0 0 4,0 4,4 0 0 0 2.82,-2.82 L 2.12,-2.12 A 3,3 0 0 1 3,0 3,3 0 0 1 0,3 3,3 0 0 1 -3,0 3,3 0 0 1 -1.8,-2.38 V 0 h 1 v -4 z' />";

        public static IList<SvgConstraint> Generate(int[] sudoku) =>
            generate(sudoku, (cell, a, b, c, d) => verify(true, a, b, c, d) ? new[] { new Clockface(cell, true) } : verify(false, a, b, c, d) ? new[] { new Clockface(cell, false) } : Enumerable.Empty<SvgFourCellConstraint>());
    }
}
