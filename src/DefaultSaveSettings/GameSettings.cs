using Harmony;

namespace DefaultSaveSettings
{
    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    public class GameSettings
    {
        static void Postfix(Game __instance)
        {
            __instance.advancedPersonalPriorities = Options.Opts.EnableProximityDefault;
        }
    }
}
