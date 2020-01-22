using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using AzeLib.Extensions;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SliderControlSetting : LogicSettingDispComp
    {
        [MyCmpGet] protected ISliderControl sliderControl;
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
