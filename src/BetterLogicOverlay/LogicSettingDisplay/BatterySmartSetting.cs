using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class BatterySmartSetting : LogicSettingDispComp
    {
        [MyCmpGet]
        private BatterySmart batterySmart;
        [MyCmpGet]
        private LogicPorts logicPorts;

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
