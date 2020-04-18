using Harmony;
using PeterHan.PLib;
using System.Collections.Generic;

namespace BetterDeselect
{
    [HarmonyPatch(typeof(PlanScreen), "OnActiveToolChanged")]
    class PreventCloseCatPanel_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(AccessTools.Method(typeof(PlanScreen), "CloseCategoryPanel"), 
                AccessTools.Method(typeof(PreventCloseCatPanel_Patch), nameof(PreventCloseCatPanel_Patch.CloseCategoryPanelWrapper)));
        }

        private static void CloseCategoryPanelWrapper(PlanScreen instance, bool playSound)
        {
            if(Options.Opts.BuildMenu == Options.ClickNum.One)
                Traverse.Create(instance).CallMethod("CloseCategoryPanel", playSound);
        }
    }

    [HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.DeactivateTool))]
    public class DeselectOverlay_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(AccessTools.Method(typeof(OverlayScreen), nameof(OverlayScreen.ToggleOverlay)), 
                AccessTools.Method(typeof(DeselectOverlay_Patch), nameof(DeselectOverlay_Patch.ToggleOverlayWrapper)));
        }

        private static void ToggleOverlayWrapper(OverlayScreen instance, HashedString newMode, bool allowSound)
        {
            if(Options.Opts.Overlay == Options.ClickNum.One)
                instance.ToggleOverlay(newMode, allowSound);
        }
    }
}