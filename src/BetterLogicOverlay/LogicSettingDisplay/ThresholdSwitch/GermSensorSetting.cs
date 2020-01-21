using Harmony;
using STRINGS;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    // Separate from ThresholSwitchSetting due to spacing before "germs"
    class GermSensorSetting : ThresholdSwitchSetting
    {
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

        [HarmonyPatch]
        private class Add
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(GasConduitDiseaseSensorConfig), nameof(GasConduitDiseaseSensorConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LiquidConduitDiseaseSensorConfig), nameof(LiquidConduitDiseaseSensorConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LogicDiseaseSensorConfig), nameof(LogicDiseaseSensorConfig.DoPostConfigureComplete));
            }

            public static void Postfix(GameObject go) => go.AddComponent<GermSensorSetting>();
        }
    }
}
