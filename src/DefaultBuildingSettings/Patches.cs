using Harmony;
using UnityEngine;

namespace DefaultBuildingSettings
{
    // Not an ideal approach, but the best that can be done given the game's current structure
    // TODO: Check periodically and see if the configs can just be edited as normal.
    [HarmonyPatch(typeof(BuildingDef), nameof(BuildingDef.Build))]
    public class Patches
    {
        static void Postfix(GameObject __result)
        {
            var suitMarker = __result.GetComponent<SuitMarker>();
            if(suitMarker)
                Traverse.Create(suitMarker).Field("onlyTraverseIfUnequipAvailable").SetValue(Options.Opts.VacancyOnly);

            var storage = __result.GetComponent<Storage>();
            if (storage)
                Traverse.Create(storage).Field("onlyFetchMarkedItems").SetValue(Options.Opts.SweepOnly);
        }
    }
}
