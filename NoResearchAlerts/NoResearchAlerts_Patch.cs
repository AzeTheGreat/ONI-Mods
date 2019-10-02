using Harmony;
using PeterHan.PLib;

namespace NoResearchAlerts
{
    [HarmonyPatch(typeof(PlanCategoryNotifications), nameof(PlanCategoryNotifications.ToggleAttention))]
    public class NoResearchAlerts_Patch
    {
        public static void OnLoad()
        {
            PUtil.LogModInit();
        }

        static void Prefix(ref bool active)
        {
            active = false;
        }
    }
}
