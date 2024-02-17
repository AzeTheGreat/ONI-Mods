using HarmonyLib;
using UnityEngine;

namespace RebalancedTiles
{
    [HarmonyPatch(typeof(TileConfig), nameof(TileConfig.CreateBuildingDef))]
    public class TileCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.Tile);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.Tile);
        }
    }

    [HarmonyPatch(typeof(TileConfig), nameof(TileConfig.ConfigureBuildingTemplate))]
    public class TileConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.Tile);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.Tile);
        }
    }

    [HarmonyPatch(typeof(BunkerTileConfig), nameof(BunkerTileConfig.CreateBuildingDef))]
    public class BunkerTileCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.BunkerTile);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.BunkerTile);
        }
    }

    [HarmonyPatch(typeof(BunkerTileConfig), nameof(BunkerTileConfig.ConfigureBuildingTemplate))]
    public class BunkerTileConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.BunkerTile);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.BunkerTile);
        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.CreateBuildingDef))]
    public class CarpetTileCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.CarpetTile);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.CarpetTile);
        }
    }

    [HarmonyPatch(typeof(CarpetTileConfig), nameof(CarpetTileConfig.ConfigureBuildingTemplate))]
    public class CarpetTileConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.CarpetTile);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.CarpetTile);
        }
    }

    [HarmonyPatch(typeof(GasPermeableMembraneConfig), nameof(GasPermeableMembraneConfig.CreateBuildingDef))]
    public class GasPermeableMembraneCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.GasPermeableMembrane);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.GasPermeableMembrane);
        }
    }

    [HarmonyPatch(typeof(GasPermeableMembraneConfig), nameof(GasPermeableMembraneConfig.ConfigureBuildingTemplate))]
    public class GasPermeableMembraneConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.GasPermeableMembrane);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.GasPermeableMembrane);
        }
    }

    [HarmonyPatch(typeof(GlassTileConfig), nameof(GlassTileConfig.CreateBuildingDef))]
    public class GlassTileCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.GlassTile);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.GlassTile);
        }
    }

    [HarmonyPatch(typeof(GlassTileConfig), nameof(GlassTileConfig.ConfigureBuildingTemplate))]
    public class GlassTileConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.GlassTile);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.GlassTile);
        }
    }

    [HarmonyPatch(typeof(InsulationTileConfig), nameof(InsulationTileConfig.CreateBuildingDef))]
    public class InsulationTileCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.InsulationTile);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.InsulationTile);
        }
    }

    [HarmonyPatch(typeof(InsulationTileConfig), nameof(InsulationTileConfig.ConfigureBuildingTemplate))]
    public class InsulationTileConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.InsulationTile);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.InsulationTile);
        }
    }

    [HarmonyPatch(typeof(MeshTileConfig), nameof(MeshTileConfig.CreateBuildingDef))]
    public class MeshTileCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.MeshTile);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.MeshTile);
        }
    }

    [HarmonyPatch(typeof(MeshTileConfig), nameof(MeshTileConfig.ConfigureBuildingTemplate))]
    public class MeshTileConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.MeshTile);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.MeshTile);
        }
    }

    [HarmonyPatch(typeof(MetalTileConfig), nameof(MetalTileConfig.CreateBuildingDef))]
    public class MetalTileCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.MetalTile);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.MetalTile);
        }
    }

    [HarmonyPatch(typeof(MetalTileConfig), nameof(MetalTileConfig.ConfigureBuildingTemplate))]
    public class MetalTileConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.MetalTile);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.MetalTile);
        }
    }

    [HarmonyPatch(typeof(PlasticTileConfig), nameof(PlasticTileConfig.CreateBuildingDef))]
    public class PlasticTileCreate_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchCreateBuildingDef(Options.Opts.PlasticTile);

        static void Postfix(BuildingDef __result)
        {
            GenericPatch.CreateBuildingDef(__result, Options.Opts.PlasticTile);
        }
    }

    [HarmonyPatch(typeof(PlasticTileConfig), nameof(PlasticTileConfig.ConfigureBuildingTemplate))]
    public class PlasticTileConfigure_Patch
    {
        static bool Prepare() => GenericPatch.ShouldPatchConfigureBuildingTemplate(Options.Opts.PlasticTile);
		
        static void Postfix(GameObject go)
        {
            GenericPatch.ConfigureBuildingTemplate(go, Options.Opts.PlasticTile);
        }
    }

}