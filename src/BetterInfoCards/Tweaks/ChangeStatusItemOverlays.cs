using Database;
using HarmonyLib;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(MiscStatusItems), nameof(MiscStatusItems.CreateStatusItems))]
    class ChangeStatusItemOverlays
    {
        static void Postfix(MiscStatusItems __instance)
        {
            // Prevents the duplicate temp status from being drawn in the temp overlay.
            __instance.OreTemp.status_overlays &= ~(int)StatusItem.StatusItemOverlays.Temperature;
            // Prevent the unreachable status from being drawn on any overlay.
            __instance.PickupableUnreachable.status_overlays = 0;

            if (Options.Opts.HideElementCategories)
                __instance.ElementalCategory.status_overlays = 0;
        }
    }
}
