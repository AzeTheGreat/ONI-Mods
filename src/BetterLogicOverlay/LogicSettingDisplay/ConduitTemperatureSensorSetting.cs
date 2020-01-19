using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    // Separate from ThresholdSwitchSetting due to the game not rounding temperature strings on conduits
    class ConduitTemperatureSensorSetting : KMonoBehaviour, ILogicSettingDisplay
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
            return aboveOrBelow + GameUtil.GetFormattedTemperature(thresholdSwitch.Threshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true);
        }

        [HarmonyPatch(typeof(GasConduitTemperatureSensorConfig), nameof(GasConduitTemperatureSensorConfig.DoPostConfigureComplete))]
        private class AddToGasTemp { static void Postfix(GameObject go) => go.AddComponent<ConduitTemperatureSensorSetting>(); }

        [HarmonyPatch(typeof(LiquidConduitTemperatureSensorConfig), nameof(LiquidConduitTemperatureSensorConfig.DoPostConfigureComplete))]
        private class AddToLiquidTemp { static void Postfix(GameObject go) => go.AddComponent<ConduitTemperatureSensorSetting>(); }
    }
}
