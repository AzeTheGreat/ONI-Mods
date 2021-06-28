using HarmonyLib;
using UnityEngine;

namespace CleanHUD
{
    [HarmonyPatch(typeof(Vignette), nameof(Vignette.OnSpawn))]
    public class Clear
    {
        static void Postfix(Vignette __instance)
        {
            if (Options.Opts.IsVignetteDisabled)
                __instance.SetColor(__instance.defaultColor = Color.clear);
        }
    }
}