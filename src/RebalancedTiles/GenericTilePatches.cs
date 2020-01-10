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

    [HarmonyPatch(typeof(BunkerTileConfig), nameof(BunkerTileConfig.CreateBuildingDef))]
    public class BunkerTileCreate_Patch
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

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.CreateBuildingDef))]
    public class CarpetTileCreate_Patch
    {
        static bool Prepare() => Options.Opts.CarpetTile.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.CarpetTile);
        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.ConfigureBuildingTemplate))]
    public class CarpetTileConfigure_Patch
    {
        static bool Prepare() => Options.Opts.CarpetTile.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.CarpetTile);
        }
    }

    [HarmonyPatch(typeof(GasPermeableMembraneConfig), nameof(GasPermeableMembraneConfig.CreateBuildingDef))]
    public class GasPermeableMembraneCreate_Patch
    {
        static bool Prepare() => Options.Opts.GasPermeableMembrane.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.GasPermeableMembrane);
        }
    }

    [HarmonyPatch(typeof(GasPermeableMembraneConfig), nameof(GasPermeableMembraneConfig.ConfigureBuildingTemplate))]
    public class GasPermeableMembraneConfigure_Patch
    {
        static bool Prepare() => Options.Opts.GasPermeableMembrane.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.GasPermeableMembrane);
        }
    }

    [HarmonyPatch(typeof(GlassTileConfig), nameof(GlassTileConfig.CreateBuildingDef))]
    public class GlassTileCreate_Patch
    {
        static bool Prepare() => Options.Opts.GlassTile.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.GlassTile);
        }
    }

    [HarmonyPatch(typeof(GlassTileConfig), nameof(GlassTileConfig.ConfigureBuildingTemplate))]
    public class GlassTileConfigure_Patch
    {
        static bool Prepare() => Options.Opts.GlassTile.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.GlassTile);
        }
    }

    [HarmonyPatch(typeof(MeshTileConfig), nameof(MeshTileConfig.CreateBuildingDef))]
    public class MeshTileCreate_Patch
    {
        static bool Prepare() => Options.Opts.MeshTile.IsTweaked;

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.MeshTile);
        }
    }

    [HarmonyPatch(typeof(MeshTileConfig), nameof(MeshTileConfig.ConfigureBuildingTemplate))]
    public class MeshTileConfigure_Patch
    {
        static bool Prepare() => Options.Opts.MeshTile.IsTweaked;

        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.MeshTile);
        }
    }

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