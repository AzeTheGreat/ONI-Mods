using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards.Converters
{
    // These need to be added later because referencing Db too early causes it to initialize (it's static) improperly and crash.
    [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
    class StatusConverters
    {
        static void Postfix()
        {
            var oreMass = Db.Get().MiscStatusItems.OreMass;
            var oreTemp = Db.Get().MiscStatusItems.OreTemp;

            // ORE MASS
            ConverterManager.AddConverter(
                oreMass.Id,
                data =>
                {
                    var go = (GameObject)data;
                    var primaryElement = go.GetComponent<PrimaryElement>();
                    if (primaryElement == null)
                    {
                        LogMissingPrimaryElementOnce(ref hasLoggedMissingPrimaryElementForMass, go, oreMass.Id);
                        return 0f;
                    }

                    return primaryElement.Mass;
                },
                (original, masses) => oreMass.Name.Replace("{Mass}", GameUtil.GetFormattedMass(masses.Sum())) + ConverterManager.sumSuffix);

            // ORE TEMP
            ConverterManager.AddConverter(
                oreTemp.Id,
                data =>
                {
                    var go = (GameObject)data;
                    var primaryElement = go.GetComponent<PrimaryElement>();
                    if (primaryElement == null)
                    {
                        LogMissingPrimaryElementOnce(ref hasLoggedMissingPrimaryElementForTemp, go, oreTemp.Id);
                        return 0f;
                    }

                    return primaryElement.Temperature;
                },
                (original, temps) => oreTemp.Name.Replace("{Temp}", GameUtil.GetFormattedTemperature(temps.Average())) + ConverterManager.avgSuffix,
                new() { (x => x, Options.Opts.TemperatureBandWidth) });
        }

        private static bool hasLoggedMissingPrimaryElementForMass;
        private static bool hasLoggedMissingPrimaryElementForTemp;

        private static void LogMissingPrimaryElementOnce(ref bool hasLogged, GameObject go, string converterId)
        {
            if (hasLogged)
                return;

            hasLogged = true;
            Debug.LogWarning($"[BetterInfoCards] Missing PrimaryElement for converter '{converterId}' on object '{go?.name ?? "<null>"}'. Using a safe default.");
        }
    }
}
