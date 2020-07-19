namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicTimeOfDaySensorSetting : LogicLabelSetting
    {
        [MyCmpGet] private LogicTimeOfDaySensor logicTimeOfDaySensor;

        public override string GetSetting() => GameUtil.GetFormattedPercent(logicTimeOfDaySensor.startTime * 100f) + " : " + GameUtil.GetFormattedPercent(logicTimeOfDaySensor.duration * 100f);
    }
}
