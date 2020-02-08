using Harmony;

namespace DefaultBuildingSettings
{
    [HarmonyPatch(typeof(TreeFilterable), "OnPrefabInit")]
    class SweepOnly_Patches
    {
        static bool Prepare() => Options.Opts.SweepOnly;
        static void Postfix(Storage ___storage)
        {
            ___storage.SetOnlyFetchMarkedItems(true);
        }
    }
}
