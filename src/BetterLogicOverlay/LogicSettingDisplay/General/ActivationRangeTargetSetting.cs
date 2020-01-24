namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ActivationRangeTargetSetting : LogicSettingDispComp
    {
        protected IActivationRangeTarget activationTarget;

        new public void Start() => activationTarget = gameObject.GetComponent<IActivationRangeTarget>();

        public override string GetSetting()
        {
            string unit = STRINGS.UI.UNITSUFFIXES.PERCENT;
            return activationTarget.DeactivateValue + unit + " - " + activationTarget.ActivateValue + unit;
        }

        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;
            var component = go.AddComponent<ActivationRangeTargetSetting>();
        }
    }
}
