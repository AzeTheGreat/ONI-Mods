using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;
using STRINGS;

namespace DefaultSaveSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option("STRINGS.OPTIONS.ENABLEPROXIMITY.NAME", "STRINGS.OPTIONS.ENABLEPROXIMITY.TOOLTIP")]
        [JsonProperty]
        public bool EnableProximityDefault { get; set; }

        [Option("STRINGS.OPTIONS.AUTOSAVEINTERVAL.NAME", "STRINGS.OPTIONS.AUTOSAVEINTERVAL.TOOLTIP")]
        [Limit(0, 6)]
        [JsonProperty]
        public int AutoSaveInterval { get; set; }

        [Option("STRINGS.OPTIONS.TIMELAPSERESOLUTION.NAME", "STRINGS.OPTIONS.TIMELAPSERESOLUTION.TOOLTIP")]
        [Limit(0, 6)]
        [JsonProperty]
        public int TimelapseResolution { get; set; }

        public Options()
        {
            EnableProximityDefault = true;
            AutoSaveInterval = 6;
            TimelapseResolution = 3;
        }
    }
}
