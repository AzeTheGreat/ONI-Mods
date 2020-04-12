using AzeLib.Extensions;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.LateUpdate))]
    class UpdateHitsBefore_Patch
    {
        const int updateCodesCount = 4;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();

            MethodInfo updateTarget = AccessTools.Method(typeof(InterfaceTool), "UpdateHoverElements");
            MethodInfo insertTarget = AccessTools.Method(typeof(InterfaceTool), "GetSelectablesUnderCursor");

            int updateIndex = codes.FindIndex(x => x.Is(OpCodes.Call, updateTarget)) - 3;
            int insertIndex = codes.FindIndex(x => x.Is(OpCodes.Call, insertTarget)) + 1;

            codes.InsertRange(insertIndex, codes.GetRange(updateIndex, updateCodesCount));
            codes.RemoveRange(updateIndex + updateCodesCount, updateCodesCount);

            return codes;
        }
    }
}
