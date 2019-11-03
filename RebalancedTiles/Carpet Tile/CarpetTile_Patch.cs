using Harmony;
using TUNING;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.CreateBuildingDef))]
    public class CarpetTile_Patch
    {
        static bool Prepare() { return Options.Opts.IsCarpetTileTweaked; }

        static void Postfix(BuildingDef __result)
        {
            __result.BaseDecor = Options.Opts.CarpetTileDecor;
            __result.BaseDecorRadius = Options.Opts.CarpetTileDecorRadius;

            // Overheatable won't work on tiles without setting .UseStructureTemperature to true (iffy), but does display in the UI
            if (Options.Opts.CarpetTileIsCombustible)
            {
                __result.Overheatable = true;
                __result.OverheatTemperature = Options.Opts.CarpetTileCombustTemp;
            }

            __result.Mass[1] = Options.Opts.CarpetTileReedFiberCount;
        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.ConfigureBuildingTemplate))]
    public class CarpetTileMovement
    {
        static bool Prepare() { return Options.Opts.IsCarpetTileTweaked; }

        static void Postfix(GameObject go)
        {
            SimCellOccupier simCellOccupier = go.GetComponent<SimCellOccupier>();
            simCellOccupier.movementSpeedMultiplier = Options.Opts.CarpetTileMovementSpeed;
        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.DoPostConfigureComplete))]
    public class CarpetTileCombust
    {
        static bool Prepare() { return Options.Opts.IsCarpetTileTweaked && Options.Opts.CarpetTileIsCombustible; }

        static void Postfix(GameObject go)
        {
            go.GetComponent<KPrefabID>().prefabInitFn += gameObject => new Combustible.Instance(gameObject.GetComponent<KPrefabID>()).StartSM();
        }
    }

    [HarmonyPatch(typeof(OccupyArea), nameof(OccupyArea.GetExtents), new[] { typeof(Orientation) })]
    public class Test
    {
        static bool Prepare() { return Options.Opts.IsCarpetTileTweaked && Options.Opts.IsCarpetNotWall; }

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

