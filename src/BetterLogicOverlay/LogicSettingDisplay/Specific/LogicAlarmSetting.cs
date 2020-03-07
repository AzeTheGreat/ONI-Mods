using AzeLib.Extensions;
using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.Specific
{
    class LogicAlarmSetting : LogicSettingDispComp
    {
        [MyCmpGet] LogicAlarm logicAlarm;

        public override string GetSetting() => logicAlarm.notificationName.Truncate(5);

        [HarmonyPatch(typeof(LogicAlarmConfig), nameof(LogicAlarmConfig.DoPostConfigureComplete))]
        private class AddToAlarm { static void Postfix(GameObject go) => go.AddComponent<LogicAlarmSetting>(); }
    }
}
