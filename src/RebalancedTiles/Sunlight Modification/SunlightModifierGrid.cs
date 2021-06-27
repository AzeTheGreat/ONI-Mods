using HarmonyLib;
using System;
using UnityEngine;

namespace RebalancedTiles.Mesh_Airflow_Tiles
{
    static class SunlightModifierGrid
    {
        public static byte[] sunlightModifiers = new byte[Grid.WidthInCells * Grid.HeightInCells];
        private static bool isInit = false;
        private static byte meshSunlightReduction;
        private static byte airflowSunlightReduction;

        private static void Initialize()
        {
            isInit = true;
            meshSunlightReduction = Convert.ToByte(Options.Opts.MeshTile.LightAbsorptionFactor * byte.MaxValue);
            airflowSunlightReduction = Convert.ToByte(Options.Opts.GasPermeableMembrane.LightAbsorptionFactor * byte.MaxValue);

            for (int i = 0; i < Grid.WidthInCells; i++)
                CalculateSunMod(i, Grid.HeightInCells - 1);
        }

        private static void Update(GameObject go)
        {
            if (!isInit || GetSunModForGO(go) == 0)
                return;

            Grid.CellToXY(Grid.PosToCell(go), out int x, out int y);
            CalculateSunMod(x, y);
        }

        private static byte GetSunModForGO(GameObject go)
        {
            if (go?.name == "MeshTileComplete")
                return meshSunlightReduction;
            if (go?.name == "GasPermeableMembraneComplete")
                return airflowSunlightReduction;
            return 0;
        }

        private static void CalculateSunMod(int column, int row)
        {
            byte currentMod = sunlightModifiers[Grid.XYToCell(column, row)];

            for (int y = row; y >= 0; y--)
            {
                int cell = Grid.XYToCell(column, y);

                // Early out if at max sunlight reduction and full column below is already maxed.
                if (currentMod == byte.MaxValue && sunlightModifiers[cell] == byte.MaxValue)
                    return;

                sunlightModifiers[cell] = currentMod;

                // The sunlight reduction can't increase any more if it's already maxed.
                if(currentMod < byte.MaxValue)
                {
                    GameObject go = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
                    currentMod = (byte)Math.Min(currentMod + GetSunModForGO(go), byte.MaxValue);
                }
            }
        }

        [HarmonyPatch(typeof(CameraController), "OnSpawn")]
        private class Init_Patch
        {
            static bool Prepare() => Options.Opts.DoMeshedTilesReduceSunlight;
            static void Postfix() => Initialize();
        }

        [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        private class SpawnTracker_Patch
        {
            static bool Prepare() => Options.Opts.DoMeshedTilesReduceSunlight;
            static void Postfix(BuildingComplete __instance) => Update(__instance.gameObject);
        }

        [HarmonyPatch(typeof(Deconstructable), "OnCompleteWork")]
        private class RemovalTracker_Patch
        {
            static bool Prepare() => Options.Opts.DoMeshedTilesReduceSunlight;
            static void Postfix(Deconstructable __instance) => Update(__instance.gameObject);
        }
    }
}
