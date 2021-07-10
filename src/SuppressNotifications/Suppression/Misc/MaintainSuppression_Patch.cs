using AzeLib.Extensions;
using HarmonyLib;

namespace SuppressNotifications.Suppression.Misc
{
    [HarmonyPatch(typeof(BuildingHP.SMInstance), "CreateProgressBar")]
    class MaintainSuppression_Patch
    {
        static void Postfix(BuildingHP.SMInstance __instance, ref ProgressBar ___progressBar)
        {
            if (___progressBar)
            {
                var dmgSuppressionComp = __instance.GetComponent<BuildingHealthSuppressedComp>();

                // Grabbing the progress bar this way avoids reflection on all but user interaction refreshes.
                if (dmgSuppressionComp)
                    ___progressBar.ToggleVisibility(dmgSuppressionComp.HideDmgBar);
            }
        }
    }
}
