using AzeLib.Attributes;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicRibbonBitSelectorSetting : LogicSettingDispComp
    {
        [MyIntGet] protected ILogicRibbonBitSelector logicRibbonBitSelector;

        public override string GetSetting() => (logicRibbonBitSelector.GetBitSelection() + 1).ToString();

        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;
            go.AddComponent<LogicRibbonBitSelectorSetting>();
        }
    }
}
