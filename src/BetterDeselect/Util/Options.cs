using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace BetterDeselect
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option]
        [JsonProperty] public ClickNum Overlay { get; set; }

        [Option]
        [JsonProperty] public ClickNum BuildMenu { get; set; }

        public enum ClickNum
        {
            [Option("First")] One,
            [Option("Second")] Two,
            [Option("Third")] Three
        }

        [Option]
        [JsonProperty]
        public ReselectMode Reselect { get; set; }

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
            Overlay = ClickNum.Two;
            BuildMenu = ClickNum.Two;
            Reselect = ReselectMode.Hold;
        }
    }
}
