using Harmony;
using TUNING;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(GlassTileConfig), nameof(GlassTileConfig.CreateBuildingDef))]
    public class WindowTileCreate_Patch
    {
        static bool Prepare() => Options.Opts.WindowTile.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.WindowTile);
        }
    }

    [HarmonyPatch(typeof(GlassTileConfig), nameof(GlassTileConfig.ConfigureBuildingTemplate))]
    public class WindowTileConfigure_Patch
    {
        static bool Prepare() => Options.Opts.WindowTile.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.WindowTile);
        }
    }
}
