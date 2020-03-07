using AzeLib.Attributes;
using AzeLib.Extensions;
using System.Linq;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SliderControlSetting : LogicSettingDispComp
    {
        [MyCmpGet] protected LogicPorts logicPorts;
        [MyIntGet] protected ISliderControl sliderControl;

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

        public override Vector2 GetPosition()
        {
            if (logicPorts)
                return Grid.CellToPosCCC(logicPorts.GetLogicCells().First(), Grid.SceneLayer.Front);
            else
                return base.GetPosition();
        }
    }
}
