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
            [Option] One,
            [Option] Two,
            [Option] Three
        }

        public enum ReselectMode
        {
            [Option] Hold,
            [Option] Close
        }

        protected override bool ValidateSettings()
        {
            var settings = new List<ClickNum>() { SelectedObj, Overlay, BuildMenu };
            var map = settings.Distinct().OrderBy(x => x).ToList();
            var newSettings = settings.Select(x => map.IndexOf(x)).Cast<ClickNum>();

            if (!newSettings.SequenceEqual(settings))
            {
                SelectedObj = newSettings.ElementAt(0);
                Overlay = newSettings.ElementAt(1); 
                BuildMenu = newSettings.ElementAt(2);
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
