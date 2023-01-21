namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LimitValveSetting : LogicLabelSetting
    {
        [MyCmpGet] private LimitValve limitValve;

        public override string GetSetting() => limitValve.Limit.ToString();
    }
}
