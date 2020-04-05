using AzeLib.Extensions;
using Harmony;
using System;

namespace DefaultSaveSettings
{
    [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
    class SaveGameSettings
    {
        private static readonly Func<int, int> sliderValueToCycleCount = AccessTools.Method(typeof(SaveConfigurationScreen), "SiderValueToCycleCount").MakeDelegate<Func<int, int>>();
        private static readonly Func<int, Vector2I> sliderValueToResolution = AccessTools.Method(typeof(SaveConfigurationScreen), "SliderValueToResolution").MakeDelegate<Func<int, Vector2I>>();

        static void Postfix(SaveGame __instance)
        {
            __instance.AutoSaveCycleInterval = sliderValueToCycleCount(Options.Opts.AutoSaveInterval);
            __instance.TimelapseResolution = sliderValueToResolution(Options.Opts.TimelapseResolution);
            __instance.enableAutoDisinfect = Options.Opts.EnableAutoDisinfect;
            __instance.minGermCountForDisinfect = Options.Opts.DisinfectMinGermCount;
        }
    }
}
