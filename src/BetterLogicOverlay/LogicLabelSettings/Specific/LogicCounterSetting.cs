namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicCounterSetting : LogicLabelSetting
    {
        [MyCmpGet] private LogicCounter logicCounter;

        private string GetAdvancedMode() => logicCounter.advancedMode ? "% " : "";
        public override string GetSetting() => GetAdvancedMode() + logicCounter.maxCount.ToString();
    }
}
