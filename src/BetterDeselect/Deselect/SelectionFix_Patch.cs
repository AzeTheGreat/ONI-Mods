using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace BetterDeselect.Deselect
{
    static class SelectionFix
    {
        [HarmonyPatch]
        private class ReselectionFix_Patch
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(PlanScreen), nameof(PlanScreen.OnSelectBuilding));
                yield return AccessTools.Method(typeof(PlanScreen), "OnClickCategory");
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return instructions.MethodReplacer(AccessTools.Method(typeof(PlanScreen), "CloseRecipe"),
                    AccessTools.Method(typeof(ReselectionFix_Patch), nameof(ReselectionFix_Patch.CloseRecipeWrapper)));
            }

            private static void CloseRecipeWrapper(PlanScreen instance, bool playSound)
            {
                if(Options.Opts.Reselect == Options.ReselectMode.Close)
                    instance.CloseRecipe(playSound);
            }
        }
    }
}
