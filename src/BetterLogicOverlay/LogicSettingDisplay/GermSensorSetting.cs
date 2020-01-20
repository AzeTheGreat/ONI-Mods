using Harmony;
using UnityEngine;
using STRINGS;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    // Separate from ThresholSwitchSetting due to spacing before "germs"
    class GermSensorSetting : LogicSettingDispComp
    {
        private IThresholdSwitch thresholdSwitch;

        new private void Start()
        {
            base.Start();
            thresholdSwitch = gameObject.GetComponent<IThresholdSwitch>();
        }

        public override string GetSetting()
        {
            string aboveOrBelow = thresholdSwitch.ActivateAboveThreshold ? ">" : "<";

            float threshold = thresholdSwitch.Threshold;
            string modifier = string.Empty;

            if (threshold > 10000f)
            {
                threshold /= 1000f;
                modifier = ASTRINGS.UNITMODIFIERS.thousand;
            }

            threshold = Mathf.Round(threshold);
                
            return aboveOrBelow + threshold + modifier + " " +  UI.UNITSUFFIXES.DISEASE.UNITS;
        }

        [HarmonyPatch(typeof(GasConduitDiseaseSensorConfig), nameof(GasConduitDiseaseSensorConfig.DoPostConfigureComplete))]
        private class AddToGasDisease { static void Postfix(GameObject go) => go.AddComponent<GermSensorSetting>(); }

        [HarmonyPatch(typeof(LiquidConduitDiseaseSensorConfig), nameof(LiquidConduitDiseaseSensorConfig.DoPostConfigureComplete))]
        private class AddToLiquidDisease { static void Postfix(GameObject go) => go.AddComponent<GermSensorSetting>(); }

        [HarmonyPatch(typeof(LogicDiseaseSensorConfig), nameof(LogicDiseaseSensorConfig.DoPostConfigureComplete))]
        private class AddToDisease { static void Postfix(GameObject go) => go.AddComponent<GermSensorSetting>(); }
    }
}
