using HarmonyLib;

namespace CleanHUD
{
    [HarmonyPatch(typeof(Vignette), nameof(Vignette.OnSpawn))]
    public class ModifyVignette
    {
        static void Prefix(Vignette __instance)
        {
            // DefaultColor is set to the image color in OnSpawn, so set the image color just before.
            var newColor = __instance.image.color;
            newColor.a = Options.Opts.VignetteAlpha;
            __instance.image.color = newColor;

            __instance.redAlertColor.a = __instance.yellowAlertColor.a = Options.Opts.AlertVignetteAlpha;
        }
    }
}