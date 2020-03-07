using Harmony;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    class ApplyGeneralChanges
    {
        static void Postfix()
        {
            foreach (var def in Assets.BuildingDefs)
            {
                var go = def.BuildingComplete;

                if (go.GetComponent<IThresholdSwitch>() != null)
                    ThresholdSwitchSetting.AddToDef(def);
                else if (go.GetComponent<ISliderControl>() != null)
                    SliderControlSetting.AddToDef(def);
                else if (go.GetComponent<IActivationRangeTarget>() != null)
                    ActivationRangeTargetSetting.AddToDef(def);
                else if (go.GetComponent<ILogicRibbonBitSelector>() != null)
                    LogicRibbonBitSelectorSetting.AddToDef(def);
            }
        }
    }
}
