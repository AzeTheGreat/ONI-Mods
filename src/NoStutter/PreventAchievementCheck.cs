using Database;
using HarmonyLib;

namespace NoStutter
{
    [HarmonyPatch(typeof(BuildOutsideStartBiome), nameof(BuildOutsideStartBiome.Success))]
    class PreventAchievementCheck
    {
        static int lastCycleChecked;

        static bool Prefix()
        {
            // Only check the achievement once each cycle.
            if (GameUtil.GetCurrentCycle() != lastCycleChecked)
            {
                lastCycleChecked = GameUtil.GetCurrentCycle();
                return true;
            }
            return false;
        }
    }
}
