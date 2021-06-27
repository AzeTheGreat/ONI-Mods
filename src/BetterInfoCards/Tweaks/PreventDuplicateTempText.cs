using Database;
using HarmonyLib;

namespace BetterInfoCards.Tweaks
{
    [HarmonyPatch(typeof(MiscStatusItems), "CreateStatusItems")]
    class PreventDuplicateTempText
    {
        static void Postfix(MiscStatusItems __instance)
        {
            __instance.OreTemp.status_overlays &= ~(int)StatusItem.StatusItemOverlays.Temperature;
        }
    }
}
