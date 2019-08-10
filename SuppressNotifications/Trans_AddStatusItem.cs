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
    internal class Trans_StatusItemGroup
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
                        AccessTools.Method(typeof(Trans_StatusItemGroup), nameof(Trans_StatusItemGroup.ShouldShowIconSub)));
                    continue;
                }

                yield return i;
            }
        }

        private static bool ShouldShowIconSub(StatusItem statusItem, GameObject gameObject)
        {
            // Seems like the game attempts to add Status items before BuildingConfigManager is run?
            // Thus the null check.  Might result in some status items not showing as the game initializes, unsure if it could be an issue.
            return gameObject.GetComponent<StatusItemsSuppressedComp>()?.ShouldShowIcon(statusItem) ?? statusItem.ShouldShowIcon();
        }
    }
}
