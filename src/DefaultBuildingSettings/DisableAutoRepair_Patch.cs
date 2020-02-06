using Harmony;

namespace DefaultBuildingSettings
{
    [HarmonyPatch(typeof(Repairable.States), nameof(Repairable.States.InitializeStates))]
    class DisableAutoRepair_Patch
    {
        static bool Prepare => Options.Opts.AutoRepairOff;
        static void Postfix(Repairable.States __instance, ref StateMachine.BaseState default_state)
        {
            default_state = __instance.forbidden;
        }
    }
}
