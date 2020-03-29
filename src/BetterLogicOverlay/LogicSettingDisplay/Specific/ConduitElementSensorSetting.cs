using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ConduitElementSensorSetting : LogicSettingDispComp
    {
        [MyCmpGet] private Filterable filterable;

        public override string GetSetting()
        {
            return filterable.SelectedTag.GetAbbreviation();
        }

        [HarmonyPatch]
        private class Add
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(GasConduitElementSensorConfig), nameof(GasConduitElementSensorConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(LiquidConduitElementSensorConfig), nameof(LiquidConduitElementSensorConfig.DoPostConfigureComplete));
                yield return AccessTools.Method(typeof(SolidConduitElementSensorConfig), nameof(SolidConduitElementSensorConfig.DoPostConfigureComplete));
            }

            static void Postfix(GameObject go) => go.AddComponent<ConduitElementSensorSetting>();
        }
    }
}
