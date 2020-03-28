using Database;
using Harmony;

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
