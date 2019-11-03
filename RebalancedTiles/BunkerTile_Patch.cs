using Harmony;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(BunkerTileConfig), nameof(BunkerTileConfig.CreateBuildingDef))]
    public class BunkerTileCreate_patch
    {
        static bool Prepare() => Options.Opts.BunkerTile.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.BunkerTile);
        }
    }

    [HarmonyPatch(typeof(BunkerTileConfig), nameof(BunkerTileConfig.ConfigureBuildingTemplate))]
    public class BunkerTileConfigure_Patch
    {
        static bool Prepare() => Options.Opts.BunkerTile.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.BunkerTile);
        }
    }
}
