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

    [HarmonyPatch(typeof(PlanScreen), "ClearButtons")]
    public class FastClearAlerts_Patch
    {
        static bool Prepare() => Options.Opts.AlertMode == Options.Mode.FastClear;

        static void Prefix(Dictionary<BuildingDef, KToggle> ___ActiveToggles, Dictionary<Tag, HashedString> ___tagCategoryMap)
        {
            if (___ActiveToggles == null || ___ActiveToggles.Count == 0)
                return;

            var item = ___ActiveToggles.First();
            HashedString category = ___tagCategoryMap[item.Key.Tag];
            object[] parameters = new object[] { category, null };

            AccessTools.Method(typeof(PlanScreen), "GetToggleEntryForCategory").Invoke(PlanScreen.Instance, parameters);
            List<Tag> pendingResearchAttentions = parameters[1].GetType().GetField("pendingResearchAttentions").GetValue(parameters[1]) as List<Tag>;
            KIconToggleMenu.ToggleInfo toggleInfo = parameters[1].GetType().GetField("toggleInfo").GetValue(parameters[1]) as KIconToggleMenu.ToggleInfo;

            pendingResearchAttentions.Clear();
            toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
        }
    }
}
