using AzeLib.Extensions;
using Database;
using HarmonyLib;
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
                    if (counter <= 2 && i.OpCodeIs(OpCodes.Ldc_I4_0))
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
        static class ProgressBar_Patch
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

        [HarmonyPatch(typeof(DuplicantStatusItems), "CreateStatusItems")]
        static class LearningStatusItem_Patch
        {
            static void Postfix(StatusItem ___Researching)
            {
                ___Researching.resolveStringCallback = (string str, object data) => getString(str, data, MYSTRINGS.DUPLICANTS.STATUSITEMS.LEARNING.NAME);
                ___Researching.resolveTooltipCallback = (string str, object data) => getString(str, data, MYSTRINGS.DUPLICANTS.STATUSITEMS.LEARNING.TOOLTIP);

                string getString(string str, object data, string nameOrTooltip)
                {
                    TechInstance tech_instance = Research.Instance.GetActiveResearch();
                    string result;
                    if (tech_instance != null)
                        result = str.Replace("{Tech}", tech_instance.tech.Name);
                    else
                        result = nameOrTooltip;
                    return result;
                }
            }
        }

        // Not necessary for functionality, but prevents the log from spamming warnings about adding research points with no target.
        [HarmonyPatch(typeof(ResearchCenter), "ConvertMassToResearchPoints")]
        static class PreventResearchPoints_Patch { static bool Prefix(ResearchCenter __instance) => !IsEndlessWorking(__instance); }

        [HarmonyPatch(typeof(ResearchCenter), nameof(ResearchCenter.Sim200ms))]
        static class Sim_Patch { static void Postfix(ResearchCenter __instance, Chore ___chore) => RemoveDupe(__instance, ___chore, IsEndlessWorking); }

        [HarmonyPatch(typeof(ResearchCenter), "CreateChore")]
        static class CreateChore_Patch { static void Postfix(ResearchCenter __instance, Chore __result) => ModifyChore(__instance, __result, IsEndlessWorking); }
    }
}
