using Harmony;

namespace RebalancedTiles.Mesh_Airflow_Tiles
{
    [HarmonyPatch(typeof(Grid.LightIntensityIndexer), "get_Item")]
    class SunlightStrength_Patch
    {
        static void Postfix(int i, ref int __result)
        {
            __result -= (int)((float)SunlightModifierGrid.sunlightModifiers[i] / 255f * Game.Instance.currentSunlightIntensity);
            if (__result < 0)
                __result = 0;
        }
    }
}
