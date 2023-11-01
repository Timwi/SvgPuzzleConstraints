using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PuzzleSolvers;
using RT.Util;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("German whisper")]
    public class GermanWhisper : SvgConstraint
    {
        public override string Description => "Adjacent digits along the line must have a difference of at least 5.";
        public static readonly Example Example = new()
        {
            Constraints = { new GermanWhisper(new[] { 18, 10, 20, 12, 3 }) },
            Cells = { 18, 10, 20, 12, 3 },
            Good = { 7, 1, 8, 2, 7 },
            Bad = { 7, 3, 8, 2, 7 },
            Reason = "The first two digits (7 and 3) have a difference of only 4."
        };

        public int[] Cells { get; private set; }

        public GermanWhisper(int[] cells) { Cells = cells; }
        private GermanWhisper() { }   // for Classify

        protected override IEnumerable<Constraint> getConstraints()
        {
            for (var i = 1; i < Cells.Length; i++)
                yield return new TwoCellLambdaConstraint(Cells[i - 1], Cells[i], (a, b) => Math.Abs(a - b) >= 5);
        }
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
                yield return $@"<mask id='german-whisper-{pathHash}' maskUnits='userSpaceOnUse'>
                    <path d='{path}' stroke-width='.35' stroke-linecap='round' stroke-linejoin='round' stroke='white' fill='none' />
                    {Cells.Select(c => $"<circle fill='black' cx='{svgX(c)}' cy='{svgY(c)}' r='.1' />").JoinString()}
                    {Enumerable.Range(0, Cells.Length - 1).Select(ix =>
                        Math.Abs(Cells[ix + 1] % 9 - Cells[ix] % 9) != 0 && Math.Abs(Cells[ix + 1] / 9 - Cells[ix] / 9) != 0
                            ? $@"
                                <circle fill='black' cx='{(3 * svgX(Cells[ix]) + svgX(Cells[ix + 1])) / 4}' cy='{(3 * svgY(Cells[ix]) + svgY(Cells[ix + 1])) / 4}' r='.1' />
                                <circle fill='black' cx='{(svgX(Cells[ix]) + svgX(Cells[ix + 1])) / 2}' cy='{(svgY(Cells[ix]) + svgY(Cells[ix + 1])) / 2}' r='.1' />
                                <circle fill='black' cx='{(svgX(Cells[ix]) + 3 * svgX(Cells[ix + 1])) / 4}' cy='{(svgY(Cells[ix]) + 3 * svgY(Cells[ix + 1])) / 4}' r='.1' />"
                            : $@"
                                <circle fill='black' cx='{(2 * svgX(Cells[ix]) + svgX(Cells[ix + 1])) / 3}' cy='{(2 * svgY(Cells[ix]) + svgY(Cells[ix + 1])) / 3}' r='.1' />
                                <circle fill='black' cx='{(svgX(Cells[ix]) + 2 * svgX(Cells[ix + 1])) / 3}' cy='{(svgY(Cells[ix]) + 2 * svgY(Cells[ix + 1])) / 3}' r='.1' />").JoinString()}
                    </mask>";
            }
        }

        public override string Svg => $"<g opacity='.2'><path d='{path}' mask='url(#german-whisper-{pathHash})' fill='none' stroke='black' stroke-width='.3' stroke-linecap='round' stroke-linejoin='round' /></g>";

        public override bool Verify(int[] grid)
        {
            for (var i = 1; i < Cells.Length; i++)
                if (Math.Abs(grid[Cells[i - 1]] - grid[Cells[i]]) < 5)
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

            IEnumerable<GermanWhisper> recurse(int[] sofar)
            {
                if (sofar.Length >= 3)
                    yield return new GermanWhisper(sofar);
                if (sofar.Length >= 9)
                    yield break;

                bool noDiagonalCrossingExists(int x1, int y1, int x2, int y2)
                {
                    var p1 = Array.IndexOf(sofar, x1 + 9 * y2);
                    var p2 = Array.IndexOf(sofar, x2 + 9 * y1);
                    return p1 == -1 || p2 == -1 || Math.Abs(p1 - p2) != 1;
                }

                var last = sofar[sofar.Length - 1];
                foreach (var adj in PuzzleUtil.Adjacent(last))
                    if (!sofar.Contains(adj) && Math.Abs(sudoku[adj] - sudoku[last]) >= 5 && noDiagonalCrossingExists(last % 9, last / 9, adj % 9, adj / 9))
                        foreach (var next in recurse(sofar.Insert(sofar.Length, adj)))
                            yield return next;
            }

            for (var startCell = 0; startCell < 81; startCell++)
                list.AddRange(recurse(new[] { startCell }));
            return list;
        }
    }
}
