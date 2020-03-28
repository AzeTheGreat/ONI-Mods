using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BetterInfoCards.Converters
{
    // These need to be added later because referencing Db too early causes it to initialize (it's static) improperly and crash.
    [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
    class StatusConverters
    {
        private static readonly string oreMass = Db.Get().MiscStatusItems.OreMass.Name;
        private static readonly string oreTemp = Db.Get().MiscStatusItems.OreTemp.Name;

        static void Postfix()
        {
            // ORE MASS
            ConverterManager.AddConverter(
                oreMass,
                data => ((GameObject)data).GetComponent<PrimaryElement>().Mass,
                (original, masses) => oreMass.Replace("{Mass}", GameUtil.GetFormattedMass(masses.Sum())) + ConverterManager.sumSuffix);

            // ORE TEMP
            ConverterManager.AddConverter(
                oreTemp,
                data => ((GameObject)data).GetComponent<PrimaryElement>().Temperature,
                (original, temps) => oreTemp.Replace("{Temp}", GameUtil.GetFormattedTemperature(temps.Average())) + ConverterManager.avgSuffix,
                new List<(Func<float, float>, float)>() { ((float x) => x, 10f) });
        }
    }
}
