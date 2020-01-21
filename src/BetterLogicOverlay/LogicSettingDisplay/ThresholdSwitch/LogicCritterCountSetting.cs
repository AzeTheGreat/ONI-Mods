using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.ThresholdSwitch
{
    class LogicCritterCountSetting : ThresholdSwitchSetting
    {
        public override string GetSetting() => GetAboveOrBelow() + thresholdSwitch.Format(thresholdSwitch.Threshold, false) + " C/E";

        [HarmonyPatch(typeof(LogicCritterCountSensorConfig), nameof(LogicCritterCountSensorConfig.DoPostConfigureComplete))]
        private class Add
        {
            static void Postfix(GameObject go) => go.AddComponent<LogicCritterCountSetting>();
        }
    }
}
