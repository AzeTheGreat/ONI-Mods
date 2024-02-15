using AzeLib;
using PeterHan.PLib.Options;

namespace BetterDeselect
{
    public class Options : BaseOptions<Options>
    {
        [Option] public ClickNum SelectedObj { get; set; }
        [Option] public ClickNum Overlay { get; set; }
        [Option] public ClickNum BuildMenu { get; set; }
        [Option] public ReselectMode Reselect { get; set; }

        public enum ClickNum
        {
            [Option("First")] One,
            [Option("Second")] Two,
            [Option("Third")] Three
        }

        public enum ReselectMode
        {
            [Option("Hold Selected", "Continue holding the same tool.")] Hold,
            [Option("Deselect Selected", "Deselect the tool. Closest to vanilla behavior.")] Close
        }

        protected override bool ValidateSettings()
        {
            if (!(Overlay == ClickNum.Three && BuildMenu == ClickNum.Three))
                return true;

            Overlay = BuildMenu = ClickNum.Two;
            return false;
        }

        public Options()
        {
            SelectedObj = ClickNum.One;
            Overlay = ClickNum.Two;
            BuildMenu = ClickNum.Two;
            Reselect = ReselectMode.Hold;
        }
    }
}
