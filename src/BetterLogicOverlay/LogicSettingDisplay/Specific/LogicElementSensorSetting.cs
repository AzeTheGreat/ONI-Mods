using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicElementSensorSetting : LogicSettingDispComp
    {
        [MyCmpGet] LogicElementSensor logicElementSensor;

        public override string GetSetting()
        {
            var element = ElementLoader.elements[logicElementSensor.desiredElementIdx];
            return element.GetAbbreviation();
        }

        [HarmonyPatch(typeof(LogicElementSensorGasConfig), nameof(LogicElementSensorGasConfig.DoPostConfigureComplete))]
        private class AddToGas { static void Postfix(GameObject go) => go.AddComponent<LogicElementSensorSetting>(); }

        [HarmonyPatch(typeof(LogicElementSensorLiquidConfig), nameof(LogicElementSensorLiquidConfig.DoPostConfigureComplete))]
        private class AddToLiquid { static void Postfix(GameObject go) => go.AddComponent<LogicElementSensorSetting>(); }
    }
}
