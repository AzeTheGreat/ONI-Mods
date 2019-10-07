using Harmony;
using UnityEngine;

namespace RebalancedTiles.Mesh_Airflow_Tiles
{
    static class SunlightModifierGrid
    {
        public static byte[] sunlightModifiers = new byte[Grid.WidthInCells * Grid.HeightInCells];
        private static bool isInit = false;

        public static void Initialize()
        {
            isInit = true;

            for (int i = 0; i < Grid.WidthInCells; i++)
            {
                CalculateSunMod(i);
            }
        }

        public static void Update(GameObject go)
        {
            if (!isInit)
                return;

            Debug.Log(go.name);
            if (go.name == "MeshTileComplete" || go.name == "GasPermeableMembraneComplete")
            {
                CalculateSunMod((int)go.transform.position.x);
            }
        }

        private static void CalculateSunMod(int column)
        {
            byte currentMod = 0;

            for (int y = Grid.HeightInCells-1; y >= 0; y--)
            {
                int cell = Grid.XYToCell(column, y);
                GameObject go = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
                sunlightModifiers[cell] = currentMod;

                if(go?.name == "MeshTileComplete" || go?.name == "GasPermeableMembraneComplete")
                {
                    if (currentMod + 64 > 255)
                        currentMod = 255;
                    else
                        currentMod += 64;
                }
            }
        }
    }

    [HarmonyPatch(typeof(CameraController), "OnSpawn")]
    class SunlightModifierGridInit_Patch
    {
        static void Postfix()
        {
            SunlightModifierGrid.Initialize();
        }
    }

    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    class SpawnTracker_Patch
    {
        static void Postfix(BuildingComplete __instance)
        {
            SunlightModifierGrid.Update(__instance.gameObject);
        }
    }

    [HarmonyPatch(typeof(Deconstructable), "OnCompleteWork")]
    class RemovalTracker_Patch
    {
        static void Postfix(Deconstructable __instance)
        {
            SunlightModifierGrid.Update(__instance.gameObject);
        }
    }
}
