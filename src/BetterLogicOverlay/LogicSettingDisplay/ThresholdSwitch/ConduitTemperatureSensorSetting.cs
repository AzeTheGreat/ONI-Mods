using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    // Separate from ThresholdSwitchSetting due to the game not rounding temperature strings on conduits
    class ConduitTemperatureSensorSetting : ThresholdSwitchSetting
    {
        public override string GetSetting()
        {
            return GetAboveOrBelow() + GameUtil.GetFormattedTemperature(thresholdSwitch.Threshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true);
        }

        [HarmonyPatch]
        private class Add
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(GasConduitTemperatureSensorConfig), nameof(GasConduitTemperatureSensorConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LiquidConduitTemperatureSensorConfig), nameof(LiquidConduitTemperatureSensorConfig.DoPostConfigureComplete));
            }

            static void Postfix(GameObject go) => go.AddComponent<ConduitTemperatureSensorSetting>();
        }
    }
}
