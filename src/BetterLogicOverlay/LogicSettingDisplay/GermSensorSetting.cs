using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    // Separate from ThresholSwitchSetting due to spacing before "germs"
    class GermSensorSetting : KMonoBehaviour, ILogicSettingDisplay
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
            return aboveOrBelow + thresholdSwitch.Format(thresholdSwitch.Threshold, false) + " " +  STRINGS.UI.UNITSUFFIXES.DISEASE.UNITS;
        }

        [HarmonyPatch(typeof(GasConduitDiseaseSensorConfig), nameof(GasConduitDiseaseSensorConfig.DoPostConfigureComplete))]
        private class AddToGasDisease { static void Postfix(GameObject go) => go.AddComponent<GermSensorSetting>(); }

        [HarmonyPatch(typeof(LiquidConduitDiseaseSensorConfig), nameof(LiquidConduitDiseaseSensorConfig.DoPostConfigureComplete))]
        private class AddToLiquidDisease { static void Postfix(GameObject go) => go.AddComponent<GermSensorSetting>(); }

        [HarmonyPatch(typeof(LogicDiseaseSensorConfig), nameof(LogicDiseaseSensorConfig.DoPostConfigureComplete))]
        private class AddToDisease { static void Postfix(GameObject go) => go.AddComponent<GermSensorSetting>(); }
    }
}
