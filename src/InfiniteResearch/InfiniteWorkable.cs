using Harmony;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace InfiniteResearch
{
    abstract class InfiniteWorkable
    {
        private static StatusItem requiresAttributeRange;

        private static bool ShouldChoreBeWorked(Chore chore)
        {
            var learnLevel = chore.driver.GetComponent<AttributeLevels>().GetAttributeLevel("Learning").GetLevel();
            var attributeRange = GetAttributeRange(chore);
            return learnLevel >= attributeRange.Item1 && learnLevel <= attributeRange.Item2;
        }

        private static bool ShouldChoreBeWorked(Chore.Precondition.Context context)
        {
            var learnLevel = context.consumerState.gameObject.GetComponent<AttributeLevels>().GetAttributeLevel("Learning").GetLevel();
            var attributeRange = GetAttributeRange(context.chore);
            return learnLevel >= attributeRange.Item1 && learnLevel <= attributeRange.Item2;
        }

        private static (int, int) GetAttributeRange(Chore chore)
        {
            switch (chore.target.gameObject.name)
            {
                case "ResearchCenterComplete":
                    return (0, 4);
                case "AdvancedResearchCenterComplete":
                    return (5, 9);
                case "TelescopeComplete":
                    return (10, 14);
                case "CosmicResearchCenterComplete":
                    return (15, 19);
                default:
                    return (0, 0);
            }
        }

        private static void AddPrecondition(Chore chore)
        {
            var precondition = new Chore.Precondition()
            {
                id = "RequireSkillLevel",
                fn = delegate (ref Chore.Precondition.Context context, object data)
                {
                    return ShouldChoreBeWorked(context);
                }
            };
            chore.AddPrecondition(precondition, null);
        }

        protected static void ModifyChore(Workable instance, Chore chore, Func<Workable, bool> isEndlessWorking)
        {
            if (isEndlessWorking(instance))
                AddPrecondition(chore);
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

        [HarmonyPatch]
        private static class AddStatusItem
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(Telescope), "OnSpawn");
                yield return AccessTools.Method(typeof(ResearchCenter), "OnSpawn");
            }

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
