using HarmonyLib;
using UnityEngine;

namespace CleanHUD
{
    [HarmonyPatch(typeof(Vignette), nameof(Vignette.Reset))]
    public class ClearDefaultVignette_Patch
    {
        static void Postfix(Vignette __instance)
        {
            if (Options.Opts.IsVignetteDisabled)
                __instance.SetColor(Color.clear);
        }
    }

    [HarmonyPatch(typeof(VignetteManager), nameof(VignetteManager.InitializeStates))]
    public class ResetAfterLoad_Patch
    {
        static void Prefix()
        {
            if (Options.Opts.IsVignetteDisabled)
                Vignette.Instance.Reset();
        }
    }
}