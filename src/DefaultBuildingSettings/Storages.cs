using HarmonyLib;
using UnityEngine;

namespace DefaultBuildingSettings
{
    static class Storages
    {
        [HarmonyPatch(typeof(TreeFilterable), "OnPrefabInit")]
        private class SweepOnly_Patch
        {
            static void Postfix(TreeFilterable __instance)
            {
                // TreeFilterableSideScreen.IsValidForTarget manually blacklists FlatTagFilterables
                // Without this, rocket oxidizer tanks get set to sweep only despite not having the UI
                // Must use reflection since FlatTagFilterable doesn't exist in vanilla currently
                if (Options.Opts.SweepOnly && __instance.GetComponent(nameof(FlatTagFilterable)) == null && __instance.storage.allowSettingOnlyFetchMarkedItems)
                    __instance.storage.SetOnlyFetchMarkedItems(true);
                    
            }
        }

        internal static bool SetReservoirValues(GameObject go)
        {
            if (!(go.GetComponent<SmartReservoir>() is SmartReservoir smartReservoir))
                return false;

            smartReservoir.activateValue = Options.Opts.ReservoirActivateValue;
            smartReservoir.deactivateValue = Options.Opts.ReservoirDeactivateValue;
            return true;
        }
    }
}
