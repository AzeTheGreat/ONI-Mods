using HarmonyLib;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.CreateBuildingDef))]
    public class CarpetTile_Patch
    {
        static bool Prepare() => Options.Opts.CarpetTile.CombustTemp != null || Options.Opts.CarpetTile.CombustTemp != null;

        static void Postfix(BuildingDef __result)
        {
            // Overheatable won't work on tiles without setting .UseStructureTemperature to true (iffy), but does display in the UI
            if (Options.Opts.CarpetTile.CombustTemp is float temp)
            {
                __result.Overheatable = true;
                __result.OverheatTemperature = temp;
            }

            if(Options.Opts.CarpetTile.ReedFiberCount is int reeds)
                __result.Mass[1] = reeds;
        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.DoPostConfigureComplete))]
    public class CarpetTileCombust
    {
        static bool Prepare() => Options.Opts.CarpetTile.CombustTemp != null;

        static void Postfix(GameObject go)
        {
            go.GetComponent<KPrefabID>().prefabInitFn += gameObject => new Combustible.Instance(gameObject.GetComponent<KPrefabID>()).StartSM();
        }
    }

    [HarmonyPatch(typeof(OccupyArea), nameof(OccupyArea.GetExtents), typeof(Orientation))]
    public class Test
    {
        static bool Prepare() => Options.Opts.CarpetTile.IsNotWall;

        static void Postfix(OccupyArea __instance, ref Extents __result)
        {
            // Hacky manipulation to get the splat to be the right size/shape...
            if (__instance.name == "CarpetTileComplete")
            {
                int radius = Options.Opts.CarpetTile.DecorRadius ?? (int)__instance.gameObject.GetComponent<DecorProvider>().baseRadius;

                if (Grid.IsSolidCell(Grid.CellAbove(Grid.PosToCell(__instance.gameObject))))
                {
                    __result.x += radius;
                    __result.width = -(radius * 2 - 1);
                }

                __result.y += radius;
                __result.height = -(radius * 2 - 2);
            }
        }
    }
}

