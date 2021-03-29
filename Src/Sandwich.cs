using System;
using System.Collections.Generic;
using System.Linq;
using PuzzleSolvers;
using RT.Util;

namespace SvgPuzzleConstraints
{
    [SvgConstraintInfo("Sandwich")]
    public class Sandwich : SvgRowColNumberConstraint
    {
        public override string Description => $"Within this {(IsCol ? "column" : "row")}, the digits sandwiched between the {Digit1} and the {Digit2} must add up to {Clue}. The {Digit1} and {Digit2} can occur in either order.";
        public override double ExtraTop => IsCol ? 0.9 : 0;
        public override double ExtraLeft => IsCol ? 0 : 1.5;
        public override bool ShownTopLeft => true;
        public static readonly Example Example = new Example
        {
            Constraints = { new Sandwich(false, 0, 3, 7, 17) },
            Cells = { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
            Good = { 4, 1, 5, 3, 9, 8, 7, 6, 2 },
            Bad = { 4, 1, 5, 3, 9, 8, 6, 7, 2 },
            Reason = "The digits between the 3 and 7 are 9+8+6 = 23.",
            Wide = true
        };

        public Sandwich(bool isCol, int rowCol, int digit1, int digit2, int sum, RowColDisplay display = RowColDisplay.Default, bool omitCrust = false)
            : base(isCol, rowCol, sum, display)
        {
            Digit1 = digit1;
            Digit2 = digit2;
            OmitCrust = omitCrust;
        }
        private Sandwich() { }    // for Classify

        public int Digit1 { get; private set; }
        public int Digit2 { get; private set; }
        public bool OmitCrust { get; private set; }

        protected override IEnumerable<Constraint> getConstraints() { yield return new SandwichUniquenessConstraint(Digit1, Digit2, Clue, Ut.NewArray(9, x => IsCol ? (RowCol + 9 * x) : (x + 9 * RowCol))); }

        public override bool Verify(int[] grid)
        {
            var numbers = Ut.NewArray(9, x => grid[IsCol ? (RowCol + 9 * x) : (x + 9 * RowCol)]);
            var p1 = Array.IndexOf(numbers, Digit1);
            var p2 = Array.IndexOf(numbers, Digit2);
            if (p1 == -1 || p2 == -1)
                return false;
            var pLeft = Math.Min(p1, p2);
            var pRight = Math.Max(p1, p2);
            var s = 0;
            for (var i = pLeft + 1; i < pRight; i++)
                s += numbers[i];
            return s == Clue;
        }

        public override IEnumerable<string> SvgDefs
        {
            get
            {
                yield return @"
                  <linearGradient id='sandwich-1' x1='17.181' x2='45.393' y1='71.738' y2='73.757' gradientUnits='userSpaceOnUse'>
                    <stop offset='0' stop-color='#ff3d2a'/>
                    <stop offset='.041' stop-color='#f63424'/>
                    <stop offset='.174' stop-color='#dd1d14'/>
                    <stop offset='.314' stop-color='#cc0d09'/>
                    <stop offset='.464' stop-color='#c10302'/>
                    <stop offset='.64' stop-color='#be0000'/>
                    <stop offset='.99' stop-color='#ff1500'/>
                  </linearGradient>";
                yield return @"
                  <linearGradient id='sandwich-2' x1='5.446' x2='33.749' y1='156.275' y2='158.301' gradientTransform='rotate(-9.633 -419.545 -82.064)' gradientUnits='userSpaceOnUse'>
                    <stop offset='0' stop-color='#ff3d2a'/>
                    <stop offset='.041' stop-color='#f63424'/>
                    <stop offset='.174' stop-color='#dd1d14'/>
                    <stop offset='.314' stop-color='#cc0d09'/>
                    <stop offset='.464' stop-color='#c10302'/>
                    <stop offset='.64' stop-color='#be0000'/>
                    <stop offset='.99' stop-color='#ff1500'/>
                  </linearGradient>";
                yield return @"
                  <linearGradient id='sandwich-3' x1='64.886' x2='93.097' y1='78.777' y2='80.796' gradientUnits='userSpaceOnUse'>
                    <stop offset='0' stop-color='#ff3d2a'/>
                    <stop offset='.041' stop-color='#f63424'/>
                    <stop offset='.174' stop-color='#dd1d14'/>
                    <stop offset='.314' stop-color='#cc0d09'/>
                    <stop offset='.464' stop-color='#c10302'/>
                    <stop offset='.64' stop-color='#be0000'/>
                    <stop offset='.99' stop-color='#ff1500'/>
                  </linearGradient>";
                yield return @"
                  <linearGradient id='sandwich-4' x1='76.479' x2='104.69' y1='68.922' y2='70.941' gradientUnits='userSpaceOnUse'>
                    <stop offset='0' stop-color='#ff3d2a'/>
                    <stop offset='.041' stop-color='#f63424'/>
                    <stop offset='.174' stop-color='#dd1d14'/>
                    <stop offset='.314' stop-color='#cc0d09'/>
                    <stop offset='.464' stop-color='#c10302'/>
                    <stop offset='.64' stop-color='#be0000'/>
                    <stop offset='.99' stop-color='#ff1500'/>
                  </linearGradient>";
                yield return @"
                  <linearGradient id='sandwich-5' x1='93.584' x2='121.796' y1='54.139' y2='56.158' gradientUnits='userSpaceOnUse'>
                    <stop offset='0' stop-color='#ff3d2a'/>
                    <stop offset='.041' stop-color='#f63424'/>
                    <stop offset='.174' stop-color='#dd1d14'/>
                    <stop offset='.314' stop-color='#cc0d09'/>
                    <stop offset='.464' stop-color='#c10302'/>
                    <stop offset='.64' stop-color='#be0000'/>
                    <stop offset='.99' stop-color='#ff1500'/>
                  </linearGradient>";
                yield return @"
                  <linearGradient id='sandwich-6' x1='21.124' x2='124.084' y1='39.244' y2='39.244' gradientTransform='rotate(2.56 -380.023 -142.253)' gradientUnits='userSpaceOnUse'>
                    <stop offset='.005' stop-color='#fbc02d'/>
                    <stop offset='.081' stop-color='#fcca30'/>
                    <stop offset='.209' stop-color='#fdd534'/>
                    <stop offset='.36' stop-color='#fdd835'/>
                    <stop offset='.69' stop-color='#fff59d'/>
                    <stop offset='1' stop-color='#fdd835'/>
                  </linearGradient>";
                yield return @"
                  <linearGradient id='sandwich-7' x1='9.732' x2='74.285' y1='67.527' y2='67.527' gradientUnits='userSpaceOnUse'>
                    <stop offset='0' stop-color='#ffc044'/>
                    <stop offset='.847' stop-color='#d95f23'/>
                  </linearGradient>";
                yield return @"
                  <linearGradient id='sandwich-8' x1='10.014' x2='117.791' y1='44.887' y2='44.887' gradientUnits='userSpaceOnUse'>
                    <stop offset='0' stop-color='#fff2d9'/>
                    <stop offset='.847' stop-color='#f7d398'/>
                  </linearGradient>";
                yield return @"
                  <linearGradient id='sandwich-9' x1='72.836' x2='118.397' y1='59.201' y2='59.201' gradientUnits='userSpaceOnUse'>
                    <stop offset='0' stop-color='#ffb219'/>
                    <stop offset='.847' stop-color='#ca4300'/>
                  </linearGradient>";
            }
        }

        private static readonly string _sandwichIcon = $@"
<path fill='#df8826' stroke-width='.045' d='M.082.354C.058.33.028.324-.005.314-.1.286-.186.266-.278.228-.304.22-.329.217-.355.212a.159.159 0 01-.08-.054.063.063 0 00-.003.019c.001.044 0 .077.01.093.012.015.336.11.42.138.06.02.073.034.098.031C.087.407.104.376.082.354z'/>
<path fill='#f6d9a4' stroke-width='.045' d='M.451.043C.455.006.446.004.401-.006.356-.016.067-.103.024-.115c-.044-.012-.07-.021-.097-.012a.353.353 0 00-.06.03c-.07.052-.263.208-.263.208s-.031.015-.04.047a.159.159 0 00.08.054c.027.005.053.008.078.017.092.038.179.058.273.084.031.011.064.02.087.042.01-.01.026-.008.035-.017C.424.094.451.045.451.043z'/>
<path fill='#d76208' stroke-width='.045' d='M.114.329C.104.338.081.352.082.354c.003.03.003.088.006.087.027-.008.09-.06.14-.097C.28.307.45.144.455.13.46.117.456.096.456.083A1.087 1.087 0 00.452.019C.454.041.44.055.428.067.329.159.229.236.114.329z'/>
<path fill='#ea8a77' stroke-width='.045' d='M-.435.105c-.012.013-.01.024.001.037.022.025.081.056.168.062.087.006.164-.023.231-.16.067-.138-.088-.22-.088-.22s-.3.268-.312.281z'/>
<path fill='#a72e18' stroke-width='.045' d='M-.423.112c-.013.012-.002.044.146.074 0 0-.052-.045-.087-.066C-.398.099-.41.1-.423.112z'/>
<path fill='#eaa88e' stroke-width='.045' d='M-.292.146c-.012.013-.01.023.001.037.022.025.081.056.168.062C-.036.25.042.222.11.085.175-.055.02-.137.02-.137S-.28.133-.292.146z'/>
<path fill='#a72e18' stroke-width='.045' d='M-.28.153c-.012.012-.002.043.146.073 0 0-.052-.044-.087-.066C-.255.14-.267.141-.279.153z'/>
<path fill='#ea8a77' stroke-width='.045' d='M-.122.196c-.013.014-.011.026.002.04.023.029.09.063.187.07C.163.311.25.28.324.125.4-.026.226-.118.226-.118s-.335.3-.348.315z'/>
<path fill='#a72e18' stroke-width='.045' d='M-.108.203C-.122.217-.11.251.054.285c0 0-.058-.05-.096-.073C-.081.188-.094.19-.108.203z'/>
<path fill='#9abc00' stroke-width='.045' d='M.479-.082C.49-.085.5-.101.499-.116.497-.11.487-.112.484-.12.479-.127.48-.137.48-.145c0-.009-.004-.019-.011-.02C.463-.166.458-.16.452-.159.432-.153.419-.196.4-.189c-.006.001-.01.006-.014.01-.02.02-.05.022-.075.014C.286-.172.263-.188.24-.2a.29.29 0 00-.04-.016c.042.084.08.171.16.201.032.011.075.018.117.017a.04.04 0 000-.011C.467-.014.452-.026.458-.039.461-.046.47-.047.476-.043a.05.05 0 01.013.02c.01-.02.005-.048-.01-.06zm-.846-.03a1.167 1.167 0 00-.06.05c-.02.019-.025.016-.047.006a.043.043 0 00-.006.013l.009.008-.016.018c-.002-.004-.006-.007-.01-.007L-.5.02A.022.022 0 00-.48.018a.039.039 0 00.01.035.035.035 0 00.01-.025c.008 0 .015.004.02.011l-.016.023c.005.01.014.014.022.01.004.013.01.024.02.032L-.416.083l.02-.006c0 .01 0 .02.004.028s.013.012.02.006a.032.032 0 00.01.03c.01-.12.052-.226.098-.33-.035.025-.07.05-.103.078z'/>
<path fill='#c6d500' stroke-width='.045' d='M-.061-.227C-.095-.24-.13-.252-.163-.247a.204.204 0 00-.082.043l-.019.014a1.044 1.044 0 00-.099.33.03.03 0 00.008.005c.01.006.023.004.035.001.004-.01 0-.023 0-.034 0-.012.009-.026.017-.02-.006.022.013.04.03.052.005.004.012.009.018.007l.01-.005c.012-.002.017.022.03.028.012.007.025-.019.014-.03.015-.01.03.013.038.032.005.014.014.03.024.034.077-.11.079-.284.078-.437z'/>
<path fill='#9abc00' stroke-width='.045' d='M.235.215A.024.024 0 01.22.194.248.248 0 01.203.167a3.783 3.783 0 00-.21-.382.223.223 0 01-.054-.013C-.06-.074-.061.1-.14.21A.013.013 0 00-.125.208C-.128.198-.117.191-.11.193-.101.195-.094.2-.085.2c.012 0 .022-.019.017-.033.01-.01.024-.001.032.01.008.012.014.027.026.032C0 .214.01.209.02.206.03.203.043.204.048.216.052.222.05.232.055.238c.005.007.013.006.02.008.016.003.029.018.043.027.014.01.034.014.045 0-.01-.01-.01-.032 0-.04.01-.01.027-.004.032.01.001.005.002.01.004.013C.204.263.213.26.22.255A.077.077 0 00.24.22L.235.215z'/>
<path fill='#9abc00' stroke-width='.045' d='M.235.215A.304.304 0 00.24.221L.242.216a.021.021 0 01-.007 0zM.083-.226c-.03.004-.058.013-.088.012h-.002c.074.121.145.243.21.382l.016.025C.218.18.23.168.24.175A.097.097 0 01.23.132c0-.015.012-.03.024-.026.012.003.019.02.03.021.007 0 .012-.005.017-.01C.292 0 .18-.117.114-.23l-.03.003z'/>
<path fill='#7a8e00' stroke-width='.045' d='M.36-.015C.28-.045.24-.132.2-.216A.264.264 0 00.113-.23C.18-.116.292 0 .302.116.31.11.315.1.324.1.33.1.335.103.34.101.35.1.35.084.358.08.368.073.383.087.391.077.398.067.388.053.379.047.375.03.392.016.406.02c.013.003.025.014.04.014C.46.035.472.02.475.002A.368.368 0 01.36-.015z'/>
<g stroke-width='2.331' transform='matrix(.00824 0 0 .00824 -.53 -.568)'>
    <ellipse cx='30.6' cy='72.7' fill='url(#sandwich-1)' rx='15.73' ry='8.68'/>
    <ellipse cx='31.02' cy='70.64' fill='#e44000' rx='15.3' ry='6.63'/>
    <ellipse cx='31.83' cy='69.76' fill='#891301' rx='14.49' ry='5.75'/>
</g>
<g stroke-width='2.331' transform='matrix(.00824 0 0 .00824 -.53 -.568)'>
    <path fill='url(#sandwich-2)' d='M68.24 77.31c.65 4.73-5.77 10.01-14.33 11.81-8.56 1.8-16.03-.59-16.68-5.31C36.58 79.08 43 73.79 51.56 72c8.56-1.79 16.03.58 16.68 5.31z'/>
    <path fill='#e44000' d='M67.96 75.29c.5 3.61-5.86 7.95-14.19 9.69-8.33 1.74-15.49.23-15.98-3.38-.5-3.61 5.86-7.95 14.19-9.69 8.33-1.74 15.49-.23 15.98 3.38z'/>
    <path fill='#891301' d='M67.85 74.42c.43 3.13-5.62 7.01-13.51 8.66-7.89 1.65-14.64.45-15.07-2.68-.43-3.13 5.62-7.01 13.51-8.66 7.89-1.65 14.64-.45 15.07 2.68z'/>
</g>
<g stroke-width='2.331' transform='matrix(.00824 0 0 .00824 -.53 -.568)'>
    <ellipse cx='78.3' cy='79.74' fill='url(#sandwich-3)' rx='15.73' ry='8.68'/>
    <ellipse cx='78.73' cy='77.68' fill='#e44000' rx='15.3' ry='6.63'/>
    <ellipse cx='78.39' cy='76.8' fill='#891301' rx='13.54' ry='5.51'/>
</g>
<g stroke-width='2.331' transform='matrix(.00824 0 0 .00824 -.53 -.568)'>
    <ellipse cx='89.89' cy='69.88' fill='url(#sandwich-4)' rx='15.73' ry='8.68'/>
    <ellipse cx='90.32' cy='67.83' fill='#e44000' rx='15.3' ry='6.63'/>
    <ellipse cx='89.99' cy='66.95' fill='#891301' rx='13.54' ry='5.51'/>
</g>
<g stroke-width='2.331' transform='matrix(.00824 0 0 .00824 -.53 -.568)'>
    <ellipse cx='107' cy='55.1' fill='url(#sandwich-5)' rx='15.73' ry='8.68'/>
    <ellipse cx='107.43' cy='53.05' fill='#e44000' rx='15.3' ry='6.63'/>
    <ellipse cx='107.09' cy='52.17' fill='#891301' rx='13.54' ry='5.51'/>
</g>
<path fill='url(#sandwich-6)' stroke-width='.019' d='M-.416-.07a.037.037 0 00-.012.018c-.004.013.004.026.014.032.01.006.02.007.03.01.05.018.075.092.124.112.034.013.071-.004.107-.01a.18.18 0 01.136.033c.033.024.067.06.104.05C.112.168.129.142.149.122A.167.167 0 01.263.068c.013 0 .026 0 .039-.004C.322.057.337.035.346.012c.01-.024.015-.05.024-.073a.245.245 0 01.05-.085c.002-.002.005-.017.005-.02.001-.006-.003 0-.007-.003C.393-.195.358-.201.327-.21.242-.232.162-.278.079-.307c-.084-.03-.176-.04-.255.005a.133.133 0 00-.045.05c-.016.024-.039.04-.058.058l-.137.125z'/>
<g stroke-width='2.331' transform='matrix(.00824 0 0 .00824 -.53 -.568)'>
    <path fill='url(#sandwich-7)' d='M72.84 74.22c-2.85-2.82-6.56-3.52-10.55-4.93-11.4-3.17-21.95-5.63-33.07-10.21-3.14-1.06-6.27-1.41-9.41-2.11-3.74-1.09-7.13-3.22-9.8-6.56-.2.71-.31 1.49-.28 2.34.19 5.4 0 9.39 1.33 11.26C12.4 65.89 51.74 77.39 62 80.79c7.15 2.37 8.83 4.1 11.9 3.81-.4-3.95 1.69-7.71-1.06-10.38z'/>
    <path fill='url(#sandwich-8)' d='M117.69 36.45c.48-4.46-.57-4.69-6.08-5.87-5.51-1.17-40.48-11.85-45.8-13.26-5.32-1.41-8.36-2.58-11.78-1.41-3.42 1.17-7.22 3.52-7.22 3.52-8.55 6.34-31.93 25.34-31.93 25.34s-3.78 1.83-4.85 5.64c2.67 3.34 6.06 5.47 9.8 6.56 3.14.7 6.27 1.06 9.41 2.11 11.12 4.58 21.67 7.04 33.07 10.21 3.83 1.35 7.84 2.49 10.64 5.04 1.14-1.21 3.04-.93 4.25-2.05 37.19-29.56 40.46-35.6 40.49-35.83z'/>
    <path fill='url(#sandwich-9)' d='M76.83 71.17c-1.21 1.12-4.01 2.87-3.99 3.05.36 3.7.45 10.66.78 10.56 3.23-.94 11.02-7.33 17.11-11.79 6.08-4.46 26.8-24.31 27.37-25.96.55-1.59.2-4.05.14-5.71-.09-2.56-.2-5.15-.44-7.7.24 2.63-1.48 4.38-2.95 5.76-11.95 11.17-24.09 20.55-38.02 31.79z'/>
</g>";

        protected override string ElaborateSvg =>
            OmitCrust ? $@"
                <g transform='translate({(IsCol ? RowCol + .5 : -1.15)}, {(IsCol ? -.95 : RowCol + .5)}) scale(.7)'>{_sandwichIcon}</g>
                <text x='{(IsCol ? RowCol + .5 : -.4)}' y='{(IsCol ? -.15 : RowCol + .675)}' text-anchor='middle' font-size='.5'>{Clue}</text>"
            : $@"
                <g transform='translate({(IsCol ? RowCol + .5 : -.95)}, {(IsCol ? -.8 : RowCol + .5)}) scale(.6)'>{_sandwichIcon}</g>
                <text x='{(IsCol ? RowCol + .5 : -.325)}' y='{(IsCol ? -.15 : RowCol + .625)}' text-anchor='middle' font-size='.4'>{Clue}</text>
                <text x='{(IsCol ? RowCol + .25 : -1.4)}' y='{(IsCol ? -1.05 : RowCol + .45)}' text-anchor='middle' font-size='.25'>{Digit1}</text>
                <text x='{(IsCol ? RowCol + .75 : -1.4)}' y='{(IsCol ? -1.05 : RowCol + .7)}' text-anchor='middle' font-size='.25'>{Digit2}</text>";

        public static IList<SvgConstraint> Generate(int[] sudoku)
        {
            var constraints = new List<SvgConstraint>();
            foreach (var isCol in new[] { false, true })
                for (var rowCol = 0; rowCol < 9; rowCol++)
                    for (var digit1 = 1; digit1 <= 9; digit1++)
                        for (var digit2 = digit1 + 1; digit2 <= 9; digit2++)
                        {
                            var digits = Enumerable.Range(0, 9).Select(x => sudoku[isCol ? (rowCol + 9 * x) : (x + 9 * rowCol)]).ToArray();
                            var p1 = Array.IndexOf(digits, digit1);
                            var p2 = Array.IndexOf(digits, digit2);
                            var pLeft = Math.Min(p1, p2);
                            var pRight = Math.Max(p1, p2);
                            var sandwichSum = 0;
                            for (var i = pLeft + 1; i < pRight; i++)
                                sandwichSum += digits[i];
                            constraints.Add(new Sandwich(isCol, rowCol, digit1, digit2, sandwichSum));
                        }
            return constraints;
        }
    }
}
