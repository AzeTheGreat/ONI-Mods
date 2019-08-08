using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(StatusItemGroup), nameof(StatusItemGroup.AddStatusItem))]
    internal class Trans_AddStatusItem
    {
        // Transpiler to replace default ShouldShowIcon with a custom version.
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo targetMethod = AccessTools.Method(typeof(StatusItem), nameof(StatusItem.ShouldShowIcon));

            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Callvirt && i.operand == targetMethod)
                {
                    yield return new CodeInstruction(OpCodes.Pop);

                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(Trans_AddStatusItem), nameof(Trans_AddStatusItem.ShouldShowIcon)));
                    continue;
                }
                yield return i;
            }
        }

        private static bool ShouldShowIcon()
        {
            return false;
        }
    }
}
