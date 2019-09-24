using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BetterDeselect
{
    [HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.DeactivateTool))]
    public class Patch_InterfaceTool_DeactivateTool
    {
        static void Prefix(ref HashedString ___toolActivatedViewMode)
        {
            if(Options.options.ImplementOverlay)
                ___toolActivatedViewMode = OverlayModes.None.ID;
        }
    }

    [HarmonyPatch(typeof(PlanScreen), "OnActiveToolChanged")]
    public class Patch_PlanScreen_OnActiveToolChanged
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo targetMethodInfo = AccessTools.Method(typeof(PlanScreen), "CloseCategoryPanel");

            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Call && i.operand == targetMethodInfo)
                {
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Nop);
                }
                else
                    yield return i;
            }
        }
    }

    [HarmonyPatch(typeof(PlanScreen), "OnClickCategory")]
    public class Patch_PlanScreen_OnClickCategory
    {
        static void Postfix()
        {
            if(Options.options.ImplementReselectFix)
                PlayerController.Instance.ActivateTool(SelectTool.Instance);
        }
    }

    [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnSelectBuilding))]
    public class Patch_PlanScreen_OnSelectBuilding
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo targetMethodInfo = AccessTools.Method(typeof(PlanScreen), "CloseRecipe");

            foreach (CodeInstruction i in instructions)
            {
                yield return i;

                if (i.opcode == OpCodes.Call && i.operand == targetMethodInfo)
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_PlanScreen_OnSelectBuilding), "Helper"));
            }
        }

        public static void Helper()
        {
            PlayerController.Instance.ActivateTool(SelectTool.Instance);
        }
    }
}
