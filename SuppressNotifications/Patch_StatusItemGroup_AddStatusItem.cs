using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(StatusItemGroup), nameof(StatusItemGroup.AddStatusItem))]
    internal class Patch_StatusItemGroup_AddStatusItem
    {
        // Transpiler to replace default ShouldShowIcon with a custom version
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo targetMethodIcon = AccessTools.Method(typeof(StatusItem), nameof(StatusItem.ShouldShowIcon));

            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Callvirt && i.operand == targetMethodIcon)
                {
                    // Load gameObject onto stack
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(StatusItemGroup), "get_gameObject"));

                    // Call custom ShouldShowIcon
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(Patch_StatusItemGroup_AddStatusItem), nameof(Patch_StatusItemGroup_AddStatusItem.ShouldShowIconSub)));
                    continue;
                }

                yield return i;
            }
        }

        private static bool ShouldShowIconSub(StatusItem statusItem, GameObject gameObject)
        {
            return gameObject.GetComponent<StatusItemsSuppressedComp>()?.ShouldShowIcon(statusItem) ?? statusItem.ShouldShowIcon();
        }
    }
}
