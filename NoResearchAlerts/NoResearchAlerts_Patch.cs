using Harmony;

namespace NoResearchAlerts
{
    [HarmonyPatch(typeof(PlanCategoryNotifications), nameof(PlanCategoryNotifications.ToggleAttention))]
    public class NoResearchAlerts_Patch
    {
        static void Prefix(ref bool active)
        {
            active = false;
        }
    }
}
