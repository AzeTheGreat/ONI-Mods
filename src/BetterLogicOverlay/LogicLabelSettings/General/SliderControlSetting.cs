using AzeLib.Attributes;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SliderControlSetting : LogicLabelSetting
    {
        [MyIntGet] protected ISliderControl sliderControl;

        public override string GetSetting() => LabelUtil.GetFormattedNum(sliderControl.GetSliderValue(0)) + sliderControl.SliderUnits;

        public class WirelessSignalRecieverSetting : SliderControlSetting
        {
            public override string GetSetting() => "I: " + base.GetSetting();
        }

        public class WirelessSignalEmitterSetting : SliderControlSetting
        {
            public override string GetSetting() => "O: " + base.GetSetting();
        }
    }
}
