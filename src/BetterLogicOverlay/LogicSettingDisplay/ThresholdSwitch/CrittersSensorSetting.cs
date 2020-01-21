using AzeLib;
using Harmony;
using STRINGS;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.RanchingSensors
{
    // MOD: Ranching Sensors
    class CrittersSensorSetting : ThresholdSwitchSetting
    {
        public override string GetSetting() => GetAboveOrBelow() + thresholdSwitch.Format(thresholdSwitch.Threshold, false) + " " + UI.UNITSUFFIXES.CRITTERS;

        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var crittersSensor = AccessTools.Method("CrittersSensorConfig:DoPostConfigureComplete");
                if (crittersSensor != null) yield return crittersSensor;
            }

            static void Postfix(GameObject go) => go.AddComponent<CrittersSensorSetting>();
        }
    }
}
