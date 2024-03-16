using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Entry = HoverTextDrawer.Pool<UnityEngine.MonoBehaviour>.Entry;

namespace BetterInfoCards
{
    public static class ConverterManager
    {
        public const string title = "Title";
        public const string germs = "Germs";
        public const string temp = "Temp";

        public const string sumSuffix = " <color=#ababab>(Σ)</color>";
        public const string avgSuffix = " <color=#ababab>(μ)</color>";

        private static readonly Dictionary<string, Func<string, string, object, TextInfo>> converters = new();

        static ConverterManager()
        {
            // DEFAULT
            AddConverter<object>(
                string.Empty,
                data => null,
                null,
                null);

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
                    try {
                    PrimaryElement element = ((GameObject)data).GetComponent<PrimaryElement>();
                    return (element.DiseaseIdx, element.DiseaseCount);
                    }
                    catch (NullReferenceException)
                    {
                        Debug.Log("Issue encountered in germs converter (getValue)");
                        Debug.Log("Data: " + data);
                        Debug.Log("GameObject: " + ((GameObject)data) + "; " + ((GameObject)data)?.name);

                        var element = ((GameObject)data)?.GetComponent<PrimaryElement>();
                        Debug.Log("Element: " + element);

                        Debug.Log("Idx: " + element?.DiseaseIdx + "; Count: " + element?.DiseaseCount);

                        Debug.LogError("Hi, you've hit an edge case crash in Better Info Cards.\n" +
                            "PLEASE upload the full player.log to the below issue so I can pin it down.\n" +
                            "https://github.com/AzeTheGreat/ONI-Mods/issues/33\n" +
                            "--------------------------------------------------");
                        throw;
                    }
                },
                // Impossible for multiple storages to overlap, so no need to worry about that part of the germ text since it will never be overwritten
                (original, pairs) => {
                    string text = UI.OVERLAYS.DISEASE.NO_DISEASE;
                    if (pairs[0].idx != 255)
                        text = GameUtil.GetFormattedDisease(pairs[0].idx, pairs.Sum(x => x.count), true) + sumSuffix;
                    return text;
                },
                new() { ( ((byte idx, int count) dP) => dP.idx, 1f) });

            // TEMP
            AddConverter(
                temp,
                data => ((GameObject)data).GetComponent<PrimaryElement>().Temperature,
                (original, temps) => GameUtil.GetFormattedTemperature(temps.Average()) + avgSuffix,
                new() { (x => x, Options.Opts.TemperatureBandWidth) });
        }

        public static void AddConverter<T>(string name, Func<object, T> getValue, Func<string, List<T>, string> getTextOverride = null, List<(Func<T, float>, float)> splitListDefs = null) where T : new()
        {
            if (converters.ContainsKey(name))
                throw new Exception("Attempted to add converter with name: " + name + ", but converter with name is already present.");

            var pool = new ResetPool<TextInfo<T>>(ref InterceptHoverDrawer.BeginDrawing.onBeginDrawing);
            converters.Add(name, (string k, string n, object d) => pool.Get().Set(k, n, d, getValue, getTextOverride, splitListDefs));
        }

        // This is not to be used internally - for reflection from external mods only.
        private static void AddConverterReflect(string name, object getValue, object getTextOverride, object splitListDefs)
        {
            var type = getValue.GetType().GetGenericArguments()[1];
            var method = typeof(ConverterManager).GetMethod(nameof(ConverterManager.AddConverter)).MakeGenericMethod(type);
            method.Invoke(null, new object[] { name, getValue, getTextOverride, splitListDefs });
        }

        public static bool TryGetConverter(string id, out Func<string, string, object, TextInfo> converter)
        {
            // TODO: Pull default converter out of dict?  Maybe Title as well?
            if (id != string.Empty && converters.TryGetValue(id, out converter))
                return true;

            converter = converters[string.Empty];
            return false;
        }
    }
}
