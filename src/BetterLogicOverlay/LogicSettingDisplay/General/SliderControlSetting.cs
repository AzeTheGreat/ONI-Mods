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

        public override string GetSetting() => prefix + sliderControl.GetSliderValue(0) + sliderControl.SliderUnits;

        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;

            // Generators that ignore battery refill percent can't have it adjusted, so don't display it.
            if (go.GetComponent<EnergyGenerator>()?.ignoreBatteryRefillPercent ?? false)
                return;

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
