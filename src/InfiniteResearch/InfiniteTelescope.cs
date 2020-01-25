using Harmony;
using Klei.AI;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace InfiniteResearch
{
    static class InfiniteTelescope
    {
        private static StatusItem requiresAttributeRange;

        private static bool ShouldMinionWork(GameObject minionGO)
        {
            var modifiers = minionGO.GetComponent<Modifiers>().attributes.Get("Learning").Modifiers;
            for (int i = 0; i < modifiers.Count; i++)
            {
                if (modifiers[i].Description == STRINGS.DUPLICANTS.MODIFIERS.SKILLLEVEL.NAME)
                {
                    float learnLevel = modifiers[i].Value;
                    return learnLevel >= 0f && learnLevel < 1f;
                }
            }
            return false;
        }

        private static void AddPrecondition(Chore chore)
        {
            var precondition = new Chore.Precondition()
            {
                id = "RequireSkillLevel",
                fn = delegate (ref Chore.Precondition.Context context, object data)
                {
                    var minionGO = context.consumerState.gameObject;
                    return ShouldMinionWork(minionGO);
                }
            };
            chore.AddPrecondition(precondition, null);
        }

        [HarmonyPatch(typeof(Telescope), "UpdateWorkingState")]
        private static class InfiniteResearch_Patch
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

            static void Postfix(Telescope __instance, Chore ___chore)
            {
                if (lastAnalysisID != -1 && SpacecraftManager.instance.AreAllDestinationsAnalyzed())
                {
                    __instance.GetComponent<BuildingEnabledButton>().IsEnabled = false;
                    AddPrecondition(___chore);
                }

                lastAnalysisID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
            }
        }

        [HarmonyPatch(typeof(Telescope), "OnWorkableEvent")]
        private static class NoProgressBar_Patch
        {
            static void Postfix(Telescope __instance)
            {
                if (SpacecraftManager.instance.AreAllDestinationsAnalyzed())
                    __instance.ShowProgressBar(false);
            }
        }

        [HarmonyPatch(typeof(Telescope), "CreateChore")]
        private static class ModifyChore
        {
            public static void Postfix(Chore __result)
            {
                if (SpacecraftManager.instance.AreAllDestinationsAnalyzed())
                    AddPrecondition(__result);
            }
        }

        [HarmonyPatch(typeof(Telescope), nameof(Telescope.Sim200ms))]
        private static class RemoveDupe
        {
            static void Postfix(Telescope __instance, Chore ___chore)
            {
                if (SpacecraftManager.instance.AreAllDestinationsAnalyzed())
                {
                    bool isBeingWorked = false;
                    if (___chore != null && ___chore.driver != null)
                    {
                        isBeingWorked = true;
                        var minionGO = ___chore.driver.gameObject;
                        if (!ShouldMinionWork(minionGO))
                            ___chore.driver.StopChore();
                    }

                    KSelectable kSelectable = __instance.GetComponent<KSelectable>();
                    kSelectable.ToggleStatusItem(requiresAttributeRange, !isBeingWorked, (10, 14));
                }
            }
        }

        [HarmonyPatch(typeof(Telescope), "OnSpawn")]
        private static class AddStatusItem
        {
            static void Postfix()
            {
                requiresAttributeRange = new StatusItem("RequiresAttributeRange", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID)
                {
                    resolveStringCallback = (string str, object obj) =>
                    {
                        var minMax = ((int, int))obj;
                        return str.Replace("{Attributes}", minMax.Item1 + " - " + minMax.Item2);
                    }
                };
            }
        }
    }
}
