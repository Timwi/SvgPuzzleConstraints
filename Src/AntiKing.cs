﻿using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Anti-king")]
    public class AntiKing : SvgCellConstraint
    {
        public override string Description => "The same digit can’t be a king’s move away (orthogonally or diagonally adjacent) from this digit.";
        public static readonly Example Example = new()
        {
            Constraints = { new AntiKing(20) },
            Cells = { 12, 20 },
            Good = { 8, 5 },
            Bad = { 5, 5 }
        };

        public AntiKing(int cell) : base(cell) { }
        private AntiKing() { }    // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new AntiKingConstraint(9, 9, enforcedCells: new[] { Cell }); }
        public override string Svg => $@"<path fill='rgba(0, 0, 0, .2)' d='{PathD}' transform='translate({Cell % 9}, {Cell / 9}) scale(.01)' />";
        private static readonly string PathD = @"m 61.406248,32.500001 q 5.78125,-6.09375 15.572917,-6.09375 7.604166,0 12.447916,4.427084 4.84375,4.427083 4.84375,11.249999 0,13.229167 -15.46875,21.666666 v 20.46875 q 0,4.166667 -9.218749,6.979166 -9.21875,2.864584 -19.583333,2.864584 -10.416667,0 -19.635416,-2.864584 -9.166667,-2.812499 -9.166667,-6.979166 V 63.75 Q 5.7291665,55.312501 5.7291665,42.083334 q 0,-6.822916 4.8437495,-11.249999 4.84375,-4.427084 12.447917,-4.427084 9.739583,0 15.572916,6.09375 2.916667,-5.15625 7.65625,-7.03125 H 39.739582 V 5.9375018 H 60.260415 V 25.468751 h -6.510416 q 4.739583,1.875 7.656249,7.03125 z M 54.687499,8.9062517 h -9.375 l 4.6875,4.6875003 z M 57.239582,20.885418 V 10.625002 l -5.104167,5.104166 z m -9.375,-5.15625 -5.104166,-5.104166 v 10.260416 z m 6.822917,6.822917 -4.6875,-4.6875 -4.6875,4.6875 z m 4.375,12.083333 q -1.25,-3.020833 -3.854167,-5.104167 -2.604167,-2.135416 -5.208333,-2.135416 -2.604167,0 -5.208333,2.135416 -2.604167,2.083334 -3.854167,5.104167 2.1875,2.135416 5,7.447916 2.864583,5.3125 4.0625,9.947917 1.197916,-4.635417 4.010416,-9.947917 2.8125,-5.3125 5.052084,-7.447916 z M 76.354165,61.71875 q 7.03125,-3.489583 10.9375,-8.645833 3.958333,-5.208333 3.958333,-10.989583 0,-5.78125 -3.958333,-9.21875 -3.90625,-3.437499 -10.3125,-3.437499 -17.34375,0 -24.53125,25.364582 15.3125,0.677084 23.90625,6.927083 z M 47.552082,54.791667 q -7.1875,-25.364582 -24.531249,-25.364582 -6.40625,0 -10.364583,3.489583 -3.9583336,3.4375 -3.9583336,9.166666 0,5.78125 3.9583336,10.989583 3.958333,5.15625 10.989583,8.645833 8.541666,-6.249999 23.906249,-6.927083 z M 75.833331,78.125 v -2.34375 l -4.062499,-6.25 4.062499,-2.5 v -1.5625 q -2.656249,-3.489583 -9.843749,-5.572916 -7.135417,-2.135417 -15.989583,-2.135417 -8.854167,0 -16.041667,2.135417 -7.135416,2.083333 -9.791666,5.572916 v 1.5625 l 4.0625,2.5 -4.0625,6.25 V 78.125 q 10.625,-5.833333 25.833333,-5.833333 15.208333,0 25.833332,5.833333 z m -19.218749,-13.541666 -6.614583,4.6875 -6.614583,-4.6875 6.614583,-4.166667 z m -6.614583,26.458332 q 7.760416,0 16.770833,-2.499999 9.062499,-2.447917 9.062499,-5.364584 0,-2.604166 -8.229166,-5.260416 -8.177083,-2.65625 -17.604166,-2.65625 -9.427083,0 -17.65625,2.65625 -8.177083,2.65625 -8.177083,5.260416 0,2.916667 9.010416,5.364584 9.0625,2.499999 16.822917,2.499999 z";

        public override bool Verify(int[] grid)
        {
            foreach (var c in AntiKingConstraint.KingsMoves(Cell, 9, 9))
                if (grid[c] == grid[Cell])
                    return false;
            return true;
        }

        public static IList<SvgConstraint> Generate(int[] sudoku) => Enumerable.Range(0, 81)
            .Where(cell => AntiKingConstraint.KingsMoves(cell, 9, 9).All(c => sudoku[c] != sudoku[cell]))
            .Select(cell => new AntiKing(cell))
            .ToArray();
    }
}
