using Harmony;

namespace SufficientOxygenGeneration
{
    [HarmonyPatch(typeof(Tutorial), "SufficientOxygenLastCycleAndThisCycle")]
    public class Threshold_Patch
    {
        static bool Prefix(ref bool __result)
        {
            if (ReportManager.Instance.YesterdaysReport == null || Options.Opts.Mode == Options.OxygenThresholMode.Off)
            {
                __result = true;
                return false;
            }

            ReportManager.ReportEntry entryYest = ReportManager.Instance.YesterdaysReport.GetEntry(ReportManager.ReportType.OxygenCreated);
            ReportManager.ReportEntry entryToday = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.OxygenCreated);
            
            switch (Options.Opts.Mode)
            {
                case Options.OxygenThresholMode.Constant:
                    __result = entryToday.Net > -Options.Opts.ConstantThreshold || entryYest.Net > 0.0001f;
                    break;
                case Options.OxygenThresholMode.Ratio:
                    __result = ((entryToday.Positive / -entryToday.Negative) > Options.Opts.RatioThreshold || entryYest.Net > 0.0001f) && GameClock.Instance.GetTimeSinceStartOfCycle() > Options.Opts.TimeDelay;
                    break;
                default:
                    __result = true;
                    break;
            }
            return false;
        }
    }
}
