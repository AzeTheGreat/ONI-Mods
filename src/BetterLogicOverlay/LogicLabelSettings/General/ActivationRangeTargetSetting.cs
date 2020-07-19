using AzeLib.Attributes;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ActivationRangeTargetSetting : LogicLabelSetting
    {
        [MyIntGet] protected IActivationRangeTarget activationTarget;

        public override string GetSetting()
        {
            string unit = STRINGS.UI.UNITSUFFIXES.PERCENT;
            return activationTarget.DeactivateValue + unit + " - " + activationTarget.ActivateValue + unit;
        }
    }
}
