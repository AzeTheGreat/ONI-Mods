using AzeLib.Attributes;
using HarmonyLib;
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

        [ApplyToBuildingPrefabs]
        internal static void SetGeneratorValues(GameObject go)
        {
            if (!(go.GetComponent<EnergyGenerator>() is EnergyGenerator generator) || !go.GetComponent<ManualDeliveryKG>())
                return;

            generator.SetSliderValue(Options.Opts.DeliverGenValue, 0);
        }
    }
}
