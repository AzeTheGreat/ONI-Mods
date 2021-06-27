using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(ToolMenu),nameof(ToolMenu.OnKeyDown))]
    class EscapeCloseToolMenu_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.Manipulator(AccessTools.Method(typeof(SelectTool), nameof(SelectTool.Activate)), AddMethod);

            IEnumerable<CodeInstruction> AddMethod(CodeInstruction i)
            {
                yield return i;
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EscapeCloseToolMenu_Patch), nameof(EscapeCloseToolMenu_Patch.CloseOverlayAndMenu)));
            }
        }

        private static void CloseOverlayAndMenu()
        {
            OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
            if(PlanScreen.Instance.activeCategoryInfo != null)
                PlanScreen.Instance.OnClickCategory(PlanScreen.Instance.activeCategoryInfo);
        }
    }

    [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnKeyDown))]
    class EscapeClosePlanScreen_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.Manipulator(AccessTools.Method(typeof(SelectTool), nameof(SelectTool.Activate)), AddMethod);

            IEnumerable<CodeInstruction> AddMethod(CodeInstruction i)
            {
                yield return i;
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EscapeClosePlanScreen_Patch), nameof(EscapeClosePlanScreen_Patch.CloseOverlay)));
            }
        }

        private static void CloseOverlay() => OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
    }
}
