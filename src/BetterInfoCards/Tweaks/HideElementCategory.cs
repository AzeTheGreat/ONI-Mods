using HarmonyLib;
using System.Collections.Generic;

namespace BetterInfoCards;

public class HideElementCategory
{
    public static IEnumerable<CodeInstruction> ChildTranspiler(IEnumerable<CodeInstruction> codes)
    {
        return new CodeMatcher(codes)
            .End()
            .MatchStartBackwards(CodeMatch.Calls((Element _) => _.GetMaterialCategoryTag()))
            .MatchStartBackwards(CodeMatch.Calls(AccessTools.PropertyGetter(typeof(Element), nameof(Element.IsVacuum))))
            .InsertAfter(CodeInstruction.CallClosure(static (bool isVacuum) => Options.Opts.HideElementCategories || isVacuum))
            .InstructionEnumeration();
    }
}