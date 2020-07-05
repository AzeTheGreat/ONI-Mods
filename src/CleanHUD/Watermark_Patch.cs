using Harmony;

namespace CleanHUD
{
    [HarmonyPatch(typeof(BuildWatermark), nameof(BuildWatermark.RefreshText))]
    public class Watermark_Patch
    {
        static void Postfix(BuildWatermark __instance)
        {
            // Hide only if in game so that it still shows on the main menu.
            if (Options.Opts.IsWatermarkDisabled && Game.Instance)
                __instance.textDisplay.SetText(string.Empty);
        }
    }
}
