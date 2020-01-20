using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicTimeOfDaySensorSetting : LogicSettingDispComp
    {
        private LogicTimeOfDaySensor logicTimeOfDaySensor;

        new private void Start()
        {
            base.Start();
            logicTimeOfDaySensor = gameObject.GetComponent<LogicTimeOfDaySensor>();
        }

        public override string GetSetting()
        {
            return GameUtil.GetFormattedPercent(logicTimeOfDaySensor.startTime * 100f) + " : " + GameUtil.GetFormattedPercent(logicTimeOfDaySensor.duration * 100f);
        }

        [HarmonyPatch(typeof(LogicTimeOfDaySensorConfig), nameof(LogicTimeOfDaySensorConfig.DoPostConfigureComplete))]
        private class AddToClock { static void Postfix(GameObject go) => go.AddComponent<LogicTimeOfDaySensorSetting>(); }
    }
}
