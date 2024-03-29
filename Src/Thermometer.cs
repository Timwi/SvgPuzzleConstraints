﻿using System;
using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Thermometer")]
    public class Thermometer : SvgConstraint
    {
        public override string Description => "Digits must increase from the bulb.";
        public static readonly Example Example = new()
        {
            Constraints = { new Thermometer(new[] { 18, 10, 20, 12, 3 }) },
            Cells = { 18, 10, 20, 12, 3 },
            Good = { 2, 4, 7, 8, 9 },
            Bad = { 2, 7, 4, 8, 9 },
            Reason = "4 is less than 7."
        };

        public int[] Cells { get; private set; }

        public Thermometer(int[] cells) { Cells = cells; }
        private Thermometer() { }   // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new LessThanConstraint(Cells); }
        public sealed override bool IncludesCell(int cell) => Cells.Contains(cell);

        public override string Svg => $@"<g opacity='.2'>
            <path d='M{Cells.Select(c => $"{c % 9 + .5} {c / 9 + .5}").JoinString(" ")}' stroke='black' stroke-width='.3' stroke-linecap='round' stroke-linejoin='round' fill='none' />
            <circle cx='{svgX(Cells[0])}' cy='{svgY(Cells[0])}' r='.4' fill='black' />
        </g>";

        public override bool Verify(int[] grid)
        {
            for (var i = 1; i < Cells.Length; i++)
                if (grid[Cells[i]] <= grid[Cells[i - 1]])
                    return false;
            return true;
        }

        public override bool ClashesWith(SvgConstraint other) => other switch
        {
            SvgCellConstraint cc => Cells.Contains(cc.Cell),
            Thermometer th => Cells.Intersect(th.Cells).Any(),
            Palindrome pali => Cells.Intersect(pali.Cells).Any(),
            GermanWhisper gw => Cells.Intersect(gw.Cells).Any(),
            Arrow ar => Cells.Intersect(ar.Cells).Any(),
            _ => false,
        };

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var list = new List<SvgConstraint>();

            IEnumerable<Thermometer> recurse(int[] sofar)
            {
                if (sofar.Length >= 3)
                    yield return new Thermometer(sofar);

                bool noDiagonalCrossingExists(int x1, int y1, int x2, int y2)
                {
                    var p1 = Array.IndexOf(sofar, x1 + 9 * y2);
                    var p2 = Array.IndexOf(sofar, x2 + 9 * y1);
                    return p1 == -1 || p2 == -1 || Math.Abs(p1 - p2) != 1;
                }

                var x = sofar.Last() % 9;
                var y = sofar.Last() / 9;
                for (var xx = x - 1; xx <= x + 1; xx++)
                    if (xx >= 0 && xx < 9)
                        for (var yy = y - 1; yy <= y + 1; yy++)
                            if (yy >= 0 && yy < 9 && !sofar.Contains(xx + 9 * yy) && sudoku[xx + 9 * yy] > sudoku[sofar.Last()]
                                && (xx == x || yy == y || noDiagonalCrossingExists(x, y, xx, yy)))
                                foreach (var next in recurse(sofar.Insert(sofar.Length, xx + 9 * yy)))
                                    yield return next;
            }

            for (var startCell = 0; startCell < 81; startCell++)
                list.AddRange(recurse(new[] { startCell }));
            return list;
        }
    }
}
