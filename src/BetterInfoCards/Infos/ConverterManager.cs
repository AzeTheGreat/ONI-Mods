using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public static class ConverterManager
    {
        public const string title = "Title";
        public const string germs = "Germs";
        public const string temp = "Temp";

        private const string sumSuffix = " (Σ)";
        private const string avgSuffix = " (μ)";

        private static readonly string oreMass = Db.Get().MiscStatusItems.OreMass.Name;
        private static readonly string oreTemp = Db.Get().MiscStatusItems.OreTemp.Name;

        public static Dictionary<string, ITextDataConverter> converters = new Dictionary<string, ITextDataConverter>();

        static ConverterManager()
        {
            // TITLE
            AddConverter(
                title,
                data => {
                    GameObject go = data as GameObject;
                    KPrefabID prefabID = go.GetComponent<KPrefabID>();
                    if (prefabID != null && Assets.IsTagCountable(prefabID.PrefabTag))
                        return go.GetComponent<PrimaryElement>().Units;
                    return 1;
                },
                (original, counts) => original.RemoveCountSuffix() + " x " + counts.Sum());

            // GERMS
            AddConverter<(byte idx, int count)>(
                germs,
                data => {
                    PrimaryElement element = ((GameObject)data).GetComponent<PrimaryElement>();
                    return (element.DiseaseIdx, element.DiseaseCount);
                },
                // Impossible for multiple storages to overlap, so no need to worry about that part of the germ text since it will never be overwritten
                (original, pairs) => {
                    string text = UI.OVERLAYS.DISEASE.NO_DISEASE;
                    if (pairs[0].idx != 255)
                        text = GameUtil.GetFormattedDisease(pairs[0].idx, pairs.Sum(x => x.count), true) + sumSuffix;
                    return text;
                },
                new List<(Func<(byte, int), float>, float)>() { ( ((byte idx, int count) dP) => dP.idx, 1f) });

            // TEMP
            AddConverter(
                temp,
                data => ((GameObject)data).GetComponent<PrimaryElement>().Temperature,
                (original, temps) => GameUtil.GetFormattedTemperature(temps.Average()) + avgSuffix,
                new List<(Func<float, float>, float)>() { ((float x) => x, 10f) });

            // ORE MASS
            AddConverter(
                oreMass,
                data => ((GameObject)data).GetComponent<PrimaryElement>().Mass,
                (original, masses) => oreMass.Replace("{Mass}", GameUtil.GetFormattedMass(masses.Sum())) + sumSuffix);

            // ORE TEMP
            AddConverter(
                oreTemp,
                data => ((GameObject)data).GetComponent<PrimaryElement>().Temperature,
                (original, temps) => oreTemp.Replace("{Temp}", GameUtil.GetFormattedTemperature(temps.Average())) + avgSuffix,
                new List<(Func<float, float>, float)>() { ((float x) => x, 10f) });
        }

        public static void AddConverter<T>(string name, Func<object, T> getValue, Func<string, List<T>, string> getTextOverride, List<(Func<T, float>, float)> splitListDefs = null)
        {
            if (converters.ContainsKey(name))
                throw new Exception("Attempted to add converter with name: " + name + ", but converter with name is already present.");

            converters.Add(name, new Converter<T>(getValue, getTextOverride, splitListDefs));
        }
    }
}
