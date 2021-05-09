using Harmony;
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
                data => ((GameObject)data).GetComponent<PrimaryElement>().Mass,
                (original, masses) => oreMass.Name.Replace("{Mass}", GameUtil.GetFormattedMass(masses.Sum())) + ConverterManager.sumSuffix);

            // ORE TEMP
            ConverterManager.AddConverter(
                oreTemp.Id,
                data => ((GameObject)data).GetComponent<PrimaryElement>().Temperature,
                (original, temps) => oreTemp.Name.Replace("{Temp}", GameUtil.GetFormattedTemperature(temps.Average())) + ConverterManager.avgSuffix,
                new() { (x => x, Options.Opts.TemperatureBandWidth) });
        }
    }
}
