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

        [Option("Separate Build Menu", "Requires a separate right click to close the build menu. Requires Restart.")]
        [JsonProperty]
        public bool ImplementBuildMenu { get; set; }

        [Option("Selection Mode", "When reselecting a held tool, or opening a category, choose what to do with the selected tool.")]
        [JsonProperty]
        public FixMode Mode { get; set; }

        public enum FixMode
        {
            [Option("Hold Selected", "Continue holding the same tool. Reqiores Restart.")]
            Hold,
            [Option("Deselect Selected", "Deselect the tool.  Closest to vanilla behavior. Requires Restart.")]
            Close
        }

        public Options()
        {
            ImplementOverlay = true;
            ImplementBuildMenu = true;
            Mode = FixMode.Hold;
        }

        public static void OnLoad()
        {
            Load();
        }
    }
}
