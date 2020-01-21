using AzeLib;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.RanchingSensors
{
    // MOD: Ranching Sensors
    class EggsSensorSetting : ThresholdSwitchSetting
    {
        public override string GetSetting() => GetAboveOrBelow() + thresholdSwitch.Format(thresholdSwitch.Threshold, false) + " eggs";

        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var eggsSensor = AccessTools.Method("EggsSensorConfig:DoPostConfigureComplete");
                if (eggsSensor != null) yield return eggsSensor;
            }

            static void Postfix(GameObject go) => go.AddComponent<EggsSensorSetting>();
        }
    }
}
