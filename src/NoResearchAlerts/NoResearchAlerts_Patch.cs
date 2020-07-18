using Harmony;
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

        static void Prefix(Dictionary<BuildingDef, KToggle> ___ActiveToggles, Dictionary<Tag, HashedString> ___tagCategoryMap)
        {
            if (___ActiveToggles == null || ___ActiveToggles.Count == 0)
                return;

            var item = ___ActiveToggles.First();
            HashedString category = ___tagCategoryMap[item.Key.Tag];

            PlanScreen.Instance.GetToggleEntryForCategory(category, out PlanScreen.ToggleEntry toggleEntry);

            toggleEntry.pendingResearchAttentions.Clear();
            toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
        }
    }
}
