using AzeLib.Attributes;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicRibbonBitSelectorSetting : LogicLabelSetting
    {
        [MyIntGet] protected ILogicRibbonBitSelector logicRibbonBitSelector;

        public override string GetSetting() => (logicRibbonBitSelector.GetBitSelection() + 1).ToString();
    }
}
