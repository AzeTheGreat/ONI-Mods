using AzeLib.Extensions;
using Harmony;
using PeterHan.PLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(ToolMenu),nameof(ToolMenu.OnKeyDown))]
    class EscapeCloseTooolMenu_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.Manipulator(AccessTools.Method(typeof(SelectTool), nameof(SelectTool.Activate)), AddMethod);

            IEnumerable<CodeInstruction> AddMethod(CodeInstruction i)
            {
                yield return i;
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EscapeCloseTooolMenu_Patch), nameof(EscapeCloseTooolMenu_Patch.CloseOverlayAndMenu)));
            }
        }

        private static void CloseOverlayAndMenu()
        {
            OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
            var planScreen = Traverse.Create(PlanScreen.Instance);
            planScreen.CallMethod("OnClickCategory", planScreen.GetField<KIconToggleMenu.ToggleInfo>("activeCategoryInfo"));
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
