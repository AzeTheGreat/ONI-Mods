using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace CleanFloors
{
    [HarmonyPatch(typeof(BlockTileDecorInfo), nameof(BlockTileDecorInfo.PostProcess))]
    public class RemoveDecors
    {
        private static readonly List<string> decorsToRemove = new List<string> { "tops" };

        static void Postfix(ref BlockTileDecorInfo __instance)
        {
            for (int i = 0; i < __instance.decor.Count(); i++)
            {
                if (decorsToRemove.Contains(__instance.decor[i].name))
                    __instance.decor[i].probabilityCutoff = float.MaxValue;
            }
        }
    }
}
