using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace NoResearchAlerts
{
    [HarmonyPatch(typeof(PlanCategoryNotifications), nameof(PlanCategoryNotifications.ToggleAttention))]
    public class NoResearchAlerts_Patch
    {
        static bool Prepare() => Options.Opts.AlertMode == Options.Mode.None;

        static void Prefix(ref bool active)
        {
            active = false;
        }
    }

    [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.ClearButtons))]
    public class FastClearAlerts_Patch
    {
        static bool Prepare() => Options.Opts.AlertMode == Options.Mode.FastClear;

        static void Prefix(Dictionary<BuildingDef, PlanBuildingToggle> ___ActiveCategoryBuildingToggles, Dictionary<Tag, HashedString> ___tagCategoryMap)
        {
            if (___ActiveCategoryBuildingToggles == null || ___ActiveCategoryBuildingToggles.Count == 0)
                return;

            var item = ___ActiveCategoryBuildingToggles.First();
            HashedString category = ___tagCategoryMap[item.Key.Tag];

            PlanScreen.Instance.GetToggleEntryForCategory(category, out PlanScreen.ToggleEntry toggleEntry);

            toggleEntry.pendingResearchAttentions.Clear();
            foreach (var kvp in ___ActiveCategoryBuildingToggles)
                kvp.Value.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);

            toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
        }
    }
}
