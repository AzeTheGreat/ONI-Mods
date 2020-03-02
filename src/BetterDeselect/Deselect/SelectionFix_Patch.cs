using AzeLib.Extensions;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

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

            static bool Prepare() => Options.Opts.Mode == Options.FixMode.Hold;

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                MethodInfo targetMethodInfo = AccessTools.Method(typeof(PlanScreen), "CloseRecipe");

                foreach (CodeInstruction i in instructions)
                {
                    if (i.Is(OpCodes.Call, targetMethodInfo))
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                        yield return new CodeInstruction(OpCodes.Pop);
                        yield return new CodeInstruction(OpCodes.Nop);
                    }
                    else
                        yield return i;
                }
            }
        }
    }
}
