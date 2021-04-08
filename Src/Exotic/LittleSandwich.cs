using System;
using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using PuzzleSolvers.Exotic;
using RT.Serialization;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Diagonal sandwich")]
    public class LittleSandwich : SvgDiagonalConstraint
    {
        public override string Description => $"Within this diagonal, there is exactly one {Digit1} and one {Digit2} and the digits sandwiched between them must add up to {Clue}. The {Digit1} and {Digit2} can occur in either order.";

#warning Add Example

        public int Digit1 { get; private set; }
        public int Digit2 { get; private set; }
        [ClassifyIgnoreIfDefault]
        public bool OmitCrust { get; private set; }

        public LittleSandwich(DiagonalDirection direction, int offset, int clue, int digit1 = 1, int digit2 = 9, bool opposite = false, DiagonalDisplay display = DiagonalDisplay.Default)
            : base(direction, offset, clue, opposite, display)
        {
            Digit1 = digit1;
            Digit2 = digit2;
        }
        private LittleSandwich() { }  // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new LittleSandwichConstraint(AffectedCells, Clue, Digit1, Digit2); }
        public override bool Verify(int[] grid)
        {
            var p1 = AffectedCells.IndexOf(ix => grid[ix] == Digit1);
            var p2 = AffectedCells.IndexOf(ix => grid[ix] == Digit2);
            if (p1 == -1 || p2 == -1)
                return false;
            return AffectedCells.Skip(p1 + 1).Take(p2 - 1 - 1).Sum(ix => grid[ix]) == Clue;
        }

#warning Implement LittleSandwich.ElaborateSvg
        protected override string ElaborateSvg => throw new NotImplementedException();

#warning Implement LittleSandwich.Generate()
        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            throw new NotImplementedException();
        }
    }
}
