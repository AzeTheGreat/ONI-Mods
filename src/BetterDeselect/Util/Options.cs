using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;

namespace BetterDeselect
{
    [JsonObject(MemberSerialization.OptIn)]
    [PeterHan.PLib.Options.RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option("Separate Overlay", "Requires a separate right click to close the overlay.")]
        [JsonProperty]
        public bool ImplementOverlay { get; set; }

        [Option("Separate Build Menu", "Requires a separate right click to close the build menu.")]
        [JsonProperty]
        public bool ImplementBuildMenu { get; set; }

        [Option("Selection Mode", "When reselecting a held tool, or opening a category, choose what to do with the selected tool.")]
        [JsonProperty]
        public FixMode Mode { get; set; }

        public enum FixMode
        {
            [Option("Hold Selected", "Continue holding the same tool.")]
            Hold,
            [Option("Deselect Selected", "Deselect the tool.  Closest to vanilla behavior.")]
            Close
        }

        public Options()
        {
            ImplementOverlay = true;
            ImplementBuildMenu = false;
            Mode = FixMode.Hold;
        }
    }
}
