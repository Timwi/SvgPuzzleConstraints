using System;
using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;

namespace SvgPuzzleConstraints
{
    public abstract class SvgFourCellConstraint : SvgConstraint
    {
        public int TopLeftCell { get; private set; }

        public SvgFourCellConstraint(int topLeftCell) { TopLeftCell = topLeftCell; }
        protected SvgFourCellConstraint() { }   // for Classify

        protected abstract bool verify(int a, int b, int c, int d);

        protected sealed override IEnumerable<Constraint> getConstraints() { yield return new FourCellLambdaConstraint(TopLeftCell, TopLeftCell + 1, TopLeftCell + 10, TopLeftCell + 9, verify); }
        protected double x => svgX(TopLeftCell) + .5;
        protected double y => svgY(TopLeftCell) + .5;

        public override bool SvgAboveLines => true;
        public sealed override bool Verify(int[] grid) => verify(grid[TopLeftCell], grid[TopLeftCell + 1], grid[TopLeftCell + 10], grid[TopLeftCell + 9]);
        public sealed override bool IncludesCell(int cell) => cell == TopLeftCell || cell == TopLeftCell + 1 || cell == TopLeftCell + 10 || cell == TopLeftCell + 9;

        public override bool ClashesWith(SvgConstraint other) => other switch
        {
            SvgFourCellConstraint cn => cn.TopLeftCell == TopLeftCell,
            _ => false
        };

        protected static IList<SvgConstraint> generate(int[] sudoku, Func<int, int, int, int, int, IEnumerable<SvgConstraint>> constructor) =>
            Enumerable.Range(0, 81).Where(cell => cell % 9 < 8 && cell / 9 < 8).SelectMany(cell => constructor(cell, sudoku[cell], sudoku[cell + 1], sudoku[cell + 10], sudoku[cell + 9])).ToList();
    }
}
