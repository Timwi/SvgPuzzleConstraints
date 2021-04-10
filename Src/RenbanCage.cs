using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Renban cage")]
    public class RenbanCage : SvgRegionConstraint
    {
        public override string Description => $"Digits within the cage must be different and form a consecutive set.";
        public static readonly Example Example = new Example
        {
            Constraints = { new RenbanCage(new[] { 0, 1, 2, 3, 10, 12 }) },
            Cells = { 0, 1, 2, 3, 10, 12 },
            Good = { 5, 3, 7, 4, 6, 2 },
            Bad = { 5, 3, 7, 1, 6, 2 },
            Reason = "Digits 1–3 and 5–7 occur. The 4 is skipped."
        };

        public RenbanCage(int[] cells) : base(cells) { }
        private RenbanCage() { }    // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new ConsecutiveUniquenessConstraint(Cells); }

        public override bool Verify(int[] grid)
        {
            var numbers = Cells.Select(c => grid[c]).ToArray();
            return numbers.Distinct().Count() == numbers.Length && numbers.Count(c => !numbers.Contains(c + 1)) == 1;
        }

        public override bool ClashesWith(SvgConstraint other) => other switch
        {
            RenbanCage r => r.Cells.Intersect(Cells).Any(),
            SvgCellConstraint c => Cells.Contains(c.Cell),
            Thermometer t => t.Cells.Intersect(Cells).Any(),
            Palindrome p => p.Cells.Intersect(Cells).Any(),
            _ => false
        };

        public override IEnumerable<string> SvgDefs => new[] { $@"<pattern id='pattern-renban' width='2' height='2' patternTransform='rotate(45) scale(.35355) translate(.5, .5)' patternUnits='userSpaceOnUse'><path d='M0 0h1v1H0zM1 1h1v1H1z' /></pattern>" };

        public override string Svg => $@"<path d='{GenerateSvgPath(Cells, .25, .25)}' fill='url(#pattern-renban)' stroke='none' opacity='.2' />";

        public static IList<SvgConstraint> Generate(int[] sudoku, int[][] uniquenessRegions) => uniquenessRegions
            .Where(region => region.Length <= 7 && region.Select(c => sudoku[c]).Apply(numbers => numbers.Count(n => !numbers.Contains(n + 1)) == 1))
            .Select(region => (SvgConstraint) new RenbanCage(region))
            .ToList();
    }
}