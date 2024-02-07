using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace NoPointlessScrollbars
{
    [HarmonyPatch(typeof(MaterialSelector), nameof(MaterialSelector.UpdateScrollBar))]
    static class HideMaterialScrollbar
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var setActiveMethod = AccessTools.Method(typeof(GameObject), nameof(GameObject.SetActive));

            return codes.Manipulator(
                i => i.Calls(setActiveMethod),
                i => new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(HideMaterialScrollbar), nameof(Splice)),
                    i
                });
        }

        private static bool Splice(bool show, MaterialSelector instance) => instance.ScrollRect.vertical = show;
    }
}
