using AzeLib.Attributes;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ThresholdSwitchSetting : LogicLabelSetting
    {
        [MyIntGet] protected IThresholdSwitch thresholdSwitch;

        protected virtual string GetValue() => thresholdSwitch.Format(thresholdSwitch.Threshold, false);
        protected virtual string GetUnits() => thresholdSwitch.ThresholdValueUnits();
        private string GetAboveOrBelow() => thresholdSwitch.ActivateAboveThreshold ? ">" : "<";

        public override string GetSetting() => GetAboveOrBelow() + GetValue() + GetUnits();

        public class ConduitTemp : ThresholdSwitchSetting
        {
            protected override string GetUnits() => string.Empty;
            protected override string GetValue() => GameUtil.AddTemperatureUnitSuffix(LabelUtil.GetFormattedNum(GameUtil.GetConvertedTemperature(thresholdSwitch.Threshold, false)));
        }

        public class Germs : ThresholdSwitchSetting
        {
            protected override string GetUnits() => " " + thresholdSwitch.ThresholdValueUnits();
            protected override string GetValue()
            {
                float threshold = thresholdSwitch.Threshold;
                string modifier = string.Empty;

                if (threshold > 10000f)
                {
                    threshold /= 1000f;
                    modifier = MYSTRINGS.UNITMODIFIERS.thousand;
                }

                return Mathf.Round(threshold) + modifier;
            }
        }

        public class CritterCount : ThresholdSwitchSetting
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
