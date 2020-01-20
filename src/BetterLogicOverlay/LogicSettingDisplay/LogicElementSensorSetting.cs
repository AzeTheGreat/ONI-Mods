using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicElementSensorSetting : LogicSettingDispComp
    {
        private Traverse desiredElementIdx;

        new private void Start()
        {
            base.Start();
            desiredElementIdx = Traverse.Create(gameObject.GetComponent<LogicElementSensor>()).Field("desiredElementIdx");
        }

        public override string GetSetting()
        {
            Element element = ElementLoader.elements[desiredElementIdx.GetValue<byte>()];
            return element.GetAbbreviation();
        }

        [HarmonyPatch(typeof(LogicElementSensorGasConfig), nameof(LogicElementSensorGasConfig.DoPostConfigureComplete))]
        private class AddToGas { static void Postfix(GameObject go) => go.AddComponent<LogicElementSensorSetting>(); }

        [HarmonyPatch(typeof(LogicElementSensorLiquidConfig), nameof(LogicElementSensorLiquidConfig.DoPostConfigureComplete))]
        private class AddToLiquid { static void Postfix(GameObject go) => go.AddComponent<LogicElementSensorSetting>(); }
    }
}
