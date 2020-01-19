using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ThresholdSwitchSetting : KMonoBehaviour, ILogicSettingDisplay
    {
        private IThresholdSwitch thresholdSwitch;

        new private void Start()
        {
            base.Start();
            thresholdSwitch = gameObject.GetComponent<IThresholdSwitch>();
        }

        public string GetSetting()
        {
            string aboveOrBelow = thresholdSwitch.ActivateAboveThreshold ? ">" : "<";
            return aboveOrBelow + thresholdSwitch.Format(thresholdSwitch.Threshold, false) + thresholdSwitch.ThresholdValueUnits();
        }

        [HarmonyPatch(typeof(LogicCritterCountSensorConfig), nameof(LogicCritterCountSensorConfig.DoPostConfigureComplete))]
        private class AddToCritterCount { static void Postfix(GameObject go) => go.AddComponent<ThresholdSwitchSetting>(); }

        [HarmonyPatch(typeof(FloorSwitchConfig), nameof(FloorSwitchConfig.DoPostConfigureComplete))]
        private class AddToFloorSwitch { static void Postfix(GameObject go) => go.AddComponent<ThresholdSwitchSetting>(); }

        [HarmonyPatch(typeof(LogicPressureSensorGasConfig), nameof(LogicPressureSensorGasConfig.DoPostConfigureComplete))]
        private class AddToGasPressure { static void Postfix(GameObject go) => go.AddComponent<ThresholdSwitchSetting>(); }

        [HarmonyPatch(typeof(LogicPressureSensorLiquidConfig), nameof(LogicPressureSensorLiquidConfig.DoPostConfigureComplete))]
        private class AddToLiquidPressure { static void Postfix(GameObject go) => go.AddComponent<ThresholdSwitchSetting>(); }

        [HarmonyPatch(typeof(LogicTemperatureSensorConfig), nameof(LogicTemperatureSensorConfig.DoPostConfigureComplete))]
        private class AddToTemp { static void Postfix(GameObject go) => go.AddComponent<ThresholdSwitchSetting>(); }
    }
}
