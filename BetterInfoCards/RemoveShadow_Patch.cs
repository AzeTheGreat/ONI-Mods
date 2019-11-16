using Harmony;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new System.Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) })]
    class RemoveShadow_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool inNopRegion = false;
            bool firstTarget = true;
            MethodInfo target = AccessTools.Method(typeof(HoverTextDrawer), "AddIndent");
            MethodInfo target2 = AccessTools.Method(typeof(RectTransform), "set_sizeDelta");

            foreach (CodeInstruction i in instructions)
            {
                if (firstTarget && i.opcode == OpCodes.Call && i.operand == target)
                {
                    inNopRegion = true;
                    firstTarget = false;
                    yield return i;
                }
                else if (inNopRegion && i.opcode == OpCodes.Callvirt && i.operand == target2)
                {
                    inNopRegion = false;
                    yield return new CodeInstruction(OpCodes.Nop);
                }
                else if (inNopRegion)
                    yield return new CodeInstruction(OpCodes.Nop);
                else
                    yield return i;
            }
        }
    }
}
