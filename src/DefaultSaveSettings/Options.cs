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
        [JsonProperty] public bool EnableProximity { get; set; }

        [Option("STRINGS.OPTIONS.AUTOSAVEINTERVAL.NAME", "STRINGS.OPTIONS.AUTOSAVEINTERVAL.TOOLTIP")]
        [Limit(0, 6)]
        [JsonProperty] public int AutoSaveInterval { get; set; }

        [Option("STRINGS.OPTIONS.TIMELAPSERESOLUTION.NAME", "STRINGS.OPTIONS.TIMELAPSERESOLUTION.TOOLTIP")]
        [Limit(0, 6)]
        [JsonProperty] public int TimelapseResolution { get; set; }

        [Option("STRINGS.OPTIONS.ENABLEAUTODISINFECT.NAME", "STRINGS.OPTIONS.ENABLEAUTODISINFECT.TOOLTIP")]
        [JsonProperty] public bool EnableAutoDisinfect { get; set; }

        [Option("STRINGS.OPTIONS.DISINFECTMINGERMCOUNT.NAME", "STRINGS.OPTIONS.DISINFECTMINGERMCOUNT.TOOLTIP")]
        [Limit(0, 1000000)]
        [JsonProperty] public int DisinfectMinGermCount { get; set; }

        public Options()
        {
            EnableProximity = true;
            AutoSaveInterval = 6;
            TimelapseResolution = 3;
            EnableAutoDisinfect = true;
            DisinfectMinGermCount = 10000;
        }
    }
}
