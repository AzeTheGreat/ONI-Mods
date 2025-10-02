using HarmonyLib;

namespace DefaultBuildingSettings
{
    [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    class LoadGeneratedBuildings_Patch
    {
        // The prefab adjustments now run through the shared attribute helper inside AzeLib.
        // Keeping an empty patch preserves compatibility with save states that expect the
        // type to exist without duplicating the actual work.
        static void Postfix() { }
    }
}
