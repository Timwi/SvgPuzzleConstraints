﻿using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Odd/even")]
    public class OddEven : SvgCellConstraint
    {
        public override string Description => $"The digit in this cell must be {(Odd ? "odd" : "even")}.";
        public static readonly Example Example = new Example
        {
            Constraints = { new OddEven(11, true), new OddEven(20, false) },
            Cells = { 11, 20 },
            Good = { 3, 6 },
            Bad = { 8, 1 }
        };

        public bool Odd { get; private set; }

        public OddEven(int cell, bool odd) : base(cell) { Odd = odd; }
        private OddEven() { }    // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new OneCellLambdaConstraint(Cell, v => v % 2 == (Odd ? 1 : 0)); }
        public override string Svg => Odd
            ? $@"<circle cx='{Cell % 9 + .5}' cy='{Cell / 9 + .5}' r='.4' fill='rgba(0, 0, 0, .1)' />"
            : $@"<rect x='{Cell % 9 + .1}' y='{Cell / 9 + .1}' width='.8' height='.8' fill='rgba(0, 0, 0, .1)' />";
        public override bool Verify(int[] grid) => grid[Cell] % 2 == (Odd ? 1 : 0);

        public static IList<SvgConstraint> Generate(int[] sudoku) =>
            Enumerable.Range(0, 81).Select(cell => new OddEven(cell, sudoku[cell] % 2 != 0)).ToArray();
    }
}
