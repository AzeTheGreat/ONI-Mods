using AzeLib;
using PeterHan.PLib.Options;
using System.Collections.Generic;
using System.Linq;

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
            [Option("1")] One,
            [Option("2")] Two,
            [Option("3")] Three
        }

        public enum ReselectMode
        {
            [Option("Hold", "Keep the current tool selected.")] Hold,
            [Option("Clear", "Deselect the current tool. Closest to vanilla behavior.")] Close
        }

        protected override bool ValidateSettings()
        {
            var settings = new List<ClickNum>() { SelectedObj, Overlay, BuildMenu };
            
            var shift = settings.Min();
            if(shift > 0)
            {
                SelectedObj -= shift;
                Overlay -= shift;
                BuildMenu -= shift;
                return false;
            }
            
            return true;
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
