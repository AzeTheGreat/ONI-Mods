using AzeLib.Extensions;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SliderControlSetting : LogicSettingDispComp
    {
        protected ISliderControl sliderControl;

        new public void Start() => sliderControl = gameObject.GetComponent<ISliderControl>();

        [SerializeField] private string prefix = string.Empty;

        private string GetUnits() => sliderControl.SliderUnits;

        public override string GetSetting() => prefix + sliderControl.GetSliderValue(0) + GetUnits();

        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;
            var component = go.AddComponent<SliderControlSetting>();

            if (go.GetReflectionComp("WirelessSignalReceiver"))
                component.prefix = "I: ";
            else if (go.GetReflectionComp("WirelessSignalEmitter"))
                component.prefix = "O: ";
        }
    }
}
