﻿using AzeLib;
using HarmonyLib;
using System;
using System.Linq;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    class ApplySettingsToDefs
    {
        static void Postfix()
        {
            var componentMapper = new ComponentMapper<PortFilter>(new()
            {
                // Specific buildings
                (typeof(LogicCounter), typeof(LogicCounterSetting), PortFilter.Output),
                (typeof(LogicTimerSensor), typeof(LogicTimerSensorSetting), PortFilter.Output),
                (typeof(LogicTimeOfDaySensor), typeof(LogicTimeOfDaySensorSetting), PortFilter.Output),
                (typeof(LogicBroadcaster), typeof(LogicBroadcasterSetting), PortFilter.Input),
                (typeof(LogicBroadcastReceiver), typeof(LogicBroadcasterSetting.Receiver), PortFilter.Output),
                (typeof(CometDetector.Def), typeof(CometDetectorSetting), PortFilter.Output),
                (typeof(ClusterCometDetector.Def), typeof(ClusterCometDetectorSetting), PortFilter.Output),
                (typeof(LogicClusterLocationSensor), typeof(LogicClusterLocationSensorSetting), PortFilter.Output),
                (typeof(LimitValve), typeof(LimitValveSetting), PortFilter.Output),

                // IThresholdSwitch
                (typeof(ConduitTemperatureSensor), typeof(ThresholdSwitchSetting.ConduitTemp), PortFilter.Output),
                (typeof(LogicDiseaseSensor), typeof(ThresholdSwitchSetting.Germs), PortFilter.Output),
                (typeof(ConduitDiseaseSensor), typeof(ThresholdSwitchSetting.Germs), PortFilter.Output),
                (typeof(LogicCritterCountSensor), typeof(ThresholdSwitchSetting.CritterCount), PortFilter.Output),
                (typeof(LogicRadiationSensor), typeof(ThresholdSwitchSetting.RadiationSensor), PortFilter.Output),
                (typeof(LogicHEPSensor), typeof(ThresholdSwitchSetting.HEPSensor), PortFilter.Output),
                (typeof(IThresholdSwitch), typeof(ThresholdSwitchSetting), PortFilter.Output),

                // ISliderControl
                // Applies to many buildings that shouldn't have labels, so whitelisting is the easiest approach.
                (AccessTools.TypeByName("WirelessSignalReceiver"), typeof(SliderControlSetting.WirelessSignalRecieverSetting), PortFilter.Output),
                (AccessTools.TypeByName("WirelessSignalEmitter"), typeof(SliderControlSetting.WirelessSignalEmitterSetting), PortFilter.Input),
                (typeof(LogicGateFilter), typeof(SliderControlSetting), PortFilter.Output),
                (typeof(LogicGateBuffer), typeof(SliderControlSetting), PortFilter.Output),

                // TreeFilterable
                (typeof(SpiceGrinder.Def), null, PortFilter.Input),
                (typeof(TreeFilterable), typeof(TreeFilterableSetting), PortFilter.InputOutput),

                // General categories
                (typeof(Filterable), typeof(FilterableSetting), PortFilter.InputOutput),
                (typeof(IActivationRangeTarget), typeof(ActivationRangeTargetSetting), PortFilter.Output),
                (typeof(ILogicRibbonBitSelector), typeof(LogicRibbonBitSelectorSetting), PortFilter.Output),
                (typeof(IUserControlledCapacity), typeof(UserControlledCapacitySetting), PortFilter.Output)
            });

            foreach (var def in Assets.BuildingDefs)
            {
                var go = def.BuildingComplete;

                // If there are no logic ports: skip this def.
                // The LogicPorts fields are not reliable; some configs don't set them, but still yield a LogicPorts component.
                if (!go.TryGetComponent<LogicPorts>(out var logicPorts) & !go.TryGetComponent<LogicGateBase>(out var logicGateBase))
                    continue;

                var hasInput = (logicPorts?.inputPortInfo?.Any() ?? false) || (logicGateBase?.inputPortOffsets?.Any() ?? false);
                var hasOutput = (logicPorts?.outputPortInfo?.Any() ?? false) || (logicGateBase?.outputPortOffsets?.Any() ?? false);

                componentMapper.ApplyMap(go, ShouldAdd);

                bool ShouldAdd(PortFilter filter) => filter switch
                {
                    PortFilter.Input => hasInput,
                    PortFilter.Output => hasOutput,
                    PortFilter.InputOutput => hasInput || hasOutput,
                    _ => throw new ArgumentException("Invalid PortFilter Enum"),
                };
            }
        }

        private enum PortFilter
        {
            Input,
            Output,
            InputOutput
        }
    }
}
