using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class BatterySmartSetting : KMonoBehaviour, ILogicSettingDisplay
    {
        private BatterySmart batterySmart;

        new private void Start()
        {
            base.Start();
            batterySmart = gameObject.GetComponent<BatterySmart>();
        }

        public string GetSetting()
        {
            string unit = STRINGS.UI.UNITSUFFIXES.PERCENT;
            return batterySmart.DeactivateValue + unit + " - " + batterySmart.ActivateValue + unit;
        }

        [HarmonyPatch(typeof(BatterySmartConfig), nameof(BatterySmartConfig.DoPostConfigureComplete))]
        private class AddToBattery { static void Postfix(GameObject go) => go.AddComponent<BatterySmartSetting>(); }
    }
}
