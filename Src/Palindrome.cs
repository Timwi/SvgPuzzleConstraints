using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PuzzleSolvers;
using RT.Util;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Palindrome")]
    public class Palindrome : SvgConstraint
    {
        public override string Description => "The digits along the line must form a palindrome (same sequence of digits when read from either end).";
        public static readonly Example Example = new()
        {
            Constraints = { new Palindrome(new[] { 19, 20, 12, 3 }) },
            Cells = { 19, 20, 12, 3 },
            Good = { 5, 2, 2, 5 },
            Bad = { 5, 2, 2, 1 },
            Reason = "The first and last digit are different."
        };

        public int[] Cells { get; private set; }

        public Palindrome(int[] cells) { Cells = cells; }
        private Palindrome() { }   // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new CloneConstraint(Cells.Subarray(0, Cells.Length / 2), Cells.Subarray((Cells.Length + 1) / 2).ReverseInplace()); }
        public sealed override bool IncludesCell(int cell) => Cells.Contains(cell);

        private string path => $"M{Cells.Select(c => $"{svgX(c)} {svgY(c)}").JoinString(" ")}";
        private string pathHash
        {
            get
            {
                using var md5 = MD5.Create();
                return md5.ComputeHash(path.ToUtf8()).ToHex();
            }
        }

        public override IEnumerable<string> SvgDefs
        {
            get
            {
                yield return $@"<mask id='capped-line-mask-{pathHash}'>
                    <rect fill='white' x='0' y='0' width='9' height='9' stroke='none' />
                    <path d='{path}' stroke='black' stroke-width='.1' stroke-linejoin='miter' fill='none' />
                </mask>";
            }
        }

        public override string Svg
        {
            get
            {
                static int angleDeg(int c1, int c2) => (c2 % 9 - c1 % 9, c2 / 9 - c1 / 9) switch { (-1, -1) => 225, (0, -1) => 270, (1, -1) => 315, (-1, 0) => 180, (1, 0) => 0, (-1, 1) => 135, (0, 1) => 90, (1, 1) => 45, _ => 10 };
                var f = Cells[0];
                var s = Cells[1];
                var sl = Cells[Cells.Length - 2];
                var l = Cells[Cells.Length - 1];
                return $@"<g opacity='.2'>
                    <path d='{path}' stroke='black' stroke-width='.3' stroke-linejoin='miter' fill='none' mask='url(#capped-line-mask-{pathHash})' />
                    <path d='M -.2 -.3 .4 0 -.2 .3z' fill='black' stroke='none' transform='translate({svgX(l)}, {svgY(l)}) rotate({angleDeg(sl, l)})' />
                    <path d='M -.2 -.3 .4 0 -.2 .3z' fill='black' stroke='none' transform='translate({svgX(f)}, {svgY(f)}) rotate({angleDeg(s, f)})' />
                </g>";
            }
        }

        public override bool Verify(int[] grid)
        {
            for (var i = 0; i < Cells.Length / 2; i++)
                if (grid[Cells[i]] != grid[Cells[Cells.Length - 1 - i]])
                    return false;
            return true;
        }

        public override bool ClashesWith(SvgConstraint other) => other switch
        {
            SvgCellConstraint cc => Cells.Contains(cc.Cell),
            Thermometer th => Cells.Intersect(th.Cells).Any(),
            Palindrome pali => Cells.Intersect(pali.Cells).Any(),
            _ => false,
        };

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var list = new List<SvgConstraint>();

            IEnumerable<Palindrome> recurse(int[] cells1, int[] cells2)
            {
                bool noDiagonalCrossingExists(int[] arr, int c1, int c2)
                {
                    var p1 = Array.IndexOf(arr, c1 % 9 + 9 * (c2 / 9));
                    var p2 = Array.IndexOf(arr, c2 % 9 + 9 * (c1 / 9));
                    return p1 == -1 || p2 == -1 || Math.Abs(p1 - p2) != 1;
                }

                if (cells1.Length >= 3)
                    yield return new Palindrome((PuzzleUtil.Adjacent(cells1[1]).Contains(cells2[1]) ? cells1.Skip(1) : cells1).Reverse().Concat(cells2.Skip(1)).ToArray());

                if (cells1.Length > 4)
                    yield break;

                foreach (var adj1 in PuzzleUtil.Adjacent(cells1.Last()))
                    if (!cells1.Contains(adj1) && !cells2.Contains(adj1) && noDiagonalCrossingExists(cells1, cells1.Last(), adj1))
                        foreach (var adj2 in PuzzleUtil.Adjacent(cells2.Last()))
                            if (adj2 != adj1 && sudoku[adj1] == sudoku[adj2] && !cells1.Contains(adj2) && !cells2.Contains(adj2) && noDiagonalCrossingExists(cells2, cells2.Last(), adj2))
                                foreach (var item in recurse(cells1.Insert(cells1.Length, adj1), cells2.Insert(cells2.Length, adj2)))
                                    yield return item;
            }

            for (var startCell = 0; startCell < 81; startCell++)
                list.AddRange(recurse(new[] { startCell }, new[] { startCell }));
            return list;
        }
    }
}
