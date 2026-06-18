using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards;

// DrawIcon draws two separate icons, one standard, and one "shadow".
// The shadow appears to change nothing visually, and will decrease performance.
// Transpiler removes the code that draws these.
[HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), [typeof(Sprite), typeof(Color), typeof(int), typeof(int)])]
class NoDrawIconShadows
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
    {
        return new CodeMatcher(codes)
            .MatchStartForward(CodeMatch.Calls((HoverTextDrawer _) => _.AddIndent))
            .Advance()
            .RemoveUntilForward(CodeMatch.Calls(AccessTools.PropertySetter(typeof(RectTransform), nameof(RectTransform.sizeDelta))))
            .RemoveInstruction()
            .InstructionEnumeration();
    }
}