using HarmonyLib;
using Klei.AI;
using System;
using UnityEngine;

namespace InfiniteResearch
{
    abstract class InfiniteWorkable
    {
        private static StatusItem requiresAttributeRange;

        private static bool ShouldChoreBeWorked(Chore chore) => IsDupeInRightAttributeRange(chore.driver.gameObject, chore);
        private static bool ShouldChoreBeWorked(Chore.Precondition.Context context) => IsDupeInRightAttributeRange(context.consumerState.gameObject, context.chore);

        private static bool IsDupeInRightAttributeRange(GameObject go, Chore chore)
        {
            var learnLevel = go.GetComponent<AttributeLevels>().GetAttributeLevel("Learning").GetLevel();
            var attributeRange = GetAttributeRange(chore);
            return learnLevel >= attributeRange.min && learnLevel <= attributeRange.max;
        }

        private static (int min, int max) GetAttributeRange(Chore chore)
        {
            var tuning = Options.Opts.GetIRTuningForGO(chore.target.gameObject);
            return (tuning.Min, tuning.Max);
        }

        protected static void ModifyChore(Workable instance, Chore chore, Func<Workable, bool> isEndlessWorking)
        {
            var precondition = new Chore.Precondition()
            {
                id = "RequireAttributeRange",
                fn = delegate (ref Chore.Precondition.Context context, object data) { return !isEndlessWorking(instance) || ShouldChoreBeWorked(context); },
                description = MYSTRINGS.DUPLICANTS.CHORES.PRECONDITIONS.REQUIRES_ATTRIBUTE_RANGE.DESCRIPTION
            };
            chore.AddPrecondition(precondition, null);
        }

        protected static void RemoveDupe(Workable instance, Chore chore, Func<Workable, bool> isEndlessWorking)
        {
            KSelectable kSelectable = instance.GetComponent<KSelectable>();

            if (isEndlessWorking(instance) && chore != null)
            {
                bool isBeingWorked = false;
                if (chore.driver != null)
                {
                    isBeingWorked = true;
                    if (!ShouldChoreBeWorked(chore))
                        chore.driver.StopChore();
                }
                kSelectable.ToggleStatusItem(requiresAttributeRange, !isBeingWorked, GetAttributeRange(chore));
            }
            else
                kSelectable.ToggleStatusItem(requiresAttributeRange, false);
        }

        [HarmonyPatch(typeof(ResearchCenter), nameof(ResearchCenter.OnSpawn))]
        private static class AddStatusItem
        {
            static void Postfix(Workable __instance)
            {
                requiresAttributeRange = new StatusItem("RequiresAttributeRange", "INFINITERESEARCH.MYSTRINGS.BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID)
                {
                    resolveStringCallback = (string str, object obj) =>
                    {
                        var minMax = ((int, int))obj;
                        return str.Replace("{Attributes}", minMax.Item1 + " - " + minMax.Item2);
                    }
                };

                __instance.attributeExperienceMultiplier = Options.Opts.GetIRTuningForGO(__instance.gameObject).ExpRate;
            }
        }
    }
}
