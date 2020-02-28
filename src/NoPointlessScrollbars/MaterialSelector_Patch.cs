using AzeLib.Extensions;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace NoPointlessScrollbars
{
    [HarmonyPatch(typeof(MaterialSelector), "UpdateScrollBar")]
    static class MaterialSelector_Patch
    {
        private static bool shouldShow;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var setActiveMethod = AccessTools.Method(typeof(GameObject), nameof(GameObject.SetActive));

            foreach (var i in instructions)
            {
                if (i.Is(OpCodes.Callvirt, setActiveMethod))
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MaterialSelector_Patch), nameof(MaterialSelector_Patch.Patch)));
                yield return i;
            }
        }

        static void Postfix(MaterialSelector __instance)
        {
            __instance.ScrollRect.horizontal = shouldShow;
        }

        private static bool Patch(bool show) => shouldShow = show;
    }
}
