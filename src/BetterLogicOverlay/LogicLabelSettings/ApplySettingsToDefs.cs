using AzeLib.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    class ApplySettingsToDefs
    {
        static void Postfix()
        {
            foreach (var def in Assets.BuildingDefs)
            {
                var go = def.BuildingComplete;

                // If there are no logic ports: skip this def.
                // The LogicPorts fields are not reliable; some configs don't set them, but still yield a LogicPorts component.
                if (!go.TryGetComponent<LogicPorts>(out var logicPorts) & !go.TryGetComponent<LogicGateBase>(out var logicGateBase))
                    continue;

                var hasInput = (logicPorts?.inputPortInfo?.Any() ?? false) || (logicGateBase?.inputPortOffsets?.Any() ?? false);
                var hasOutput = (logicPorts?.outputPortInfo?.Any() ?? false) || (logicGateBase?.outputPortOffsets?.Any() ?? false);

                // Add the correct setting component for the building.
                foreach (var (building, setting, filter) in buildingToLabelMap)
                {
                    if (building != null && HasComponentOrDef(building, go))
                    {
                        var shouldAdd = filter switch
                        {
                            PortFilter.Input => hasInput,
                            PortFilter.Output => hasOutput,
                            PortFilter.InputOutput => hasInput || hasOutput,
                            _ => throw new ArgumentException("Invalid PortFilter Enum"),
                        };

                        if (shouldAdd)
                        {
                            if(setting != null)
                                go.AddComponent(setting);
                                
                            break;
                        }
                    }
                }

                bool HasComponentOrDef(Type building, GameObject go) => go.GetComponent(building) ?? go.GetDef(building) != null;
            }
        }

        // Maps a component on the def to the correct type of setting to add.
        // Processed first to last, so interface fallbacks should be after specific implementations.
        static readonly List<(Type building, Type setting, PortFilter filter)> buildingToLabelMap = new()
        {
            // Blocklist
            (typeof(SpiceGrinder.Def), null, PortFilter.Input),

            // IThresholdSwitch
            (typeof(ConduitTemperatureSensor), typeof(ThresholdSwitchSetting.ConduitTemp), PortFilter.Output),
            (typeof(LogicDiseaseSensor), typeof(ThresholdSwitchSetting.Germs), PortFilter.Output),
            (typeof(ConduitDiseaseSensor), typeof(ThresholdSwitchSetting.Germs), PortFilter.Output),
            (typeof(LogicCritterCountSensor), typeof(ThresholdSwitchSetting.CritterCount), PortFilter.Output),
            (typeof(IThresholdSwitch), typeof(ThresholdSwitchSetting), PortFilter.Output),

            // ISliderControl
            // Applies to many buildings that shouldn't have labels, so whitelisting is the easiest approach.
            (AccessTools.TypeByName("WirelessSignalReceiver"), typeof(SliderControlSetting.WirelessSignalRecieverSetting), PortFilter.Output),
            (AccessTools.TypeByName("WirelessSignalEmitter"), typeof(SliderControlSetting.WirelessSignalEmitterSetting), PortFilter.Input),
            (typeof(LogicGateFilter), typeof(SliderControlSetting), PortFilter.Output),
            (typeof(LogicGateBuffer), typeof(SliderControlSetting), PortFilter.Output),

            // Specific buildings
            (typeof(Filterable), typeof(ConduitElementSensorSetting), PortFilter.InputOutput),
            (typeof(LogicElementSensor), typeof(LogicElementSensorSetting), PortFilter.Output),
            (typeof(LogicCounter), typeof(LogicCounterSetting), PortFilter.Output),
            (typeof(LogicTimerSensor), typeof(LogicTimerSensorSetting), PortFilter.Output),
            (typeof(LogicTimeOfDaySensor), typeof(LogicTimeOfDaySensorSetting), PortFilter.Output),
            (typeof(LogicBroadcaster), typeof(LogicBroadcasterSetting), PortFilter.Input),
            (typeof(LogicBroadcastReceiver), typeof(LogicBroadcasterSetting.Receiver), PortFilter.Output),
            (typeof(CometDetector.Def), typeof(CometDetectorSetting), PortFilter.Output),
            (typeof(ClusterCometDetector.Def), typeof(ClusterCometDetectorSetting), PortFilter.Output),

            // Specific mod buildings
            (typeof(TreeFilterable), typeof(SolidElementSetting), PortFilter.InputOutput),

            // General categories
            (typeof(IActivationRangeTarget), typeof(ActivationRangeTargetSetting), PortFilter.Output),
            (typeof(ILogicRibbonBitSelector), typeof(LogicRibbonBitSelectorSetting), PortFilter.Output),
            (typeof(IUserControlledCapacity), typeof(UserControlledCapacitySetting), PortFilter.Output)
        };

        private enum PortFilter
        {
            Input,
            Output,
            InputOutput
        }
    }
}
