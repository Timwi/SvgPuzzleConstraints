using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Find the 9")]
    public class FindThe9 : SvgCellConstraint
    {
        public override string Description => "The digit in this cell indicates how far in the indicated direction the 9 is.";

        public static readonly Example Example = new()
        {
            Constraints = { new FindThe9(9, CellDirection.Right) },
            Cells = { 9, 12 },
            Good = { 3, 9 },
            Bad = { 3, 8 },
            Reason = "The digit pointed to is not a 9."
        };

        public FindThe9(int cell, CellDirection dir) : base(cell) { Direction = dir; }
        private FindThe9() { }    // for Classify

        public CellDirection Direction { get; private set; }

        protected override IEnumerable<Constraint> getConstraints() { yield return new FindThe9Constraint(Cell, Direction); }

        public override string Svg => $"<path transform='translate({Cell % 9 + .5}, {Cell / 9 + .5}) rotate({90 * (int) Direction}) translate(-.5 -.5)' d='M .5 .1 .9 .5 .75 .5 .75 .9 .25 .9 .25 .5 .1 .5z' opacity='.2' />";

        public override bool Verify(int[] grid)
        {
            var value = grid[Cell];
            return
                inRange(Cell % 9 + dx(Direction) * value) &&
                inRange(Cell / 9 + dy(Direction) * value) &&
                grid[Cell % 9 + dx(Direction) * value + 9 * (Cell / 9 + dy(Direction) * value)] == 9;
        }

        public static IList<SvgConstraint> Generate(int[] sudoku) => Enumerable.Range(0, 81)
            .Where(c => sudoku[c] != 9)
            .SelectMany(c => EnumStrong.GetValues<CellDirection>()
                .Where(dir => inRange(c % 9 + dx(dir) * sudoku[c]) && inRange(c / 9 + dy(dir) * sudoku[c]) && sudoku[c + dx(dir) * sudoku[c] + dy(dir) * 9 * sudoku[c]] == 9)
                .Select(dir => new FindThe9(c, dir)))
            .ToArray();

        private sealed class FindThe9Constraint(int cell, CellDirection dir) : Constraint(findCellsInDirection(cell, dir))
        {
            public int Cell { get; private set; } = cell;
            public CellDirection Direction { get; private set; } = dir;

            public override ConstraintResult Process(SolverState state)
            {
                if (state.LastPlacedCell == null)
                {
                    // The focus cell cannot be so large that it points outside the grid,
                    // nor can it have a value that points at a cell that already contains something other than a 9
                    state.MarkImpossible(AffectedCells[0], v => v > AffectedCells.Length - 1 || state.IsImpossible(AffectedCells[v], 9));
                }
                else if (state.LastPlacedCell.Value == AffectedCells[0])
                {
                    // The focus cell has been set, therefore place the 9 in the correct position
                    state.MustBe(AffectedCells[state.LastPlacedValue], 9);
                    for (var i = 1; i < AffectedCells.Length; i++)
                        if (i != state.LastPlacedValue)
                            state.MarkImpossible(AffectedCells[i], 9);
                }
                else if (AffectedCells.Contains(state.LastPlacedCell.Value))
                {
                    var index = AffectedCells.IndexOf(state.LastPlacedCell.Value);
                    if (state.LastPlacedValue == 9)
                    {
                        // A 9 has been placed somewhere, therefore set the focus cell to the correct value
                        // (This is the main difference with FindTheValueConstraint; it’s an optimization that assumes uniqueness)
                        state.MustBe(AffectedCells[0], index);
                    }
                    else
                    {
                        // A value other than 9 has been placed somewhere, therefore the focus cell cannot point at it anymore
                        state.MarkImpossible(AffectedCells[0], index);
                    }
                }

                return null;
            }
        }
    }
}
