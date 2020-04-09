using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;
using static DefaultSaveSettings.STRINGS.DEFAULTSAVESETTINGS.OPTIONS;

namespace DefaultSaveSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option(ENABLEPROXIMITY.nameKey, ENABLEPROXIMITY.tooltipKey)]
        [JsonProperty] public bool EnableProximity { get; set; }

        [Option(AUTOSAVEINTERVAL.nameKey, AUTOSAVEINTERVAL.tooltipKey)]
        [Limit(0, 6)]
        [JsonProperty] public int AutoSaveInterval { get; set; }

        [Option(TIMELAPSERESOLUTION.nameKey, TIMELAPSERESOLUTION.tooltipKey)]
        [Limit(0, 6)]
        [JsonProperty] public int TimelapseResolution { get; set; }

        [Option(GERMS.ENABLEAUTODISINFECT.nameKey, GERMS.ENABLEAUTODISINFECT.tooltipKey, GERMS.categoryKey)]
        [JsonProperty] public bool EnableAutoDisinfect { get; set; }

        [Option(GERMS.DISINFECTMINGERMCOUNT.nameKey, GERMS.DISINFECTMINGERMCOUNT.tooltipKey, GERMS.categoryKey)]
        [Limit(0, 1000000)]
        [JsonProperty] public int DisinfectMinGermCount { get; set; }

        public Options()
        {
            EnableProximity = true;
            AutoSaveInterval = 6;
            TimelapseResolution = 2;
            EnableAutoDisinfect = true;
            DisinfectMinGermCount = 10000;
        }
    }
}
