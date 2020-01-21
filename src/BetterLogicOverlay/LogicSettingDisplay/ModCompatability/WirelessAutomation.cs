using AzeLib;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.SliderControl
{
    // MOD: Wireless Automation
    class WirelessSignalEmitterSetting : SliderControlSetting
    {
        public override string GetSetting() => "O: " + sliderControl.GetSliderValue(0);

        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var wirelessEmitter = AccessTools.Method("WirelessSignalEmitterConfig:DoPostConfigureComplete");
                if (wirelessEmitter != null) yield return wirelessEmitter;
            }

            public static void Postfix(GameObject go) => go.AddComponent<WirelessSignalEmitterSetting>();
        }
    }

    class WirelessSignalReceiverSetting : SliderControlSetting
    {
        public override string GetSetting() => "I: " + sliderControl.GetSliderValue(0);

        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var wirelessReciever = AccessTools.Method("WirelessSignalReceiverConfig:DoPostConfigureComplete");
                if (wirelessReciever != null) yield return wirelessReciever;
            }

            public static void Postfix(GameObject go) => go.AddComponent<WirelessSignalReceiverSetting>();
        }
    }
}
