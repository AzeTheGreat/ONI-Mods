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
                {
                    float val = duration / 600f;
                    if(val >= 10f)
                        return GameUtil.FloatToString(val, "F1") + "c";
                    else
                        return GameUtil.FloatToString(val, "F2") + "c";
                }
                    
                return GameUtil.GetFormattedTime(duration, "F0");
            }
        }
    }
}
