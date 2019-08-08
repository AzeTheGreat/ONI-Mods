using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(StatusItemGroup), nameof(StatusItemGroup.AddStatusItem))]
    internal class Trans_AddStatusItem
    {
        // Transpiler to replace default ShouldShowIcon with a custom version
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo targetMethod = AccessTools.Method(typeof(StatusItem), nameof(StatusItem.ShouldShowIcon));

            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Callvirt && i.operand == targetMethod)
                {
                    // Load gameObject onto stack
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(StatusItemGroup), "get_gameObject"));

                    // Call custom ShouldShowIcon
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(Trans_AddStatusItem), nameof(Trans_AddStatusItem.ShouldShowIcon)));
                    continue;
                }
                yield return i;
            }
        }

        private static bool ShouldShowIcon(StatusItem item, GameObject go)
        {
            bool isShown = go.GetComponent<StatusItemsSuppressed>()?.IsShown(item) ?? true;
            return item.ShouldShowIcon() && isShown;
        }
    }
}
