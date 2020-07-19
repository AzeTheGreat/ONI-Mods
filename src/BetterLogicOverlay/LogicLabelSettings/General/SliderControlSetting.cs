using AzeLib.Attributes;
using AzeLib.Extensions;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SliderControlSetting : LogicLabelSetting
    {
        [MyCmpGet] protected LogicPorts logicPorts;
        [MyIntGet] protected ISliderControl sliderControl;

        public override string GetSetting() => sliderControl.GetSliderValue(0) + sliderControl.SliderUnits;

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
