using AzeLib;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.ModCompatability
{
    // MOD: Thermo Sensor Tile
    class ThermoSensorTile : ThresholdSwitchSetting
    {
        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var thermoSensorTile = AccessTools.Method("TileTempSensorConfig:DoPostConfigureComplete");
                if (thermoSensorTile != null) yield return thermoSensorTile;
            }

            public static void Postfix(GameObject go) => go.AddComponent<ThresholdSwitchSetting>();
        }
    }
}
