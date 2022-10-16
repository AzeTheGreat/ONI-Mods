namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicBroadcasterSetting : LogicLabelSetting
    {
        public override string GetSetting() => GetString(this);

        protected string GetString(KMonoBehaviour l) => l.GetProperName();

        public class Receiver : LogicBroadcasterSetting
        {
            [MyCmpGet] private LogicBroadcastReceiver logicBroadcastReceiver;

            public override string GetSetting() => GetString(logicBroadcastReceiver.GetChannel());
        }
    }
}
