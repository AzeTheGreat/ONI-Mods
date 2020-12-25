using Database;
using Harmony;

namespace NoStutter
{
    [HarmonyPatch(typeof(BuildOutsideStartBiome), nameof(BuildOutsideStartBiome.Success))]
    class PreventAchievementCheck
    {
        static bool Prefix(ref bool __result)
        {
            if (Options.Opts.OnlyInDebug && !Game.Instance.debugWasUsed)
                return true;

            __result = Options.Opts.Mode switch
            {
                Options.DisableMode.Prevent => false,
                Options.DisableMode.InstantSuccess => true,
                _ => false
            };

            if(__result == true)
                Game.Instance.unlocks.Unlock("buildoutsidestartingbiome");

            return false;
        }
    }
}
