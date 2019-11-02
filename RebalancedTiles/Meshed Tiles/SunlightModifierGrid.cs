﻿using Harmony;
using System;
using UnityEngine;

namespace RebalancedTiles.Mesh_Airflow_Tiles
{
    static class SunlightModifierGrid
    {
        public static byte[] sunlightModifiers = new byte[Grid.WidthInCells * Grid.HeightInCells];
        private static bool isInit = false;
        private static byte sunlightReductionFactor;

        public static void Initialize()
        {
            isInit = true;
            sunlightReductionFactor = Convert.ToByte(Options.Opts.MeshedTilesSunlightReduction * byte.MaxValue/100f);

            for (int i = 0; i < Grid.WidthInCells; i++)
            {
                CalculateSunMod(i, Grid.HeightInCells-1);
            }
        }

        public static void Update(GameObject go)
        {
            if (!isInit)
                return;

            if (go.name == "MeshTileComplete" || go.name == "GasPermeableMembraneComplete")
            {
                CalculateSunMod((int)go.transform.position.x, (int)go.transform.position.y);
            }
        }

        private static void CalculateSunMod(int column, int row)
        {
            byte currentMod = sunlightModifiers[Grid.XYToCell(column, row)];

            for (int y = row; y >= 0; y--)
            {
                int cell = Grid.XYToCell(column, y);
                GameObject go = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
                sunlightModifiers[cell] = currentMod;

                if(go?.name == "MeshTileComplete" || go?.name == "GasPermeableMembraneComplete")
                {
                    if (currentMod + sunlightReductionFactor > byte.MaxValue)
                        currentMod = byte.MaxValue;
                    else
                        currentMod += sunlightReductionFactor;
                }
            }
        }
    }

    [HarmonyPatch(typeof(CameraController), "OnSpawn")]
    class SunlightModifierGridInit_Patch
    {
        static bool Prepare()
        {
            return Options.Opts.DoMeshedTilesReduceSunlight;
        }

        static void Postfix()
        {
            SunlightModifierGrid.Initialize();
        }
    }

    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    class SpawnTracker_Patch
    {
        static bool Prepare()
        {
            return Options.Opts.DoMeshedTilesReduceSunlight;
        }

        static void Postfix(BuildingComplete __instance)
        {
            SunlightModifierGrid.Update(__instance.gameObject);
        }
    }

    [HarmonyPatch(typeof(Deconstructable), "OnCompleteWork")]
    class RemovalTracker_Patch
    {
        static bool Prepare()
        {
            return Options.Opts.DoMeshedTilesReduceSunlight;
        }

        static void Postfix(Deconstructable __instance)
        {
            SunlightModifierGrid.Update(__instance.gameObject);
        }
    }
}