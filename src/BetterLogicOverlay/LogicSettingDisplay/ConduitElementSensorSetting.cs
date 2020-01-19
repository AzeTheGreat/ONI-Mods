using Harmony;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ConduitElementSensorSetting : KMonoBehaviour, ILogicSettingDisplay
    {
        private Traverse desiredElementIdx;

        new private void Start()
        {
            base.Start();
            desiredElementIdx = Traverse.Create(gameObject.GetComponent<ConduitElementSensor>()).Field("desiredElement");
        }

        public string GetSetting()
        {
            Element element = ElementLoader.FindElementByHash(desiredElementIdx.GetValue<SimHashes>());
            return element.nameUpperCase;
        }

        [HarmonyPatch(typeof(GasConduitElementSensorConfig), nameof(GasConduitElementSensorConfig.DoPostConfigureComplete))]
        private class AddToGas { static void Postfix(GameObject go) => go.AddComponent<ConduitElementSensorSetting>(); }

        [HarmonyPatch(typeof(LiquidConduitElementSensorConfig), nameof(LiquidConduitElementSensorConfig.DoPostConfigureComplete))]
        private class AddToLiquid { static void Postfix(GameObject go) => go.AddComponent<ConduitElementSensorSetting>(); }
    }
}
