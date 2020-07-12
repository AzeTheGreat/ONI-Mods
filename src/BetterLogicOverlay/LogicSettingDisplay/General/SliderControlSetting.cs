using AzeLib.Attributes;
using AzeLib.Extensions;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SliderControlSetting : LogicSettingDispComp
    {
        [MyCmpGet] protected LogicPorts logicPorts;
        [MyIntGet] protected ISliderControl sliderControl;

        [SerializeField] private string prefix = string.Empty;

        public override string GetSetting() => prefix + sliderControl.GetSliderValue(0) + sliderControl.SliderUnits;

        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;

            // Generator settings aren't relevant to automation.
            // If there are no logic ports (medical cot), don't add.
            if (go.GetComponent<Generator>() || !go.GetComponent<LogicPorts>())
                return;

            var component = go.AddComponent<SliderControlSetting>();

            if (go.GetReflectionComp("WirelessSignalReceiver"))
                component.prefix = "I: ";
            else if (go.GetReflectionComp("WirelessSignalEmitter"))
                component.prefix = "O: ";
        }
    }
}
