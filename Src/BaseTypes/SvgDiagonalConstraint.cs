using System.Linq;
using RT.Serialization;

namespace SvgPuzzleConstraints
{
    public abstract class SvgDiagonalConstraint : SvgConstraint
    {
        public override double ExtraRight => Direction == DiagonalDirection.SouthWest || (Direction == DiagonalDirection.NorthWest && Offset == 0) ? .5 : 0;
        public override double ExtraTop => Direction == DiagonalDirection.SouthEast || (Direction == DiagonalDirection.SouthWest && Offset == 0) ? .5 : 0;
        public override double ExtraLeft => Direction == DiagonalDirection.NorthEast || (Direction == DiagonalDirection.SouthEast && Offset == 0) ? .5 : 0;

        public override bool IncludesCell(int cell) => false;
        public override bool IncludesRowCol(bool isCol, int rowCol, bool topLeft) => Direction switch
        {
            DiagonalDirection.SouthEast => isCol && rowCol == Offset - 1 && topLeft,
            DiagonalDirection.SouthWest => !isCol && rowCol == Offset - 1 && !topLeft,
            DiagonalDirection.NorthWest => isCol && rowCol == 9 - Offset && !topLeft,
            DiagonalDirection.NorthEast => !isCol && rowCol == 9 - Offset && topLeft,
            _ => false
        };

        public DiagonalDirection Direction { get; private set; }
        public int Offset { get; private set; }
        public int Clue { get; private set; }
        [ClassifyIgnoreIfDefault]
        public bool Opposite { get; private set; }

        public SvgDiagonalConstraint(DiagonalDirection dir, int offset, int clue, bool opposite = false)
        {
            Direction = dir;
            Offset = offset;
            Clue = clue;
            Opposite = opposite;
        }
        protected SvgDiagonalConstraint() { }  // for Classify

        public int[] AffectedCells => GetAffectedCells(Direction, Offset);
        protected static int[] GetAffectedCells(DiagonalDirection dir, int offset) => dir switch
        {
            DiagonalDirection.SouthEast => Enumerable.Range(0, 9 - offset).Select(i => offset + 10 * i).ToArray(),
            DiagonalDirection.SouthWest => Enumerable.Range(0, 9 - offset).Select(i => 8 + 9 * offset + 8 * i).ToArray(),
            DiagonalDirection.NorthWest => Enumerable.Range(0, 9 - offset).Select(i => 80 - offset - 10 * i).ToArray(),
            DiagonalDirection.NorthEast => Enumerable.Range(0, 9 - offset).Select(i => 72 - 9 * offset - 8 * i).ToArray(),
            _ => null,
        };

        protected const double svgArrLen = .275;
        protected const double svgArrWidth = .2;
        protected const double svgMargin = .1;

