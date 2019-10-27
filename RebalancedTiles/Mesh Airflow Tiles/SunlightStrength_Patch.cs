using Harmony;
using System;

namespace RebalancedTiles.Mesh_Airflow_Tiles
{
    [HarmonyPatch(typeof(Grid.LightIntensityIndexer), "get_Item")]
    class SunlightStrength_Patch
    {
        static unsafe bool Prefix(int i, ref int __result)
        {
            int adjustedExposure = Math.Max(0, Grid.exposedToSunlight[i] - SunlightModifierGrid.sunlightModifiers[i]);
            int sunlight = (int)(adjustedExposure / 255f * Game.Instance.currentSunlightIntensity);

            int light = Grid.LightCount[i];

            __result = sunlight + light;
            return false;
        }
    }
}
