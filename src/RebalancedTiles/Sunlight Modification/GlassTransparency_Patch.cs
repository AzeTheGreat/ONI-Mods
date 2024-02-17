using HarmonyLib;
using System.Collections.Generic;

namespace RebalancedTiles.Sunlight_Modification
{
    [HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.CollectElementsFromYAML))]
    class GlassTransparency_Patch
    {
        static bool Prepare() => Options.Opts.GlassTile.GlassLightAbsorptionFactor != null || Options.Opts.GlassTile.DiamondLightAbsorptionFactor != null;

        static void Postfix(ref List<ElementLoader.ElementEntry> __result)
        {
            if(Options.Opts.GlassTile.GlassLightAbsorptionFactor is float glassFac)
                __result.Find(x => x.elementId == "Glass").lightAbsorptionFactor = glassFac;
            if(Options.Opts.GlassTile.DiamondLightAbsorptionFactor is float diamondFac)
            __result.Find(x => x.elementId == "Diamond").lightAbsorptionFactor = diamondFac;
        }
    }
}
