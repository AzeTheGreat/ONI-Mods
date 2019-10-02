using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace RebalancedTiles.Mesh_Tile
{
    [HarmonyPatch(typeof(MeshTileConfig), nameof(MeshTileConfig.CreateBuildingDef))]
    class MeshTileTransparency_Patch
    {
        static void Postifx(BuildingDef __result)
        {
            __result.BlockTileIsTransparent = true;
        }
    }

    [HarmonyPatch(typeof(MeshTileConfig), nameof(MeshTileConfig.ConfigureBuildingTemplate))]
    class MeshTileTransparency_Patch2
    {
        static void Postifx(GameObject go)
        {
            go.AddOrGet<SimCellOccupier>().setTransparent = true;
        }
    }
}
