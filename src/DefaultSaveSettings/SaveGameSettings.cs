using HarmonyLib;

namespace DefaultSaveSettings
{
    [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
    class SaveGameSettings
    {
        static void Postfix(SaveGame __instance)
        {
            var configScreen = new SaveConfigurationScreen();
            __instance.AutoSaveCycleInterval = configScreen.SliderValueToCycleCount((int)Options.Opts.AutoSaveInterval);
            __instance.TimelapseResolution = configScreen.SliderValueToResolution((int)Options.Opts.TimelapseResolution);
            __instance.enableAutoDisinfect = Options.Opts.EnableAutoDisinfect;
            __instance.minGermCountForDisinfect = Options.Opts.DisinfectMinGermCount;
        }
    }
}
