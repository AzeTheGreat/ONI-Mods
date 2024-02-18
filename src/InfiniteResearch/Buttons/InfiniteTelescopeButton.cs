using HarmonyLib;

namespace InfiniteResearch
{
    class InfiniteTelescopeButton : InfiniteModeToggleButton
    {
        [MyCmpGet] Telescope telescope;

        protected override void UpdateState() => telescope.UpdateWorkingState(null);
    }

    [HarmonyPatch(typeof(Telescope), nameof(Telescope.OnPrefabInit))]
    class TelescopeConfig_Patch { static void Postfix(Telescope __instance) => __instance.gameObject.AddComponent<InfiniteTelescopeButton>(); }
}
