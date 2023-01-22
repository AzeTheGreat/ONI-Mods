namespace BetterLogicOverlay.LogicSettingDisplay
{
    class FilterableSetting : LogicLabelSetting
    {
        [MyCmpGet] private Filterable filterable;

        public override string GetSetting() => filterable.SelectedTag.GetAbbreviation();
    }
}
