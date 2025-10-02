using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleanFloors;

[HarmonyPatch(typeof(Assets), nameof(Assets.OnPrefabInit))]
public class ModifyDecors
{
    static void Postfix()
    {
        var decorsToRemove = new HashSet<string>(StringComparer.Ordinal)
        {
            "tops"
        };

        if (Options.Opts.RemoveOutsideCorners)
        {
            decorsToRemove.Add("outside_bl_corner");
            decorsToRemove.Add("outside_br_corner");
            decorsToRemove.Add("outside_tl_corner");
            decorsToRemove.Add("outside_tr_corner");
        }

        if (Options.Opts.RemoveInsideCorners)
        {
            decorsToRemove.Add("inside_bl_corner");
            decorsToRemove.Add("inside_br_corner");
            decorsToRemove.Add("inside_tl_corner");
            decorsToRemove.Add("inside_tr_corner");
        }

        var cornersToSort = Options.Opts.CornersBelowTops
            ? new HashSet<string>(StringComparer.Ordinal)
            {
                "inside_bl_corner",
                "inside_br_corner"
            }
            : null;

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
            if (cornersToSort is null)
                return;

            if (cornersToSort.Contains(decor.name))
                decor.sortOrder = -2;
        }
    }
}