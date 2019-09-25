using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnSelectBuilding))]
    public class ReselectFix_patch
    {
        //private static InterfaceTool lastTool;

        //static void Prefix()
        //{
        //    lastTool = PlayerController.Instance.ActiveTool;
        //}

        //static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    MethodInfo targetMethodInfo = AccessTools.Method(typeof(PlanScreen), "CloseRecipe");

        //    foreach (CodeInstruction i in instructions)
        //    {
        //        yield return i;

        //        if (i.opcode == OpCodes.Call && i.operand == targetMethodInfo)
        //            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ReselectFix_patch), "Helper"));
        //    }
        //}

        //public static void Helper()
        //{
        //    PlayerController.Instance.ActivateTool(SelectTool.Instance);

        //    if (Options.options.Mode == Options.FixMode.Hold)
        //        PlayerController.Instance.ActivateTool(lastTool);
        //}
    }
}
