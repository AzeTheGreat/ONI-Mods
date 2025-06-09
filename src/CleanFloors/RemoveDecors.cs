using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleanFloors;

[HarmonyPatch(typeof(Assets), nameof(Assets.OnPrefabInit))]
public class RemoveDecors
{
    static void Postfix()
    {
        foreach (var decorInfo in Assets.BlockTileDecorInfos)
        {
            decorInfo.decor = decorInfo.decor.Select(decor =>
            {
                RemoveDecorIfHidden(ref decor);
                FixInsideBLCornerSort(ref decor);
                SortCornersBelowTops(ref decor);
                return decor;
            })
            .ToArray();
        }

        void RemoveDecorIfHidden(ref BlockTileDecorInfo.Decor decor)
        {
            List<string> decorsToRemove = [
                "tops",
                ..(Options.Opts.RemoveOutsideCorners ? [ "outside_bl_corner", "outside_br_corner", "outside_tl_corner", "outside_tr_corner" ] : Array.Empty<string>()),
                ..(Options.Opts.RemoveInsideCorners ? [ "inside_bl_corner", "inside_br_corner", "inside_tl_corner", "inside_tr_corner" ] : Array.Empty<string>())
            ];

            if (decorsToRemove.Contains(decor.name))
                decor.probabilityCutoff = float.MaxValue;
        }

        // The base game places the Inside BL decor on the top right corner of a tile.  This causes inconsistent sorting between it and the Tops decor.
        // Moving the Inside BL decor to the top left corner (and thus to the same tile with the Tops decor), fixes this.
        void FixInsideBLCornerSort(ref BlockTileDecorInfo.Decor decor)
        {
            if (decor.name == "inside_bl_corner")
            {
                decor.requiredConnections = Rendering.BlockTileRenderer.Bits.Left | Rendering.BlockTileRenderer.Bits.UpLeft;
                decor.forbiddenConnections = Rendering.BlockTileRenderer.Bits.Up;

                decor.variants = decor.variants.Select(variant =>
                {
                    variant.offset += new Vector3(-1, 0);
                    return variant;
                })
                .ToArray();
            }
        }

        void SortCornersBelowTops(ref BlockTileDecorInfo.Decor decor)
        {
            if (!Options.Opts.CornersBelowTops)
                return;

            List<string> decorsToSort = ["inside_bl_corner", "inside_br_corner"];
            if (decorsToSort.Contains(decor.name))
                decor.sortOrder = -2;
        }
    }
}