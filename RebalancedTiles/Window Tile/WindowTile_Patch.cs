using Harmony;
using TUNING;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(GlassTileConfig), nameof(GlassTileConfig.ConfigureBuildingTemplate))]
    public class WindowTileMovement
    {
        static void Postfix(GameObject go)
        {
            SimCellOccupier simCellOccupier = go.GetComponent<SimCellOccupier>();
            simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.PENALTY_2;
        }
    }
}
