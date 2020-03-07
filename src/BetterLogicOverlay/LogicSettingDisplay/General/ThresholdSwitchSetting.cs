using AzeLib.Attributes;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ThresholdSwitchSetting : LogicSettingDispComp
    {
        [MyIntGet] protected IThresholdSwitch thresholdSwitch;

        [SerializeField] private string units = string.Empty;

        protected virtual string GetValue() => thresholdSwitch.Format(thresholdSwitch.Threshold, false);
        protected virtual string GetUnits() => units == string.Empty ? (string)thresholdSwitch.ThresholdValueUnits() : units;
        private string GetAboveOrBelow() => thresholdSwitch.ActivateAboveThreshold ? ">" : "<";

        public override string GetSetting() => GetAboveOrBelow() + GetValue() + GetUnits();
            
        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;

            if (go.GetComponent<ConduitTemperatureSensor>())
                go.AddComponent<ConduitTemp>();
            else if (go.GetComponent<LogicDiseaseSensor>() || go.GetComponent<ConduitDiseaseSensor>())
                go.AddComponent<Germs>();
            else if (go.GetComponent<LogicCritterCountSensor>())
                go.AddComponent<CritterCount>();
            else
                go.AddComponent<ThresholdSwitchSetting>();
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

        private class CritterCount : ThresholdSwitchSetting
        {
            [MyCmpGet] private LogicCritterCountSensor logicCritterCountSensor;

            protected override string GetUnits()
            {
                if (logicCritterCountSensor.countCritters && logicCritterCountSensor.countEggs)
                    return " c/e";
                else if (logicCritterCountSensor.countCritters)
                    return " c";
                else if (logicCritterCountSensor.countEggs)
                    return " e";
                else
                    return string.Empty;
            }
        }
    }
}
