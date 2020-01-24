using AzeLib.Extensions;
using STRINGS;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ThresholdSwitchSetting : LogicSettingDispComp
    {
        protected IThresholdSwitch thresholdSwitch;

        new public void Start() => thresholdSwitch = gameObject.GetComponent<IThresholdSwitch>();

        [SerializeField] private string units = string.Empty;

        protected virtual string GetValue() => thresholdSwitch.Format(thresholdSwitch.Threshold, false);
        protected virtual string GetUnits() => units == string.Empty ? (string)thresholdSwitch.ThresholdValueUnits() : units;
        private string GetAboveOrBelow() => thresholdSwitch.ActivateAboveThreshold ? ">" : "<";

        public override string GetSetting() => GetAboveOrBelow() + GetValue() + GetUnits();
            
        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;

            ThresholdSwitchSetting component = null;
            if (go.GetComponent<ConduitTemperatureSensor>())
                go.AddComponent<ConduitTemp>();
            else if (go.GetComponent<LogicDiseaseSensor>() || go.GetComponent<ConduitDiseaseSensor>())
                go.AddComponent<Germs>();
            else
                component = go.AddComponent<ThresholdSwitchSetting>();

            if (!component)
                return;

            if (go.GetReflectionComp("CrittersSensor") != null)
                component.units = " " + UI.UNITSUFFIXES.CRITTERS;
            else if (go.GetReflectionComp("EggsSensor") != null)
                component.units = " eggs";
            else if (go.GetComponent<LogicCritterCountSensor>())
                component.units = " C/E";
        }

        private class ConduitTemp : ThresholdSwitchSetting
        {
            protected override string GetUnits() => string.Empty;
            protected override string GetValue() => GameUtil.GetFormattedTemperature(thresholdSwitch.Threshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true);
        }

        private class Germs : ThresholdSwitchSetting
        {
            protected override string GetUnits() => " " + thresholdSwitch.ThresholdValueUnits();
            protected override string GetValue()
            {
                float threshold = thresholdSwitch.Threshold;
                string modifier = string.Empty;

                if (threshold > 10000f)
                {
                    threshold /= 1000f;
                    modifier = ASTRINGS.UNITMODIFIERS.thousand;
                }

                return Mathf.Round(threshold) + modifier;
            }
        }
    }
}
