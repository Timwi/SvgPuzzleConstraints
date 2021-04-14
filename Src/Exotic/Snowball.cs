using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PuzzleSolvers;
using RT.Util;
using RT.Util.ExtensionMethods;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Snowball")]
    public class Snowball : SvgConstraint
    {
        public override string Description => "One of the regions must contain the same digits in the same places as the other, plus or minus a consistent addend. For example, if one region contains 1, 4, 7, the other might contain 3, 6, 9 in the same order. (The digits within one region need not necessarily be different. The addend can be zero.)";
        public static readonly Example Example = new()
        {
            Constraints = { new Snowball(new[] { 0, 1, 9 }, new[] { 11, 12, 20 }) },
            Cells = { 0, 1, 9, 11, 12, 20 },
            Good = { 1, 4, 7, 3, 6, 9 },
            Bad = { 1, 4, 7, 3, 5, 8 },
            Reason = "3 is 2 more than 1, but 5 and 8 are only 1 more than 4 and 7."
        };

        public int[] Cells1 { get; private set; }
        public int[] Cells2 { get; private set; }
        public Snowball(int[] cells1, int[] cells2) { Cells1 = cells1; Cells2 = cells2; }
        private Snowball() { }      // for Classify

        protected override IEnumerable<Constraint> getConstraints() { yield return new OffsetCloneConstraint(Cells1, Cells2); }
        public sealed override bool IncludesCell(int cell) => Cells1.Contains(cell) || Cells2.Contains(cell);

        private static string hash(string path)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(path.ToUtf8()).ToHex();
        }

        public override IEnumerable<string> SvgDefs
        {
            get
            {
                yield return "<filter id='snowball-filter' color-interpolation-filters='sRGB'><feGaussianBlur result='fbSourceGraphic' stdDeviation='.03' /></filter>";
                var path1 = GenerateSvgPath(Cells1, 0, 0);
                yield return $"<clipPath id='snowball-clip-{hash(path1)}' clipPathUnits='userSpaceOnUse'><path d='{path1}' /></clipPath>";
                var path2 = GenerateSvgPath(Cells2, 0, 0);
                yield return $"<clipPath id='snowball-clip-{hash(path2)}' clipPathUnits='userSpaceOnUse'><path d='{path2}' /></clipPath>";
            }
        }

        public override string Svg
        {
            get
            {
                var path1 = GenerateSvgPath(Cells1, 0, 0);
                var path2 = GenerateSvgPath(Cells2, 0, 0);
                return
                    $"<path d='{path1}' fill='none' stroke='#666' stroke-width='.08' filter='url(#snowball-filter)' clip-path='url(#snowball-clip-{hash(path1)})' />" +
                    $"<path d='{path2}' fill='none' stroke='#666' stroke-width='.08' filter='url(#snowball-filter)' clip-path='url(#snowball-clip-{hash(path2)})' />";
            }
        }

        public override bool Verify(int[] grid) => Cells1.Select((c1, ix) => grid[Cells2[ix]] - grid[c1]).Distinct().ToArray().Length == 1;

        public override bool ClashesWith(SvgConstraint other) => other switch
        {
            RenbanCage => false,
            Snowball sn => sn.Cells1.Length == Cells1.Length || sn.Cells1.Concat(sn.Cells2).Intersect(Cells1.Concat(Cells2)).Any(),
            SvgRegionConstraint kr => Cells1.Concat(Cells2).Intersect(kr.Cells).Any(),
            _ => false
        };

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            IEnumerable<Snowball> generateRegions(bool[] sofar1, bool[] sofar2, int dx, int dy, int offset, bool[] banned, int count)
            {
                if (count >= 2)
                    yield return new Snowball(sofar1.SelectIndexWhere(b => b).ToArray(), sofar2.SelectIndexWhere(b => b).ToArray());

                for (var adj = 0; adj < 81; adj++)
                {
                    if (banned[adj] || adj % 9 + dx < 0 || adj % 9 + dx >= 9 || adj / 9 + dy < 0 || adj / 9 + dy >= 9 || !PuzzleUtil.Orthogonal(adj).Any(a => sofar1[a]) || sudoku[adj] + offset != sudoku[adj + dx + 9 * dy])
                        continue;
                    sofar1[adj] = true;
                    sofar2[adj + dx + 9 * dy] = true;
                    banned[adj] = true;
                    banned[adj + dx + 9 * dy] = true;
                    foreach (var item in generateRegions(sofar1, sofar2, dx, dy, offset, (bool[]) banned.Clone(), count + 1))
                        yield return item;
                    sofar1[adj] = false;
                    sofar2[adj + dx + 9 * dy] = false;
                    banned[adj + dx + 9 * dy] = false;
                }
            }
            var list = new List<SvgConstraint>();
            for (var s1 = 0; s1 < 81; s1++)
                for (var s2 = s1 + 1; s2 < 81; s2++)
                    list.AddRange(generateRegions(Ut.NewArray(81, x => x == s1), Ut.NewArray(81, x => x == s2), s2 % 9 - s1 % 9, s2 / 9 - s1 / 9, sudoku[s2] - sudoku[s1], Ut.NewArray(81, x => x <= s1 || x == s2), 1));
            return list;
        }
    }
}
