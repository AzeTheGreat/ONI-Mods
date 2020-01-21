using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SliderControlSetting : LogicSettingDispComp
    {
        [MyCmpGet]
        protected ISliderControl sliderControl;

        public override string GetSetting() => sliderControl.GetSliderValue(0) + sliderControl.SliderUnits;

        [HarmonyPatch]
        private class Add
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(LogicGateBufferConfig), nameof(LogicGateBufferConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LogicGateFilterConfig), nameof(LogicGateFilterConfig.DoPostConfigureComplete));
            }

            public static void Postfix(GameObject go) => go.AddComponent<SliderControlSetting>();
        }
    }
}
