using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace CleanFloors
{
    [HarmonyPatch(typeof(BlockTileDecorInfo), nameof(BlockTileDecorInfo.PostProcess))]
    public class RemoveDecors
    {
        private static readonly List<string> decorsToRemove = new List<string> { "tops" };

        static void Postfix(BlockTileDecorInfo __instance)
        {
            __instance.decor = __instance.decor.Select(decor =>
                {
                    if (decorsToRemove.Contains(decor.name))
                        decor.probabilityCutoff = float.MaxValue;

                    return decor;
                })
                .ToArray();
        }
    }
}
