using Harmony;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InfiniteResearch
{
    [HarmonyPatch(typeof(Telescope), "UpdateWorkingState")]
    class InfiniteResearch_Patch
    {
        static int lastAnalysisID = -1;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool first = true;

            foreach (CodeInstruction i in instructions)
            {
                if (first && i.opcode == OpCodes.Ldc_I4_0)
                {
                    first = false;
                    yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(SpacecraftManager), nameof(SpacecraftManager.instance)));
                    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(SpacecraftManager), nameof(SpacecraftManager.AreAllDestinationsAnalyzed)));
                }

                else
                    yield return i;
            }
        }

        static void Postfix(Telescope __instance)
        {
            if (lastAnalysisID != -1 && SpacecraftManager.instance.AreAllDestinationsAnalyzed())
                __instance.GetComponent<BuildingEnabledButton>().IsEnabled = false;

            lastAnalysisID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
        }
    }

    [HarmonyPatch(typeof(Telescope), "OnWorkableEvent")]
    class NoProgressBar_Patch
    {
        static void Postfix(Telescope __instance)
        {
            if (SpacecraftManager.instance.AreAllDestinationsAnalyzed())
                __instance.ShowProgressBar(false);
        }
    }
}
