using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("A-sum")]
    public class ASum : SvgCellConstraint
    {
        public override string Description => $"The value at the base of the arrow indicates how many cells in the direction of the arrow to add up. Those cells must sum up to {Sum}.";

        public static readonly Example Example = new()
        {
            Constraints = { new ASum(9, CellDirection.Right, 11) },
            Cells = { 9, 10, 11, 12 },
            Good = { 2, 5, 6, 4 },
            Bad = { 3, 5, 6, 4 },
            Reason = "5+6+4 = 15 instead of 11."
        };

        public ASum(int cell, CellDirection dir, int sum) : base(cell) { Direction = dir; Sum = sum; }
        private ASum() { }    // for Classify

        public CellDirection Direction { get; private set; }
        public int Sum { get; private set; }

        protected override IEnumerable<Constraint> getConstraints()
        {
            var affectedCells = findCellsInDirection(Cell, Direction).ToArray();
            var combinations = PuzzleUtil.Combinations(1, 9, affectedCells.Length).Where(c => c[0] <= affectedCells.Length - 1 && c.Skip(1).Take(c[0]).Sum() == Sum).ToArray();
            yield return new CombinationsConstraint(affectedCells, combinations);
        }

        public override string Svg => $"<path d='M .5 -.15 .8 .1 H .2z' transform='translate({Cell % 9 + .5}, {Cell / 9 + .5}) rotate({90 * (int) Direction}) translate(-.5 -.5)' /><text x='{Direction switch { CellDirection.Left => 0, CellDirection.Right => 1, _ => .5 } + Cell % 9}' y='{Direction switch { CellDirection.Up => .06, CellDirection.Down => 1.06, _ => .56 } + Cell / 9}' font-size='.17' fill='white' text-anchor='middle'>{Sum}</text>";
        public override bool SvgAboveLines => true;

        public override bool Verify(int[] grid)
        {
            var value = grid[Cell];
            return
                inRange(Cell % 9 + dx(Direction) * value) &&
                inRange(Cell / 9 + dy(Direction) * value) &&
                Enumerable.Range(1, value).Sum(d => grid[Cell % 9 + dx(Direction) * d + 9 * (Cell / 9 + dy(Direction) * d)]) == Sum;
        }

        public static IList<SvgConstraint> Generate(int[] sudoku) => Enumerable.Range(0, 81)
            .SelectMany(c => EnumStrong.GetValues<CellDirection>()
                .Where(dir => inRange(c % 9 + dx(dir) * 2) && inRange(c / 9 + dy(dir) * 2))
                .Where(dir => inRange(c % 9 + dx(dir) * sudoku[c]) && inRange(c / 9 + dy(dir) * sudoku[c]))
                .Select(dir => new ASum(c, dir, Enumerable.Range(1, sudoku[c]).Sum(d => sudoku[c + dx(dir) * d + dy(dir) * 9 * d]))))
            .ToArray();
    }
}
