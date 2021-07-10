using HarmonyLib;

namespace DefaultBuildingSettings
{
    [HarmonyPatch(typeof(Repairable.States), nameof(Repairable.States.InitializeStates))]
    class DisableAutoRepair_Patch
    {
        static void Postfix(Repairable.States __instance, ref StateMachine.BaseState default_state)
        {
            if(Options.Opts.AutoRepairOff)
                default_state = __instance.forbidden;
        }
    }
}
