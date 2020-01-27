using Harmony;
using Klei.AI;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InfiniteResearch
{
    class InfiniteResearchCenter : InfiniteWorkable
    {
        static bool IsEndlessWorking(Workable instance) => Research.Instance.GetActiveResearch() == null && instance.GetComponent<InfiniteModeToggleButton>().isInfiniteMode;

        [HarmonyPatch(typeof(ResearchCenter), "UpdateWorkingState")]
        static class InfiniteResearch_Patch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                int counter = 1;

                foreach (CodeInstruction i in instructions)
                {
                    if (counter <= 2 && i.opcode == OpCodes.Ldc_I4_0)
                    {
                        counter++;
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(InfiniteResearchCenter), nameof(InfiniteResearchCenter.IsEndlessWorking)));
                    }
                    else
                        yield return i;
                }
            }
        }

        [HarmonyPatch(typeof(ResearchCenter), nameof(ResearchCenter.GetPercentComplete))]
        static class NoProgressBar_Patch
        {
            static bool Prefix(ResearchCenter __instance, ref float __result)
            {
                if (IsEndlessWorking(__instance))
                {
                    __result = __instance.worker.GetComponent<AttributeLevels>().GetPercentComplete("Learning");
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ResearchCenter), nameof(ResearchCenter.Sim200ms))]
        static class Sim_Patch { static void Postfix(ResearchCenter __instance, Chore ___chore) => RemoveDupe(__instance, ___chore, IsEndlessWorking); }

        [HarmonyPatch(typeof(ResearchCenter), "CreateChore")]
        static class CreateChore_Patch { static void Postfix(ResearchCenter __instance, Chore __result) => ModifyChore(__instance, __result, IsEndlessWorking); }
    }
}
