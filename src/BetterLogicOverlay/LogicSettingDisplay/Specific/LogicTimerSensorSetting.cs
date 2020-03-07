using Harmony;
using STRINGS;
using System;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicTimerSensorSetting : LogicSettingDispComp
    {
        [MyCmpGet] private LogicTimerSensor logicTimerSensor;

        public override string GetSetting()
        {
            return GetFormattedDuration(logicTimerSensor.onDuration) + " / " + GetFormattedDuration(logicTimerSensor.offDuration);

            string GetFormattedDuration(float duration)
            {
                if (duration > 600f)
                {
                    float val = duration / 600f;
                    if(val >= 10f)
                        return GameUtil.FloatToString(val, "F1") + "c";
                    else
                        return GameUtil.FloatToString(val, "F2") + "c";
                }
                    
                return GameUtil.GetFormattedTime(duration, "F0");
            }
        }

        [HarmonyPatch(typeof(LogicTimerSensorConfig), nameof(LogicTimerSensorConfig.DoPostConfigureComplete))]
        private class AddToClock { static void Postfix(GameObject go) => go.AddComponent<LogicTimerSensorSetting>(); }
    }
}
