using Harmony;
using System;

namespace DefaultSaveSettings
{
    [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
    class SaveGameSettings
    {
        static void Postfix(SaveGame __instance)
        {
            var trav = Traverse.Create(new SaveConfigurationScreen());
            __instance.AutoSaveCycleInterval = trav.Method("SliderValueToCycleCount", new Type[] { typeof(int) }).GetValue<int>(Options.Opts.AutoSaveInterval);
            __instance.TimelapseResolution = trav.Method("SliderValueToResolution", new Type[] { typeof(int) }).GetValue<Vector2I>(Options.Opts.TimelapseResolution);
            __instance.enableAutoDisinfect = Options.Opts.EnableAutoDisinfect;
            __instance.minGermCountForDisinfect = Options.Opts.DisinfectMinGermCount;
        }
    }
}
