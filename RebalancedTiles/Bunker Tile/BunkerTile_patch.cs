using Harmony;
using TUNING;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(BunkerTileConfig), nameof(BunkerTileConfig.CreateBuildingDef))]
    public class BunkerTile_patch
    {
        static void Postfix(BuildingDef __result)
        {
            __result.BaseDecor = BUILDINGS.DECOR.PENALTY.TIER0.amount;
            __result.BaseDecorRadius = BUILDINGS.DECOR.PENALTY.TIER0.radius;
        }
    }
}
