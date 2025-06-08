using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanFloors
{
    [HarmonyPatch(typeof(Assets), nameof(Assets.OnPrefabInit))]
    public class RemoveDecors
    {
        static bool removeOutsideCorners = true;
        static bool removeInsideCorners = true;

        static void Postfix()
        {
            List<string> decorsToRemove = [
                "tops",
                ..(removeOutsideCorners ? [ "outside_bl_corner", "outside_br_corner", "outside_tl_corner", "outside_tr_corner" ] : Array.Empty<string>()),
                ..(removeInsideCorners ? [ "inside_bl_corner", "inside_br_corner", "inside_tl_corner", "inside_tr_corner" ] : Array.Empty<string>())
            ];

            foreach (var decorInfo in Assets.BlockTileDecorInfos)
            {
                decorInfo.decor = decorInfo.decor.Select(decor =>
                {
                    if (decorsToRemove.Contains(decor.name))
                        decor.probabilityCutoff = float.MaxValue;

                    return decor;
                })
                .ToArray();
            }
        }
    }
}
