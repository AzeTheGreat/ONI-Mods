using Harmony;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(BunkerTileConfig), nameof(BunkerTileConfig.CreateBuildingDef))]
    public class BunkerTile_patch
    {
        static bool Prepare()
        {
            return Options.Opts.IsBunkerTileTweaked;
        }

        static void Postfix(BuildingDef __result)
        {
            __result.BaseDecor = Options.Opts.BunkerTileDecor;
            __result.BaseDecorRadius = Options.Opts.BunkerTileDecorRadius;
        }
    }
}
