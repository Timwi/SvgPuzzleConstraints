using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Numbered room")]
    public class NumberedRoom : SvgRowColNumberConstraint
    {
        public override string Description => $"The Xth digit in this {(IsCol ? "column" : "row")} must be {Clue}, where X is the first digit in the {(IsCol ? "column" : "row")}.";
        public override double ExtraTop => IsCol && !Reverse ? .5 : 0;
        public override double ExtraRight => !IsCol && Reverse ? .25 : 0;
        public override double ExtraLeft => !IsCol && !Reverse ? .5 : 0;
        public override bool ShownTopLeft => !Reverse;

        public static readonly Example Example = new()
        {
            Constraints = { new NumberedRoom(false, 0, false, 2) },
            Cells = { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
            Good = { 4, 7, 6, 2, 1, 9, 8, 5, 3 },
            Bad = { 5, 7, 6, 2, 1, 9, 8, 4, 3 },
            Reason = "The first digit is 5; the 5th digit is not a 2.",
            Layout = ExampleLayout.Wide
        };

        public NumberedRoom(bool isCol, int rowCol, bool reverse, int clue, RowColDisplay display = RowColDisplay.Default)
            : base(isCol, rowCol, clue, display) { Reverse = reverse; }
        private NumberedRoom() { }    // for Classify

        public bool Reverse { get; private set; }

        protected override IEnumerable<Constraint> getConstraints() { yield return new IndexingConstraint(GetAffectedCells(Reverse), 0, Clue, offset: 1); }

        public override bool Verify(int[] grid) => GetAffectedCells(Reverse).Select(cell => grid[cell]).ToArray().Apply(numbers => numbers[0] > 0 && numbers[0] < numbers.Length && numbers[numbers[0] - 1] == Clue);

        public override IEnumerable<string> SvgDefs => base.SvgDefs.Concat(["""
            <linearGradient id="numbered-room" x1="64" x2="64" y1="96.202" y2="-11.015" gradientUnits="userSpaceOnUse">
                <stop offset="0" stop-color="#795548" />
                <stop offset="1" stop-color="#A1887F" />
            </linearGradient>
            """]);

        protected override string ElaborateSvg => $"""
            <g transform='translate({(IsCol ? RowCol : Reverse ? 8.8 : -.8)}, {(IsCol ? (Reverse ? 9 : -.8) : RowCol + .1)})'>
                <path fill="#4e342e" d="M100 4H28l.02 120h72.06z" />
                <path fill="url(#numbered-room)" d="m94 123.85-60 .13V10.55l60-.13z" />
                <path fill="#424242" d="m91.86,123.93h2.11V10.42h-60.09V123.93h2.11V12.43H91.86z" opacity=".2" />
                <path fill="#ffd54f" d="M51.71 72.44h-9a2.5 2.5 0 0 1 0-5h9a2.5 2.5 0 0 1 0 5z" />
            </g>
            """;

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var constraints = new List<SvgConstraint>();
            foreach (var isCol in new[] { false, true })
                foreach (var reverse in new[] { false, true })
                    for (var rowCol = 0; rowCol < 9; rowCol++)
                        if (sudoku[isCol ? (rowCol + 9 * (reverse ? 8 : 0)) : ((reverse ? 8 : 0) + 9 * rowCol)] - 1 is { } dist)
                            constraints.Add(new NumberedRoom(isCol, rowCol, reverse,
                                sudoku[isCol ? (rowCol + 9 * (reverse ? 8 - dist : dist)) : ((reverse ? 8 - dist : dist) + 9 * rowCol)]));
            return constraints;
        }
    }
}