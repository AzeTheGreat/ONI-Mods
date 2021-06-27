using HarmonyLib;

namespace DefaultSaveSettings
{
    // This must be in Immigration and not Game because it is used to set default priorities.
    [HarmonyPatch(typeof(Immigration), "OnPrefabInit")]
    public class GameSettings
    {
        static void Postfix()
        {
            Game.Instance.advancedPersonalPriorities = Options.Opts.EnableProximity;
        }
    }
}
