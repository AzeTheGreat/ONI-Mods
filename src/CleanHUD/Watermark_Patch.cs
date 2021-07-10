using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace CleanHUD
{
    [HarmonyPatch]
    public class Watermark_Patch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(BuildWatermark), nameof(BuildWatermark.RefreshText));
            yield return AccessTools.Method(typeof(BuildWatermark), nameof(BuildWatermark.OnPrefabInit));
        }

        static void Postfix(BuildWatermark __instance)
        {
            // Hide only if in game so that it still shows on the main menu.
            if (Options.Opts.IsWatermarkDisabled && Game.Instance)
                __instance.Show(false);
        }
    }
}
