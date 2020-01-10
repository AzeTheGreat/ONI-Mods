using Harmony;

namespace NoBuildWatermark
{
    [HarmonyPatch(typeof(BuildWatermark), "OnSpawn")]
    public class Watermark_Patch
    {
        static void Postfix(BuildWatermark __instance)
        {
            __instance.textDisplay.SetText(string.Empty);
        }
    }
}
