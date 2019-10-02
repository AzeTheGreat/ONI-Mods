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
            __result.BaseDecor = TUNING.BUILDINGS.DECOR.BONUS.TIER2.amount;
            __result.BaseDecorRadius = TUNING.BUILDINGS.DECOR.BONUS.TIER2.radius;
            __result.Overheatable = true;
            __result.OverheatTemperature = TUNING.BUILDINGS.OVERHEAT_TEMPERATURES.LOW_2;
            __result.Mass[1] = 1;
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

    [HarmonyPatch(typeof(OccupyArea), nameof(OccupyArea.GetExtents), new[] { typeof(Orientation) })]
    public class Test
    {
        static void Postfix(OccupyArea __instance, ref Extents __result)
        {
            if (__instance.name == "CarpetTileComplete")
            {
                if (Grid.IsSolidCell(Grid.CellAbove(Grid.PosToCell(__instance.gameObject))))
                {
                    __result.x += 3;
                    __result.width = -5;
                }

                __result.y += 3;
                __result.height = -4;
            }
        }
    }   
}

