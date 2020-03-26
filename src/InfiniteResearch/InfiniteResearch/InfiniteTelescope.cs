 using AzeLib.Extensions;
using Harmony;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InfiniteResearch
{
    class InfiniteTelescope : InfiniteWorkable
    {
        static bool IsEndlessWorking(Workable instance) => !SpacecraftManager.instance.HasAnalysisTarget() && instance.GetComponent<InfiniteModeToggleButton>().isInfiniteMode;

        [HarmonyPatch(typeof(Telescope), "UpdateWorkingState")]
        static class InfiniteResearch_Patch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                bool first = true;

                foreach (CodeInstruction i in instructions)
                {
                    if (first && i.OpCodeIs(OpCodes.Ldc_I4_0))
                    {
                        first = false;
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(InfiniteTelescope), nameof(InfiniteTelescope.IsEndlessWorking)));
                    }
                    else
                        yield return i;
                }
            }
        }

        [HarmonyPatch(typeof(Telescope), "OnWorkableEvent")]
        static class ProgressBar_Patch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var i in instructions)
                {
                    if (i.OpCodeIs(OpCodes.Stsfld))
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ProgressBar_Patch), nameof(ProgressBar_Patch.AlterProgBar)));
                    }
                        
                    yield return i;
                }
            }

            private static Func<float> AlterProgBar(Func<float> originalFunc, Telescope instance)
            {
                if (IsEndlessWorking(instance))
                    return () => instance.worker.GetComponent<AttributeLevels>().GetPercentComplete("Learning");
                return originalFunc;
            }
        }

        [HarmonyPatch(typeof(Telescope), nameof(Telescope.Sim200ms))]
        static class Sim_Patch { static void Postfix(Telescope __instance, Chore ___chore) => RemoveDupe(__instance, ___chore, IsEndlessWorking); }

        [HarmonyPatch(typeof(Telescope), "CreateChore")]
        static class CreateChore_Patch { static void Postfix(Telescope __instance, Chore __result) => ModifyChore(__instance, __result, IsEndlessWorking); }
    }
}
