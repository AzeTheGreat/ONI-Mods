using Harmony;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.CreateBuildingDef))]
    public class CarpetTile_Patch
    {
        static bool Prepare() => Options.Opts.CarpetTile.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.CarpetTile);

            // Overheatable won't work on tiles without setting .UseStructureTemperature to true (iffy), but does display in the UI
            if (Options.Opts.CarpetTile.IsCombustible)
            {
                __result.Overheatable = true;
                __result.OverheatTemperature = Options.Opts.CarpetTile.CombustTemp;
            }

            __result.Mass[1] = Options.Opts.CarpetTile.ReedFiberCount;
        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.ConfigureBuildingTemplate))]
    public class CarpetTileMovement
    {
        static bool Prepare() => Options.Opts.CarpetTile.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.CarpetTile);
        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.DoPostConfigureComplete))]
    public class CarpetTileCombust
    {
        static bool Prepare() => Options.Opts.CarpetTile.IsTweaked && Options.Opts.CarpetTile.IsCombustible;

        static void Postfix(GameObject go)
        {
            go.GetComponent<KPrefabID>().prefabInitFn += gameObject => new Combustible.Instance(gameObject.GetComponent<KPrefabID>()).StartSM();
        }
    }

    [HarmonyPatch(typeof(OccupyArea), nameof(OccupyArea.GetExtents), new[] { typeof(Orientation) })]
    public class Test
    {
        static bool Prepare() => Options.Opts.CarpetTile.IsTweaked && Options.Opts.CarpetTile.IsNotWall;

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

