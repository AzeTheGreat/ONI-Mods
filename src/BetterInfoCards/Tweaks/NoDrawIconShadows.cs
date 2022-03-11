using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterInfoCards
{
    // DrawIcon draws two separate icons, one standard, and one "shadow".
    // The shadow appears to change nothing visually, and will decrease performance.
    // Transpiler removes the code that draws these.
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) })]
    class NoDrawIconShadows
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool inNopRegion = false;
            bool firstTarget = true;
            MethodInfo target = AccessTools.Method(typeof(HoverTextDrawer), "AddIndent");
            MethodInfo target2 = AccessTools.Method(typeof(RectTransform), "set_sizeDelta");

            foreach (CodeInstruction i in instructions)
            {
                if (firstTarget && i.Is(OpCodes.Call, target))
                {
                    inNopRegion = true;
                    firstTarget = false;
                    yield return i;
                }
                else if (inNopRegion && i.Is(OpCodes.Callvirt, target2))
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
