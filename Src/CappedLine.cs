﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PuzzleSolvers;
using RT.Util;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Capped line")]
    public sealed class CappedLine : SvgConstraint
    {
        public override string Description => "The digits along the line must be numerically between the digits at the ends.";
        public static readonly Example Example = new()
        {
            Constraints = { new CappedLine(new[] { 9, 19, 20, 12, 3 }) },
            Cells = { 9, 19, 20, 12, 3 },
            Good = { 2, 5, 7, 3, 8 },
            Bad = { 3, 5, 7, 2, 8 },
            Reason = "The 2 is not between 3 and 8."
        };

        public int[] Cells { get; private set; }

        public CappedLine(int[] cells) { Cells = cells; }
        private CappedLine() { }   // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new BetweenLineConstraint(Cells[0], Cells[Cells.Length - 1], Cells.Skip(1).SkipLast(1).ToArray()); }

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
            var min = Math.Min(grid[Cells[0]], grid[Cells[Cells.Length - 1]]);
            var max = Math.Max(grid[Cells[0]], grid[Cells[Cells.Length - 1]]);
            return Cells.Skip(1).SkipLast(1).All(c => grid[c] > min && grid[c] < max);
        }
        public override bool IncludesCell(int cell) => Cells.Contains(cell);

        public override bool ClashesWith(SvgConstraint other) => other switch
        {
            SvgCellConstraint cc => Cells.Contains(cc.Cell),
            RenbanCage rb => Cells.Intersect(rb.Cells).Any(),
            Thermometer th => Cells.Intersect(th.Cells).Any(),
            Arrow ar => Cells.Intersect(ar.Cells).Any(),
            Palindrome pa => Cells.Intersect(pa.Cells).Any(),
            CappedLine cl => Cells.Intersect(cl.Cells).Any(),
            _ => false,
        };

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var list = new List<SvgConstraint>();

            IEnumerable<CappedLine> recurse(int[] sofar)
            {
                if (sofar.Length >= 3 && sofar.SkipLast(1).All(c => sudoku[c] < sudoku[sofar.Last()]))
                    yield return new CappedLine(sofar);
                if (sofar.Length >= 9)
                    yield break;

                bool noDiagonalCrossingExists(int x1, int y1, int x2, int y2)
                {
                    var p1 = Array.IndexOf(sofar, x1 + 9 * y2);
                    var p2 = Array.IndexOf(sofar, x2 + 9 * y1);
                    return p1 == -1 || p2 == -1 || Math.Abs(p1 - p2) != 1;
                }

                bool isSmallTurn(int x1, int y1, int x2, int y2, int x3, int y3)
                {
                    var dx = (x3 - x2) - (x2 - x1);
                    var dy = (y3 - y2) - (y2 - y1);
                    return (Math.Abs(dx) <= 1 && dy == 0) || (Math.Abs(dy) <= 1 && dx == 0);
                }

                var last = sofar[sofar.Length - 1];
                if (last == 9)
                    yield break;
                var secondLast = sofar.Length == 1 ? 0 : sofar[sofar.Length - 2];
                foreach (var adj in PuzzleUtil.Adjacent(last))
                    if (!sofar.Contains(adj) && sudoku[adj] > sudoku[sofar[0]] && noDiagonalCrossingExists(last % 9, last / 9, adj % 9, adj / 9)
                        && (sofar.Length == 1 || isSmallTurn(secondLast % 9, secondLast / 9, last % 9, last / 9, adj % 9, adj / 9)))
                        foreach (var next in recurse(sofar.Insert(sofar.Length, adj)))
                            yield return next;
            }

            for (var startCell = 0; startCell < 81; startCell++)
                list.AddRange(recurse(new[] { startCell }));
            return list;
        }
    }
}
