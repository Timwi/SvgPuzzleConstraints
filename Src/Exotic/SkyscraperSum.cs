using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Skyscraper Sum")]
    public class SkyscraperSum : SvgRowColNumberConstraint
    {
        public override string Description => $"Within this {(IsCol ? "column" : "row")}, the digits represent skyscrapers, where taller ones obstruct the view of smaller ones behind them. The heights of the skyscrapers visible from the clue sum up to {Clue}.";
        public override double ExtraTop => IsCol && !Reverse ? .5 : 0;
        public override double ExtraRight => !IsCol && Reverse ? .8 : 0;
        public override double ExtraLeft => !IsCol && !Reverse ? .8 : 0;
        public override bool ShownTopLeft => !Reverse;
        public static readonly Example Example = new()
        {
            Constraints = { new SkyscraperSum(false, 0, false, 38) },
            Cells = { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
            Good = { 3, 5, 6, 4, 7, 2, 8, 9, 1 },
            Bad = { 4, 1, 6, 5, 2, 7, 3, 8, 9 },
            Reason = "The obstructed skyscrapers are 1, 5, 2, and 3. The rest sum up to 4+6+7+8+9 = 34.",
            Wide = true
        };

        public SkyscraperSum(bool isCol, int rowCol, bool reverse, int clue, RowColDisplay display = RowColDisplay.Default)
            : base(isCol, rowCol, clue, display) { Reverse = reverse; }
        private SkyscraperSum() { }    // for Classify

        public bool Reverse { get; private set; }

        protected override IEnumerable<Constraint> getConstraints() { yield return new SkyscraperSumUniquenessConstraint(Clue, GetAffectedCells(Reverse)); }

        public override bool Verify(int[] grid) => SkyscraperSumUniquenessConstraint.CalculateSkyscraperSumClue(GetAffectedCells(Reverse).Select(cell => grid[cell])) == Clue;

        public override IEnumerable<string> SvgDefs
        {
            get
            {
                yield return @"<linearGradient id='skyscraper-2' x1='0' x2='0' y1='0' y2='1.5'><stop offset='0' stop-color='#fff'/><stop offset='1' stop-color='#fff' stop-opacity='0'/></linearGradient>";
                yield return @"<mask id='skyscraper-1'><path fill='url(#skyscraper-2)' d='M10.25 10.25L19.75.5V90h-9.5zM20.25.5l28.5 9.75V90h-28.5zM51.25 25.25l9.5-9.75V90h-9.5zM61.25 15.5l28.5 9.75V90h-28.5z'/></mask>";
                yield return @"<mask id='skyscraper-3'><path fill='#fff' d='M10.25 10.25L19.75.5V90h-9.5zM20.25.5l28.5 9.75V90h-28.5zM51.25 25.25l9.5-9.75V90h-9.5zM61.25 15.5l28.5 9.75V90h-28.5z'/></mask>";
            }
        }

        protected override string ElaborateSvg => $@"<g transform='translate({(IsCol ? RowCol : Reverse ? 9.075 : -1.075)}, {(IsCol ? (Reverse ? 9 : -.85) : RowCol + .15)})'>
            <g transform='translate(.05, .05) scale(.006)'>
                <path fill='#729FCF' stroke='#4165BA' d='M10 90V10L20 0l29 10v80'/>
                <path fill='none' stroke='#4165BA' d='M20 90V0'/>
                <path fill='none' stroke='#D5F6FF' d='M10 81l10-1 29 1m-39-9l10-2 29 2m-39-9l10-3 29 3m-39-9l10-4 29 4m-39-9l10-5 29 5m-39-9l10-6 29 6m-39-9l10-7 29 7m-39-9l10-8 29 8' mask='url(#skyscraper-3)'/>
                <path fill='none' stroke='#D5F6FF' stroke-width='.5' d='M11 90V9m10 81V.357M12.333 90V7.667M25.667 90V2.024M13.667 90V6.333M30.333 90V3.69M15 90V5m20 85V5.357M16.333 90V3.667M39.667 90V7.024M17.667 90V2.333M44.333 90V8.69M19 90V1' mask='url(#skyscraper-1)'/>
                <path fill='#208531' stroke='#325722' stroke-width='.75' d='M26.11 87.19c-2.27-.26-4.79-.44-7.48-.44-9.67 0-17.53 1.92-17.53 4.28 0 2.36 7.86 4.28 17.53 4.28 2.41 0 6.86-.31 6.86-.31 4.78 1.92 13.13 3.15 22.62 3.15 5.56 0 11.88-.59 16.16-1.34 4.46 1.41 7.78 2.11 13.82 2.09 11.49-.02 20.85-2.88 20.85-6.43 0-3.54-9.35-6.43-20.85-6.43-3.41 0-6.59.24-9.44.69-4.92-1.52-12.24-2.46-20.94-2.46-8.69 0-16.74 1.1-21.61 2.9z'/>
                <path fill='#3465A4' stroke='#1f3c60' d='M51 90V25l10-10 29 10v65'/>
                <path fill='none' stroke='#1f3c60' d='M61 90V15'/>
                <path fill='none' stroke='#D5F6FF' d='M51 82.688l10-1.021 29 1.02m-39-7.312l10-2.042 29 2.042m-39-7.313L61 65l29 3.063M51 60.75l10-4.083 29 4.083m-39-7.313l10-5.104 29 5.105m-39-7.313L61 40l29 6.125m-39-7.313l10-7.145 29 7.145M51 31.5l10-8.167L90 31.5' mask='url(#skyscraper-3)'/>
                <path fill='none' stroke='#D5F6FF' stroke-width='.5' d='M52 90V9m10 81V.357M53.333 90V7.667M66.667 90V2.024M54.667 90V6.333M71.333 90V3.69M56 90V5m20 85V5.357M57.333 90V3.667M80.667 90V7.024M58.667 90V2.333M85.333 90V8.69M60 90V1' mask='url(#skyscraper-1)'/>
                <path fill='#208531' stroke='#325722' stroke-width='.75' d='M26.11 87.19c-2.27-.26-4.79-.44-7.48-.44-9.67 0-17.53 1.92-17.53 4.28 0 2.36 7.86 4.28 17.53 4.28 2.41 0 6.86-.31 6.86-.31 4.78 1.92 13.13 3.15 22.62 3.15 5.56 0 11.88-.59 16.16-1.34 4.46 1.41 7.78 2.11 13.82 2.09 11.49-.02 20.85-2.88 20.85-6.43 0-3.54-9.35-6.43-20.85-6.43-3.41 0-6.59.24-9.44.69-4.92-1.52-12.24-2.46-20.94-2.46-8.69 0-16.74 1.1-21.61 2.9z'/>
            </g>
            <text x='.35' y='.55' font-size='.45' fill='black' stroke='white' stroke-width='.04' paint-order='stroke'>Σ</text>
            <text x='.8' y='.5' font-size='.3'>{Clue}</text>
        </g>";

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var constraints = new List<SvgConstraint>();
            foreach (var isCol in new[] { false, true })
                foreach (var reverse in new[] { false, true })
                    for (var rowCol = 0; rowCol < 9; rowCol++)
                        constraints.Add(new SkyscraperSum(isCol, rowCol, reverse, SkyscraperSumUniquenessConstraint.CalculateSkyscraperSumClue(Ut.NewArray(9, x => sudoku[isCol ? (rowCol + 9 * (reverse ? 8 - x : x)) : ((reverse ? 8 - x : x) + 9 * rowCol)]))));
            return constraints;
        }
    }
}