using Harmony;
using System.Collections.Generic;
using System.Reflection;
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

        [HarmonyPatch]
        private class Add
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(LogicCritterCountSensorConfig), nameof(LogicCritterCountSensorConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(FloorSwitchConfig), nameof(FloorSwitchConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LogicPressureSensorGasConfig), nameof(LogicPressureSensorGasConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LogicPressureSensorLiquidConfig), nameof(LogicPressureSensorLiquidConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LogicTemperatureSensorConfig), nameof(LogicTemperatureSensorConfig.DoPostConfigureComplete));
            }
            static void Postfix(GameObject go) => go.AddComponent<ThresholdSwitchSetting>();
        }
    }
}
