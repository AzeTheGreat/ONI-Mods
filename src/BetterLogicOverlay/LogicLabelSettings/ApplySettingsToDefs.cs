using AzeLib.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
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
                if (!go.GetComponent<LogicPorts>() && !go.GetComponent<LogicGateBase>())
                    continue;

                // Add the correct setting component for the building.
                foreach (var (building, setting) in buildingToLabelMap)
                {
                    if (building != null && HasComponentOrDef(building, go))
                    {
                        go.AddComponent(setting);
                        break;
                    }
                }

                bool HasComponentOrDef(Type building, GameObject go) => go.GetComponent(building) ?? go.GetDef(building) != null;
            }
        }

        // Maps a component on the def to the correct type of setting to add.
        // Processed first to last, so interface fallbacks should be after specific implementations.
        static readonly List<(Type building, Type setting)> buildingToLabelMap = new()
        {
            // IThresholdSwitch
            (typeof(ConduitTemperatureSensor), typeof(ThresholdSwitchSetting.ConduitTemp)),
            (typeof(LogicDiseaseSensor), typeof(ThresholdSwitchSetting.Germs)),
            (typeof(ConduitDiseaseSensor), typeof(ThresholdSwitchSetting.Germs)),
            (typeof(LogicCritterCountSensor), typeof(ThresholdSwitchSetting.CritterCount)),
            (typeof(IThresholdSwitch), typeof(ThresholdSwitchSetting)),

            // ISliderControl
            // Applies to many buildings that shouldn't have labels, so whitelisting is the easiest approach.
            (AccessTools.TypeByName("WirelessSignalReceiver"), typeof(SliderControlSetting.WirelessSignalRecieverSetting)),
            (AccessTools.TypeByName("WirelessSignalEmitter"), typeof(SliderControlSetting.WirelessSignalEmitterSetting)),
            (typeof(LogicGateFilter), typeof(SliderControlSetting)),
            (typeof(LogicGateBuffer), typeof(SliderControlSetting)),

            // General categories
            (typeof(IActivationRangeTarget), typeof(ActivationRangeTargetSetting)),
            (typeof(ILogicRibbonBitSelector), typeof(LogicRibbonBitSelectorSetting)),
            (typeof(IUserControlledCapacity), typeof(UserControlledCapacitySetting)),

            // Specific buildings
            (typeof(Filterable), typeof(ConduitElementSensorSetting)),
            (typeof(LogicElementSensor), typeof(LogicElementSensorSetting)),
            (typeof(LogicCounter), typeof(LogicCounterSetting)),
            (typeof(LogicTimerSensor), typeof(LogicTimerSensorSetting)),
            (typeof(LogicTimeOfDaySensor), typeof(LogicTimeOfDaySensorSetting)),
            (typeof(LogicBroadcaster), typeof(LogicBroadcasterSetting)),
            (typeof(LogicBroadcastReceiver), typeof(LogicBroadcasterSetting.Receiver)),
            (typeof(CometDetector.Def), typeof(CometDetectorSetting)),
            (typeof(ClusterCometDetector.Def), typeof(ClusterCometDetectorSetting)),

            // Specific mod buildings
            (typeof(TreeFilterable), typeof(SolidElementSetting))
        };

    }
}
