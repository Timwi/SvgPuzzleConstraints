using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PuzzleSolvers;

namespace SvgPuzzleConstraints
{
    public class BinairoCage : SvgRegionConstraint
    {
        public override string Description => "Within a cage, each row and each column has equal amounts of odd and even numbers. Further, the rows have unique arrangements of odds/evens, and the same is separately true of the columns.";
        public override bool SvgAboveLines => false;

        public int TopLeft { get; private set; }
        public int Size { get; private set; }

        public BinairoCage(int topLeft, int size) : base(Enumerable.Range(0, size * size).Select(ix => ix % size + topLeft % 9 + 9 * (ix / size + topLeft / 9)).ToArray())
        {
            if (Size % 2 != 0)
                throw new ArgumentException("Binairo cages must have an even size.", nameof(size));
            TopLeft = topLeft;
            Size = size;
        }
        private BinairoCage() { }   // for Classify

        public override string Svg => $@"<path d='{GenerateSvgPath(Cells, .06, .06, .35, .35)}' fill='none' stroke='black' stroke-width='.025' stroke-dasharray='.09,.07' />
            <g stroke='black' stroke-width='0.075' fill='none' transform='translate({TopLeft % 9 + .2}, {TopLeft / 9 + .2}) scale(.3)'><circle cx='0.25' cy='-0.25' r='0.2' /><circle cx='-0.25' cy='0.25' r='0.2' /><path d='m -0.25 -0.45 v 0.4 m 0.5 0.1 v 0.4' /></g>";

        public override bool Verify(int[] grid)
        {
            bool verifyRowOrCol(Func<int, int, bool> isOdd)
            {
                var arrangements = new int[Size];
                for (var y = 0; y < Size; y++)
                {
                    var numOdd = 0;
                    for (var x = 0; x < Size; x++)
                    {
                        if (isOdd(x, y))
                        {
                            numOdd++;
                            arrangements[y] |= (1 << x);
                        }
                        if (x > 2 && isOdd(x - 2, y) == isOdd(x - 1, y) && isOdd(x - 1, y) == isOdd(x, y))
                            return false;
                    }
                    if (numOdd != Size / 2 || arrangements.Take(y).Contains(arrangements[y]))
                        return false;
                }
                return true;
            }
            var ox = TopLeft % 9;
            var oy = TopLeft / 9;
            return verifyRowOrCol((i, j) => grid[i + ox + 9 * (j + oy)] % 2 != 0)
                && verifyRowOrCol((i, j) => grid[j + ox + 9 * (i + oy)] % 2 != 0);
        }

        protected override IEnumerable<Constraint> getConstraints()
        {
            // Rows and columns
            for (var i = 0; i < Size; i++)
            {
                yield return new ParityNoTripletsConstraint(Enumerable.Range(0, Size).Select(j => TopLeft % 9 + j + 9 * (TopLeft / 9 + i)));
                yield return new ParityEvennessConstraint(Enumerable.Range(0, Size).Select(j => TopLeft % 9 + j + 9 * (TopLeft / 9 + i)));
                yield return new ParityNoTripletsConstraint(Enumerable.Range(0, Size).Select(j => TopLeft % 9 + i + 9 * (TopLeft / 9 + j)));
                yield return new ParityEvennessConstraint(Enumerable.Range(0, Size).Select(j => TopLeft % 9 + i + 9 * (TopLeft / 9 + j)));
            }

            // Unique arrangements
            yield return new ParityUniqueRowsColumnsConstraint(Size, Enumerable.Range(0, Size * Size).Select(i => TopLeft % 9 + i % Size + 9 * (TopLeft / 9 + i / Size)));
        }

        public override bool ClashesWith(SvgConstraint other) => other switch
        {
            BinairoCage bc => bc.Cells.Intersect(Cells).Any(),
            KillerCage kc => kc.Cells.Intersect(Cells).Any(),
            _ => false
        };

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var list = new List<SvgConstraint>();
            for (var size = 4; size <= 8; size += 2)
                for (var x = 0; x <= 9 - size; x++)
                    for (var y = 0; y <= 9 - size; y++)
                    {
                        var bin = new BinairoCage(x + 9 * y, size);
                        if (bin.Verify(sudoku))
                            list.Add(bin);
                    }
            return list;
        }
    }
}
