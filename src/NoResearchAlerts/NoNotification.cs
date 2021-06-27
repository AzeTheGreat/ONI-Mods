using HarmonyLib;

namespace NoResearchAlerts
{
    [HarmonyPatch(typeof(ResearchEntry), nameof(ResearchEntry.ResearchCompleted))]
    class NoNotification_Patch
    {
        static bool Prepare() => Options.Opts.SuppressMessage;

        static void Prefix(ref bool notify)
        {
            notify = false;
        }
    }
}
