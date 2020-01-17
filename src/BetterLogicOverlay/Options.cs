using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;

namespace BetterLogicOverlay
{
    [JsonObject(MemberSerialization.OptIn)]
    [PeterHan.PLib.Options.RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option("Fix Wire Overwriting", "If true, green signals will not make a red output port display as green.")]
        [JsonProperty]
        public bool FixWireOverwrite { get; set; }

        [Option("Display Logic Settings", "If true, logic gate and sensor settings will be displayed in the automation overlay.")]
        [JsonProperty]
        public bool DisplayLogicSettings { get; set; }

        [Option("Font Size", "Sets the font size for the automation overlay.  Default of 9")]
        [JsonProperty]
        [Limit(3, 9)]
        public float FontSize { get; set; }

        public enum FixMode
        {
            [Option("Hold Selected", "Continue holding the same tool. Reqiores Restart.")]
            Hold,
            [Option("Deselect Selected", "Deselect the tool.  Closest to vanilla behavior. Requires Restart.")]
            Close
        }

        public Options()
        {
            FixWireOverwrite = true;
            DisplayLogicSettings = true;
            FontSize = 9;
        }

        public static void OnLoad()
        {
            Load();
        }
    }
}
