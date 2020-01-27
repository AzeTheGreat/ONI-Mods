using Harmony;
using UnityEngine;

namespace InfiniteResearch
{
    class InfiniteTelescopeButton : InfiniteModeToggleButton
    {
        [MyCmpGet] Telescope telescope;

        protected override void UpdateState() => Traverse.Create(telescope).Method("UpdateWorkingState", new object[] { null }).GetValue(new object[] { null });
    }

    [HarmonyPatch(typeof(TelescopeConfig), nameof(TelescopeConfig.ConfigureBuildingTemplate))]
    class TelescopeConfig_Patch { static void Postfix(GameObject go) => go.AddComponent<InfiniteTelescopeButton>(); }
}
