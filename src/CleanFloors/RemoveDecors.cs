using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace CleanFloors
{
    [HarmonyPatch(typeof(Assets), nameof(Assets.OnPrefabInit))]
    public class RemoveDecors
    {
        private static readonly List<string> decorsToRemove = new List<string> { "tops" };

        static void Postfix()
        {
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
