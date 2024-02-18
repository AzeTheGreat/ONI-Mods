using HarmonyLib;

namespace InfiniteResearch
{
    class InfiniteResearchCenterButton : InfiniteModeToggleButton
    {
        [MyCmpGet] ResearchCenter researchCenter;

        protected override void UpdateState() => researchCenter.UpdateWorkingState(null);
    }

    [HarmonyPatch(typeof(ResearchCenter), nameof(ResearchCenter.OnPrefabInit))]
    class ResearchCenterConfig_Patch
    {
        static void Postfix(ResearchCenter __instance) => __instance.gameObject.AddComponent<InfiniteResearchCenterButton>();
    }
}
