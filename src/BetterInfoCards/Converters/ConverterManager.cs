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

        public const string sumSuffix = " (Σ)";
        public const string avgSuffix = " (μ)";

        private static Dictionary<string, Func<Entry, string, object, TextInfo>> converters = new Dictionary<string, Func<Entry, string, object, TextInfo>>();

        static ConverterManager()
        {
            // DEFAULT
            AddConverter<object>(
                string.Empty,
                data => null,
                (original, o) => original);

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
        }

        public static void AddConverter<T>(string name, Func<object, T> getValue, Func<string, List<T>, string> getTextOverride, List<(Func<T, float>, float)> splitListDefs = null)
        {
            if (converters.ContainsKey(name))
                throw new Exception("Attempted to add converter with name: " + name + ", but converter with name is already present.");

            converters.Add(name, (Entry e, string n, object d) => new TextInfo<T>(e, n, d, getValue, getTextOverride, splitListDefs));
        }

        // This is not to be used internally - for reflection from external mods only.
        private static void AddConverterReflect(string name, object getValue, object getTextOverride, object splitListDefs)
        {
            var type = getValue.GetType().GetGenericArguments()[1];
            var method = typeof(ConverterManager).GetMethod(nameof(ConverterManager.AddConverter)).MakeGenericMethod(type);
            method.Invoke(null, new object[] { name, getValue, getTextOverride, splitListDefs });
        }

        public static Func<Entry, string, object, TextInfo> GetConverter(string name)
        {
            if (converters.TryGetValue(name, out var converter))
                return converter;

            else if (converters.TryGetValue(string.Empty, out var defaultConv))
                return defaultConv;

            throw new Exception("Somehow name: `" + name + "` wasn't in the dict, and the fallback `string.Empty` wasn't either...");
        }
    }
}
