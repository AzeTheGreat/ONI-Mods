using AzeLib;
using PeterHan.PLib.Options;
using UnityEngine;

namespace InfiniteResearch
{
    public class Options : BaseOptions<Options>
    {
        [Option("STRINGS.BUILDINGS.PREFABS.RESEARCHCENTER.NAME", null, "STRINGS.BUILDINGS.PREFABS.RESEARCHCENTER.NAME")]
        public IRTuning ResearchCenter { get; set; } = new()
        {
            Min = 0,
            Max = 6
        };

        [Option("STRINGS.BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME", null, "STRINGS.BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME")]
        public IRTuning AdvancedResearchCenter { get; set; } = new()
        {
            Min = 7,
            Max = 13
        };

        [Option("STRINGS.BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME", null, "STRINGS.BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME")]
        public IRTuning CosmicResearchCenter { get; set; } = new()
        {
            Min = 14,
            Max = 19
        };

        public IRTuning GetIRTuningForGO(GameObject go)
        {
            return go.name switch
            {
                ResearchCenterConfig.ID + "Complete" => ResearchCenter,
                AdvancedResearchCenterConfig.ID + "Complete" => AdvancedResearchCenter,
                CosmicResearchCenterConfig.ID + "Complete" or DLC1CosmicResearchCenterConfig.ID + "Complete" => CosmicResearchCenter,
                _ => new(),
            };
        }

        public class IRTuning
        {
            [Option] public int Min { get; set; }
            [Option] public int Max { get; set; }
            [Option] public float ExpRate { get; set; } = TUNING.SKILLS.ALL_DAY_EXPERIENCE;
        }
    }
}