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
}
