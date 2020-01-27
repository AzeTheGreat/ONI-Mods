using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace InfiniteResearch
{
    class InfiniteResearchCenterButton : InfiniteModeToggleButton
    {
        [MyCmpGet] ResearchCenter researchCenter;

        protected override void UpdateWorkingState()
        {
            Traverse.Create(researchCenter).Method("UpdateWorkingState", new object[] { null }).GetValue(new object[] { null });
        }
    }

    [HarmonyPatch]
    class ResearchCenterConfig_Patch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(ResearchCenterConfig), nameof(ResearchCenterConfig.ConfigureBuildingTemplate));
            yield return AccessTools.Method(typeof(AdvancedResearchCenterConfig), nameof(AdvancedResearchCenterConfig.ConfigureBuildingTemplate));
            yield return AccessTools.Method(typeof(CosmicResearchCenterConfig), nameof(CosmicResearchCenterConfig.ConfigureBuildingTemplate));
        }

        static void Postfix(GameObject go) => go.AddComponent<InfiniteResearchCenterButton>();
    }
}
