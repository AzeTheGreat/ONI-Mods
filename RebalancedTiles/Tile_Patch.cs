using Harmony;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(TileConfig), nameof(TileConfig.CreateBuildingDef))]
    public class TileCreate_Patch
    {
        static bool Prepare() => Options.Opts.Tile.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.Tile);
        }
    }

    [HarmonyPatch(typeof(TileConfig), nameof(TileConfig.ConfigureBuildingTemplate))]
    public class TileConfigure_Patch
    {
        static bool Prepare() => Options.Opts.Tile.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.Tile);
        }
    }
}
