namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ConduitElementSensorSetting : LogicLabelSetting
    {
        [MyCmpGet] private Filterable filterable;

        public override string GetSetting() => filterable.SelectedTag.GetAbbreviation();
    }
}
