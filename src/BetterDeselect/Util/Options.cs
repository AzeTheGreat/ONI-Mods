using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace BetterDeselect
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        private const string tooltip = "What order to deslect items. The Cursor is deslected on First.";

        [Option("Deselect Overlay", tooltip)]
        [JsonProperty] public ClickNum Overlay { get; set; }

        [Option("Deselect Build Menu", tooltip)]
        [JsonProperty] public ClickNum BuildMenu { get; set; }

        public enum ClickNum
        {
            [Option("First")] One,
            [Option("Second")] Two,
            [Option("Third")] Three
        }

        [Option("Selection Mode", "When reselecting a held tool, or opening a category, choose what to do with the selected tool.")]
        [JsonProperty]
        public ReselectMode Reselect { get; set; }

        public enum ReselectMode
        {
            [Option("Hold Selected", "Continue holding the same tool.")] Hold,
            [Option("Deselect Selected", "Deselect the tool. Closest to vanilla behavior.")] Close
        }

        public override bool ValidateSettings()
        {
            if (!(Overlay == ClickNum.Three && BuildMenu == ClickNum.Three))
                return true;

            Overlay = BuildMenu = ClickNum.Two;
            return false;
        }

        public Options()
        {
            Overlay = ClickNum.Two;
            BuildMenu = ClickNum.Two;
            Reselect = ReselectMode.Hold;
        }
    }
}
