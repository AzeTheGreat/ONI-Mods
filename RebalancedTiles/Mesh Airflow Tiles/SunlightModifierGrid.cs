using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace RebalancedTiles.Mesh_Airflow_Tiles
{
    static class SunlightModifierGrid
    {
        public static byte[] sunlightModifiers;

        public static void Initialize()
        {
            sunlightModifiers = new byte[Grid.WidthInCells * Grid.HeightInCells];

            for (int i = 0; i < Grid.WidthInCells; i++)
            {
                CalculateUpdateRange(i);
                CalculateSunMod(i);
            }
        }

        public static bool IsInUpdateRange(Transform transform)
        {
            return true;
        }

        public static void Update(GameObject go)
        {
            if (go.HasTag(GameTags.FloorTiles))
            {
                CalculateSunMod((int)go.transform.position.x);
            }
        }

        private static void CalculateUpdateRange(int column)
        {

        }

        private static void CalculateSunMod(int column)
        {
            byte currentMod = 0;

            for (int y = Grid.HeightInCells-1; y >= 0; y--)
            {
                int cell = Grid.XYToCell(column, y);
                GameObject go = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
                sunlightModifiers[cell] = currentMod;

                if(go?.name == "MeshTileComplete")
                {
                    if (currentMod + 64 > 255)
                        currentMod = 255;
                    else
                        currentMod += 64;
                    
                }
            }
        }
    }

    [HarmonyPatch(typeof(Game), "OnSpawn")]
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
