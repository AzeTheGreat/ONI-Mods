using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicCounterSetting : LogicSettingDispComp
    {
        [MyCmpGet] private LogicCounter logicCounter;

        public override string GetSetting() => logicCounter.maxCount.ToString();

        [HarmonyPatch(typeof(LogicCounterConfig), nameof(LogicCounterConfig.DoPostConfigureComplete))]
        private class AddToCounter { static void Postfix(GameObject go) => go.AddComponent<LogicCounterSetting>(); }
    }
}
