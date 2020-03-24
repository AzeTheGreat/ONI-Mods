using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultSaveSettings
{
    [HarmonyPatch(typeof(global::SaveGame), "OnPrefabInit")]
    class SaveGame
    {
        static void Postfix(global::SaveGame __instance)
        {
            var trav = Traverse.Create(new SaveConfigurationScreen());
            __instance.AutoSaveCycleInterval = trav.Method("SliderValueToCycleCount", new Type[] { typeof(int) }).GetValue<int>(Options.Opts.AutoSaveInterval);
            __instance.TimelapseResolution = trav.Method("SliderValueToResolution", new Type[] { typeof(int) }).GetValue<Vector2I>(Options.Opts.TimelapseResolution);
        }
    }
}