        protected string ArrowSvg => Opposite
            ? Direction switch
            {
                DiagonalDirection.SouthEast => $"<path d='M {9 + svgMargin + svgArrLen} {9 - Offset + svgMargin + svgArrLen} {9 + svgMargin} {9 - Offset + svgMargin} M {9 + svgMargin} {9 - Offset + svgMargin + svgArrWidth} v {-svgArrWidth} h {svgArrWidth}' stroke='black' stroke-width='.02' fill='none' />",
                DiagonalDirection.SouthWest => $"<path d='M {Offset - svgMargin - svgArrLen} {9 + svgMargin + svgArrLen} {Offset - svgMargin} {9 + svgMargin} M {Offset - svgMargin - svgArrWidth} {9 + svgMargin} h {svgArrWidth} v {svgArrWidth}' stroke='black' stroke-width='.02' fill='none' />",
                DiagonalDirection.NorthWest => $"<path d='M {-svgMargin - svgArrLen} {Offset - svgMargin - svgArrLen} {-svgMargin} {Offset - svgMargin} M {-svgMargin - svgArrWidth} {Offset - svgMargin} h {svgArrWidth} v {-svgArrWidth}' stroke='black' stroke-width='.02' fill='none' />",
                DiagonalDirection.NorthEast => $"<path d='M {9 - Offset + svgMargin + svgArrLen} {-svgMargin - svgArrLen} {9 - Offset + svgMargin} {-svgMargin} M {9 - Offset + svgMargin} {-svgMargin - svgArrWidth} v {svgArrWidth} h {svgArrWidth}' stroke='black' stroke-width='.02' fill='none' />",
                _ => null
            }
            : Direction switch
            {
                DiagonalDirection.SouthEast => $"<path d='M {Offset - svgMargin - svgArrLen} {-svgMargin - svgArrLen} {Offset - svgMargin} {-svgMargin} M {Offset - svgMargin - svgArrWidth} {-svgMargin} h {svgArrWidth} v {-svgArrWidth}' stroke='black' stroke-width='.02' fill='none' />",
                DiagonalDirection.SouthWest => $"<path d='M {9 + svgMargin + svgArrLen} {Offset - svgMargin - svgArrLen} {9 + svgMargin} {Offset - svgMargin} M {9 + svgMargin} {Offset - svgMargin - svgArrWidth} v {svgArrWidth} h {svgArrWidth}' stroke='black' stroke-width='.02' fill='none' />",
                DiagonalDirection.NorthWest => $"<path d='M {9 - Offset + svgMargin + svgArrLen} {9 + svgMargin + svgArrLen} {9 - Offset + svgMargin} {9 + svgMargin} M {9 - Offset + svgMargin + svgArrWidth} {9 + svgMargin} h {-svgArrWidth} v {svgArrWidth}' stroke='black' stroke-width='.02' fill='none' />",
                DiagonalDirection.NorthEast => $"<path d='M {-svgMargin - svgArrLen} {9 - Offset + svgMargin + svgArrLen} {-svgMargin} {9 - Offset + svgMargin} M {-svgMargin - svgArrWidth} {9 - Offset + svgMargin} h {svgArrWidth} v {svgArrWidth}' stroke='black' stroke-width='.02' fill='none' />",
                _ => null
            };

        protected string NumberSvg => Opposite
            ? Direction switch
            {
                DiagonalDirection.SouthEast => $"<text text-anchor='middle' x='{9 + svgMargin + svgArrLen + svgMargin}' y='{9 - Offset + svgMargin + svgArrLen + svgMargin + .2}' font-size='.35'>{Clue}</text>",
                DiagonalDirection.SouthWest => $"<text text-anchor='middle' x='{Offset - svgMargin - svgArrLen - svgMargin}' y='{9 + svgMargin + svgArrLen + svgMargin + .2}' font-size='.35'>{Clue}</text>",
                DiagonalDirection.NorthWest => $"<text text-anchor='middle' x='{-svgMargin - svgArrLen - svgMargin}' y='{Offset - svgMargin - svgArrLen - svgMargin + .05}' font-size='.35'>{Clue}</text>",
                DiagonalDirection.NorthEast => $"<text text-anchor='middle' x='{9 - Offset + svgMargin + svgArrLen + svgMargin}' y='{-svgMargin - svgArrLen - svgMargin + .05}' font-size='.35'>{Clue}</text>",
                _ => null
            }
            : Direction switch
            {
                DiagonalDirection.SouthEast => $"<text text-anchor='middle' x='{Offset - svgMargin - svgArrLen - svgMargin}' y='{-svgMargin - svgArrLen - svgMargin + .05}' font-size='.35'>{Clue}</text>",
                DiagonalDirection.SouthWest => $"<text text-anchor='middle' x='{9 + svgMargin + svgArrLen + svgMargin}' y='{Offset - svgMargin - svgArrLen - svgMargin + .05}' font-size='.35'>{Clue}</text>",
                DiagonalDirection.NorthWest => $"<text text-anchor='middle' x='{9 - Offset + svgMargin + svgArrLen + svgMargin}' y='{9 + svgMargin + svgArrLen + svgMargin + .2}' font-size='.35'>{Clue}</text>",
                DiagonalDirection.NorthEast => $"<text text-anchor='middle' x='{-svgMargin - svgArrLen - svgMargin}' y='{9 - Offset + svgMargin + svgArrLen + svgMargin + .2}' font-size='.35'>{Clue}</text>",
                _ => null
            };

        public override bool ClashesWith(SvgConstraint other) => other switch
        {
            SvgRowColConstraint rc => IncludesRowCol(rc.IsCol, rc.RowCol, rc.ShownTopLeft),
            SvgDiagonalConstraint lk => lk.Direction == Direction && lk.Offset == Offset,
            _ => false
        };
    }
}
