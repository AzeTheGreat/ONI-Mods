using Harmony;
using UnityEngine;

namespace DefaultBuildingSettings
{
    static class Generators
    {
        [HarmonyPatch(typeof(ManualGeneratorConfig), nameof(ManualGeneratorConfig.DoPostConfigureComplete))]
        private class ManualGen_Patch
        {
            static void Postfix(GameObject go)
            {
                go.GetComponent<ManualGenerator>().SetSliderValue(Options.Opts.ManualGenValue, 0);
            }
        }

        internal static bool SetGeneratorValues(GameObject go)
        {
            if (!(go.GetComponent<EnergyGenerator>() is EnergyGenerator generator) || !go.GetComponent<ManualDeliveryKG>())
                return false;

            generator.SetSliderValue(Options.Opts.DeliverGenValue, 0);
            return true;
        }
    }
}
