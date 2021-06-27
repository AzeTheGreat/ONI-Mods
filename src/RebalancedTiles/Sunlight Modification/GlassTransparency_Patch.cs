using HarmonyLib;
using System.Collections.Generic;

namespace RebalancedTiles.Sunlight_Modification
{
    [HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.CollectElementsFromYAML))]
    class GlassTransparency_Patch
    {
        static bool Prepare() => Options.Opts.GlassTile.IsTweaked;

        static void Postfix(ref List<ElementLoader.ElementEntry> __result)
        {
            __result.Find(x => x.elementId == "Glass").lightAbsorptionFactor = Options.Opts.GlassTile.GlassLightAbsorptionFactor;
            __result.Find(x => x.elementId == "Diamond").lightAbsorptionFactor = Options.Opts.GlassTile.DiamondLightAbsorptionFactor;
        }
    }
}
