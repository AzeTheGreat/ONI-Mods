using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BetterInfoCards.Hits
{
    [HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.LateUpdate))]
    class UpdateHitsBefore_Patch
    {
        const int updateCodesCount = 4;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            MethodInfo updateTarget = AccessTools.Method(typeof(InterfaceTool), "UpdateHoverElements");
            MethodInfo insertTarget = AccessTools.Method(typeof(InterfaceTool), "GetSelectablesUnderCursor");

            int updateIndex = codes.FindIndex(x => x.opcode == OpCodes.Call && x.operand == updateTarget) - 3;
            int insertIndex = codes.FindIndex(x => x.opcode == OpCodes.Call && x.operand == insertTarget) + 1;

            codes.InsertRange(insertIndex, codes.GetRange(updateIndex, updateCodesCount));
            codes.RemoveRange(updateIndex + updateCodesCount, updateCodesCount);

            return codes;
        }
    }
}
