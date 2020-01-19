using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SliderControlSetting : KMonoBehaviour, ILogicSettingDisplay
    {
        private ISliderControl sliderControl;

        new private void Start()
        {
            base.Start();
            sliderControl = gameObject.GetComponent<ISliderControl>();
        }

        public string GetSetting()
        {
            return sliderControl.GetSliderValue(0) + sliderControl.SliderUnits;
        }

        [HarmonyPatch(typeof(LogicGateBufferConfig), nameof(LogicGateBufferConfig.DoPostConfigureComplete))]
        private class AddToBuffer { static void Postfix(GameObject go) => go.AddComponent<SliderControlSetting>(); }

        [HarmonyPatch(typeof(LogicGateFilterConfig), nameof(LogicGateFilterConfig.DoPostConfigureComplete))]
        private class AddToFilter { static void Postfix(GameObject go) => go.AddComponent<SliderControlSetting>(); }
    }
}
