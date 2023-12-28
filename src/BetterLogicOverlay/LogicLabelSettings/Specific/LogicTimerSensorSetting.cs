namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicTimerSensorSetting : LogicLabelSetting
    {
        [MyCmpGet] private LogicTimerSensor logicTimerSensor;

        public override string GetSetting()
        {
            return GetFormattedDuration(logicTimerSensor.onDuration) + " / " + GetFormattedDuration(logicTimerSensor.offDuration);

            string GetFormattedDuration(float duration)
            {
                if (duration > 600f)
                    return LabelUtil.GetFormattedNum(duration / 600f) + "c";
                return GameUtil.GetFormattedTime(duration, LabelUtil.GetFormatForNum(duration));
            }
        }
    }
}
