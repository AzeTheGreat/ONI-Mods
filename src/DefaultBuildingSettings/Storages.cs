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

            var trav = Traverse.Create(smartReservoir);
            trav.Field("activateValue").SetValue(Options.Opts.ReservoirActivateValue);
            trav.Field("deactivateValue").SetValue(Options.Opts.ReservoirDeactivateValue);
            return true;
        }
    }
}
