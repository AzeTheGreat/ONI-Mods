using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class BatterySmartSetting : LogicSettingDispComp
    {
        private BatterySmart batterySmart;
        private LogicPorts logicPorts;

        new private void Start()
        {
            base.Start();
            batterySmart = gameObject.GetComponent<BatterySmart>();
            logicPorts = gameObject.GetComponent<LogicPorts>();
        }

        public override string GetSetting()
        {
            string unit = STRINGS.UI.UNITSUFFIXES.PERCENT;
            return batterySmart.DeactivateValue + unit + " - " + batterySmart.ActivateValue + unit;
        }

        public override Vector2 GetPosition()
        {
            return Grid.CellToPosCCC(logicPorts.outputPorts[0].GetLogicUICell(), Grid.SceneLayer.Front);
        }

        public override Vector2 GetSizeDelta()
        {
            return Vector2.one;
        }

        [HarmonyPatch(typeof(BatterySmartConfig), nameof(BatterySmartConfig.DoPostConfigureComplete))]
        private class AddToBattery { static void Postfix(GameObject go) => go.AddComponent<BatterySmartSetting>(); }
    }
}
