using AzeLib;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ThresholdSwitchSetting : LogicSettingDispComp
    {
        protected IThresholdSwitch thresholdSwitch;

        new private void Start()
        {
            base.Start();
            thresholdSwitch = gameObject.GetComponent<IThresholdSwitch>();
        }

        protected string GetAboveOrBelow() => thresholdSwitch.ActivateAboveThreshold ? ">" : "<";

        public override string GetSetting() => GetAboveOrBelow() + thresholdSwitch.Format(thresholdSwitch.Threshold, false) + thresholdSwitch.ThresholdValueUnits();

        [HarmonyPatch]
        private class Add : PostLoad
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(FloorSwitchConfig), nameof(FloorSwitchConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LogicPressureSensorGasConfig), nameof(LogicPressureSensorGasConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LogicPressureSensorLiquidConfig), nameof(LogicPressureSensorLiquidConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LogicTemperatureSensorConfig), nameof(LogicTemperatureSensorConfig.DoPostConfigureComplete));
            }

            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                // MOD: Thermo Sensor Tile
                var thermoSensorTile = AccessTools.Method("TileTempSensorConfig:DoPostConfigureComplete");
                if (thermoSensorTile != null) yield return thermoSensorTile;
            }

            public static void Postfix(GameObject go) => go.AddComponent<ThresholdSwitchSetting>();
        }
    }
}
