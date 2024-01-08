using HarmonyLib;
using System;
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
                if (Options.Opts.SweepOnly && __instance.storage.allowSettingOnlyFetchMarkedItems && IsValidForTarget(null, __instance.gameObject))
                    __instance.storage.SetOnlyFetchMarkedItems(true);
                    
            }

            // Some items are special cased so that their side screen does not allow setting OnlyFetch.
            [HarmonyReversePatch][HarmonyPatch(typeof(TreeFilterableSideScreen), nameof(TreeFilterableSideScreen.IsValidForTarget))]
            public static bool IsValidForTarget(TreeFilterableSideScreen _, GameObject target) => 
                throw new NotImplementedException($"Failed to reverse patch {nameof(IsValidForTarget)}.");
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
