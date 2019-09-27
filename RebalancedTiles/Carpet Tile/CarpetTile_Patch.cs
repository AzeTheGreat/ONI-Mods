using Harmony;
using TUNING;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.CreateBuildingDef))]
    public class CarpetTile_Patch
    {
        static void Postfix(BuildingDef __result)
        {
            __result.BaseDecor = BUILDINGS.DECOR.BONUS.TIER2.amount;
            __result.BaseDecorRadius = BUILDINGS.DECOR.BONUS.TIER2.radius;
            __result.Overheatable = true;
            __result.OverheatTemperature = BUILDINGS.OVERHEAT_TEMPERATURES.LOW_2;

        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.ConfigureBuildingTemplate))]
    public class CarpetTileMovement
    {
        static void Postfix(GameObject go)
        {
            SimCellOccupier simCellOccupier = go.GetComponent<SimCellOccupier>();
            simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.PENALTY_1;
        }
    }
}
