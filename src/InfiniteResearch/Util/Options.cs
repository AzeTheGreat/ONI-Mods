using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Database;
using PeterHan.PLib.Options;

namespace InfiniteResearch
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        private const string min = "Minimum";
        private const string max = "Maximum";
        private const string minDesc = "The lowest level at which a dupe can operate this station";
        private const string maxDesc = "The highest level at which a dupe can operate this station";
        private const string exp = "Experience Rate";
        private const string expDesc = "How quickly duplicants gain experience while working at this station.  Vanilla defaults to 1.11.";

        [Option(min, minDesc, "Research Center")]
        [JsonProperty] public int ResearchCenterMin { get; set; }
        [Option(max, maxDesc, "Research Center")]
        [JsonProperty] public int ResearchCenterMax { get; set; }
        [Option(exp, expDesc, "Research Center")]
        [JsonProperty] public float ResearchCenterExpRate { get; set; }

        [Option(min, minDesc, "Super Computer")]
        [JsonProperty] public int AdvancedResearchCenterMin { get; set; }
        [Option(max, maxDesc, "Super Computer")]
        [JsonProperty] public int AdvancedResearchCenterMax { get; set; }
        [Option(exp, expDesc, "Super Computer")]
        [JsonProperty] public float AdvancedResearchCenterExpRate { get; set; }

        [Option(min, minDesc, "Telescope")]
        [JsonProperty] public int TelescopeMin { get; set; }
        [Option(max, maxDesc, "Telescope")]
        [JsonProperty] public int TelescopeMax { get; set; }
        [Option(exp, expDesc, "Telescope")]
        [JsonProperty] public float TelescopeExpRate { get; set; }

        [Option(min, minDesc, "Planetarium")]
        [JsonProperty] public int CosmicResearchCenterMin { get; set; }
        [Option(max, maxDesc, "Planetarium")]
        [JsonProperty] public int CosmicResearchCenterMax { get; set; }
        [Option(exp, expDesc, "Planetarium")]
        [JsonProperty] public float CosmicResearchCenterExpRate { get; set; }

        public Options()
        {
            ResearchCenterMin = 0;
            ResearchCenterMax = 1;
            ResearchCenterExpRate = TUNING.SKILLS.ALL_DAY_EXPERIENCE;
            AdvancedResearchCenterMin = 2;
            AdvancedResearchCenterMax = 7;
            AdvancedResearchCenterExpRate = TUNING.SKILLS.ALL_DAY_EXPERIENCE;
            TelescopeMin = 8;
            TelescopeMax = 13;
            TelescopeExpRate = TUNING.SKILLS.ALL_DAY_EXPERIENCE;
            CosmicResearchCenterMin = 14;
            CosmicResearchCenterMax = 19;
            CosmicResearchCenterExpRate = TUNING.SKILLS.ALL_DAY_EXPERIENCE;
        }

        public static void OnLoad()
        {
            var loc = new PLocalization();
            loc.Register();
        }
    }
}
