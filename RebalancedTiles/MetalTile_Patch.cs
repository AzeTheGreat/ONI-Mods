using Harmony;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(MetalTileConfig), nameof(MetalTileConfig.CreateBuildingDef))]
    public class MetalTileCreate_Patch
    {
        static bool Prepare() => Options.Opts.MetalTile.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.MetalTile);
        }
    }

    [HarmonyPatch(typeof(MetalTileConfig), nameof(MetalTileConfig.ConfigureBuildingTemplate))]
    public class MetalTileConfigure_Patch
    {
        static bool Prepare() => Options.Opts.MetalTile.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.MetalTile);
        }
    }
}
