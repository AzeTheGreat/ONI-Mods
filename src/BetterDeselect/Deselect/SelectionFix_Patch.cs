using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BetterDeselect.Deselect
{
    static class SelectionFix
    {
        [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnSelectBuilding))]
        private class ReselectionFix_Patch
        {
            static bool Prepre() => Options.Opts.Mode == Options.FixMode.Hold;

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return ProcessCodeInstructions(instructions);
            }
        }

        [HarmonyPatch(typeof(PlanScreen), "OnClickCategory")]
        private class CategoryFix_Patch
        {
            static bool Prepre() => Options.Opts.Mode == Options.FixMode.Hold;

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return ProcessCodeInstructions(instructions);
            }
        }

        private static IEnumerable<CodeInstruction> ProcessCodeInstructions(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo targetMethodInfo = AccessTools.Method(typeof(PlanScreen), "CloseRecipe");

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
}
