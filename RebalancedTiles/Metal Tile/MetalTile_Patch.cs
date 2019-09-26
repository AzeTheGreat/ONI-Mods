using Harmony;
using TUNING;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(MetalTileConfig), nameof(MetalTileConfig.CreateBuildingDef))]
    public class MetalTile_Patch
    {
        static void Postfix(BuildingDef __result)
        {
            __result.BaseDecor = BUILDINGS.DECOR.BONUS.TIER1.amount;
            __result.BaseDecorRadius = BUILDINGS.DECOR.BONUS.TIER0.radius;
        }
    }
}
