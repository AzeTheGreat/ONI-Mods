using HarmonyLib;

namespace DefaultBuildingSettings
{
    [HarmonyPatch(typeof(BatterySmart), "OnPrefabInit")]
    class BatterySmartRange_Patch
    {
        static void Postfix(ref int ___activateValue, ref int ___deactivateValue)
        {
            ___activateValue = Options.Opts.SmartBatteryActivateValue;
            ___deactivateValue = Options.Opts.SmartBatteryDeactivateValue;
        }
    }
}
