using FMOD;
using Harmony;
using Klei.AI;
using PeterHan.PLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TUNING;
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
                    return (Options.Opts.ResearchCenterMin, Options.Opts.ResearchCenterMax);
                case "AdvancedResearchCenterComplete":
                    return (Options.Opts.AdvancedResearchCenterMin, Options.Opts.AdvancedResearchCenterMax);
                case "TelescopeComplete":
                    return (Options.Opts.TelescopeMin, Options.Opts.TelescopeMax);
                case "CosmicResearchCenterComplete":
                    return (Options.Opts.CosmicResearchCenterMin, Options.Opts.CosmicResearchCenterMax);
                default:
                    return (0, 0);
            }
        }

        protected static void ModifyChore(Workable instance, Chore chore, Func<Workable, bool> isEndlessWorking)
        {
            var precondition = new Chore.Precondition()
            {
                id = "RequireAttributeRange",
                fn = delegate (ref Chore.Precondition.Context context, object data) { return !isEndlessWorking(instance) || ShouldChoreBeWorked(context); },
                description = DUPLICANTS.CHORES.PRECONDITIONS.REQUIRES_ATTRIBUTE_RANGE.DESCRIPTION
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

        [HarmonyPatch]
        private static class AddStatusItem
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(Telescope), "OnSpawn");
                yield return AccessTools.Method(typeof(ResearchCenter), "OnSpawn");
            }

            static void Postfix(Workable __instance)
            {
                requiresAttributeRange = new StatusItem("RequiresAttributeRange", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID)
                {
                    resolveStringCallback = (string str, object obj) =>
                    {
                        var minMax = ((int, int))obj;
                        return str.Replace("{Attributes}", minMax.Item1 + " - " + minMax.Item2);
                    }
                };

                __instance.attributeExperienceMultiplier = GetMultiplier(__instance.gameObject);
            }

            private static float GetMultiplier(GameObject go)
            {
                switch (go.name)
                {
                    case "ResearchCenterComplete":
                        return Options.Opts.ResearchCenterExpRate;
                    case "AdvancedResearchCenterComplete":
                        return Options.Opts.AdvancedResearchCenterExpRate;
                    case "TelescopeComplete":
                        return Options.Opts.TelescopeExpRate;
                    case "CosmicResearchCenterComplete":
                        return Options.Opts.CosmicResearchCenterExpRate;
                    default:
                        return TUNING.SKILLS.ALL_DAY_EXPERIENCE;
                }
            }
        }
    }
}
