using AzeLib.Attributes;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ActivationRangeTargetSetting : LogicSettingDispComp
    {
        [MyIntGet] protected IActivationRangeTarget activationTarget;

        public override string GetSetting()
        {
            string unit = STRINGS.UI.UNITSUFFIXES.PERCENT;
            return activationTarget.DeactivateValue + unit + " - " + activationTarget.ActivateValue + unit;
        }

        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;
            go.AddComponent<ActivationRangeTargetSetting>();
        }
    }
}
