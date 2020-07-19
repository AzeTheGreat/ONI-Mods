using AzeLib.Extensions;

namespace BetterLogicOverlay.LogicSettingDisplay.Specific
{
    class LogicAlarmSetting : LogicLabelSetting
    {
        [MyCmpGet] LogicAlarm logicAlarm;

        public override string GetSetting() => logicAlarm.notificationName.Truncate(5);
    }
}
