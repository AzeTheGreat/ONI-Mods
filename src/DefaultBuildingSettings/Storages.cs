using Harmony;
using UnityEngine;

namespace DefaultBuildingSettings
{
    static class Storages
    {
        [HarmonyPatch(typeof(TreeFilterable), "OnPrefabInit")]
        private class SweepOnly_Patch
        {
            static void Postfix(Storage ___storage)
            {
                if (Options.Opts.SweepOnly && ___storage.allowSettingOnlyFetchMarkedItems)
                    ___storage.SetOnlyFetchMarkedItems(true);
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
