using HarmonyLib;
using System.Collections.Generic;

namespace BetterDeselect;

// If the active tool is changed, it normally closes the category panel.
// This prevents the category panel from being closed if the build menu is set to close after the selected object.
[HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnActiveToolChanged))]
class PreventCloseCategoryPanel
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
    {
        return codes.MethodReplacer(
            AccessTools.Method(typeof(PlanScreen), nameof(PlanScreen.CloseCategoryPanel)),
            AccessTools.Method(typeof(PreventCloseCategoryPanel), nameof(Splice)));
    }

    private static void Splice(PlanScreen instance, bool playSound)
    {
        if (Options.Opts.BuildMenu <= Options.Opts.SelectedObj)
            instance.CloseCategoryPanel(playSound);
    }
}

// When the current tool is deselected, it normally closes the overlay screen.
// This prevents the overlay screen from being closed if the overlay screen is set to close after the selected object.
[HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.DeactivateTool))]
public class PreventCloseOverlay
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions.MethodReplacer(
            AccessTools.Method(typeof(OverlayScreen), nameof(OverlayScreen.ToggleOverlay)),
            AccessTools.Method(typeof(PreventCloseOverlay), nameof(Splice)));
    }

    private static void Splice(OverlayScreen instance, HashedString newMode, bool allowSound)
    {
        if (Options.Opts.Overlay <= Options.Opts.SelectedObj)
            instance.ToggleOverlay(newMode, allowSound);
    }
}