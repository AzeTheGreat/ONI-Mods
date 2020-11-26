using Harmony;
using System.Collections.Generic;

namespace BetterInfoCards.Util
{
    // Transpiling this method is incredibly slow on the Harmony side since it's so long.
    // Grouping the transpilers reduces the number of times it has to get reapplied, and reduces the exponential growth effect.

    [HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
    class GroupedTranspiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var results = CollectHoverInfo.GetSelectInfo_Patch.ChildTranspiler(instructions);
            return DetectRunStart_Patch.ChildTranspiler(results);
        }
    }
}
