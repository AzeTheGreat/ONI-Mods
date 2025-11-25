using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace BetterDeselect;

[HarmonyPatch]
class ReselectionFix
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(PlanScreen), nameof(PlanScreen.OnSelectBuilding));
        yield return AccessTools.Method(typeof(PlanScreen), nameof(PlanScreen.OnClickCategory));
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions.MethodReplacer(AccessTools.Method(typeof(PlanScreen), nameof(PlanScreen.CloseRecipe)),
            AccessTools.Method(typeof(ReselectionFix), nameof(CloseRecipeWrapper)));
    }

    private static void CloseRecipeWrapper(PlanScreen instance, bool playSound)
    {
        if (Options.Opts.Reselect == Options.ReselectMode.Close)
            instance.CloseRecipe(playSound);
    }
}