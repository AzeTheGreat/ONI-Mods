using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class StatusDataManager
    {
        public static readonly Dictionary<string, StatusData> statusConverter = new Dictionary<string, StatusData>()
        {
            { "Title", new StatusData() {
                getStatusValue = data => {
                    GameObject go = data as GameObject;
                    KPrefabID prefabID = go.GetComponent<KPrefabID>();
                    if (prefabID != null && Assets.IsTagCountable(prefabID.PrefabTag))
                        return go.GetComponent<PrimaryElement>().Units;
                    return 1; },
                getTextOverride = (name, counts, original) => original + " x " + counts.Sum(),
                getSplitLists = (infoCards) => new List<List<InfoCard>> {infoCards}
            } },

            { "Germs", new StatusData() {
                getStatusValue = data => {
                    GameObject go = data as GameObject;
                    KPrefabID prefabID = go.GetComponent<KPrefabID>();
                    if (prefabID != null && Assets.IsTagCountable(prefabID.PrefabTag))
                        return go.GetComponent<PrimaryElement>().Units;
                    return 1; },
                getTextOverride = (name, counts, original) => original + " x " + counts.Sum(),
                getSplitLists = (infoCards) => new List<List<InfoCard>> {infoCards}
            } },

            { Db.Get().MiscStatusItems.OreMass.Name, new StatusData() {
                getStatusValue = go => ((GameObject)go).GetComponent<PrimaryElement>().Mass,
                getTextOverride = (name, masses, original) => name.Replace("{Mass}", GameUtil.GetFormattedMass(masses.Sum())) + " (Σ)",
                getSplitLists = (infoCards) => new List<List<InfoCard>> {infoCards}
            } },

            { Db.Get().MiscStatusItems.OreTemp.Name, new StatusData() {
                getStatusValue = go => ((GameObject)go).GetComponent<PrimaryElement>().Temperature,
                getTextOverride = (name, temps, original) => name.Replace("{Temp}", GameUtil.GetFormattedTemperature(temps.Average())) + " (μ)",
                getSplitLists = (infoCards) => GetSplitLists(infoCards, Db.Get().MiscStatusItems.OreTemp.Name, 10f)
            } }
        };

        private static List<List<InfoCard>> GetSplitLists(List<InfoCard> cards, string name, float maxRange)
        {
            List<float> values = cards.Select(x => x.textValues[name]).ToList();
            values.Sort();

            var splits = new List<List<InfoCard>>();

            float range = values[values.Count - 1] - values[0];
            int maxLists = Mathf.CeilToInt(range / maxRange);

            if (maxLists == 1)
            {
                splits.Add(cards);
                return splits;
            }

            var listStartIndices = new List<int>();
            float listStartValue = 0;

            for (int i = 0; i < values.Count; i++)
            {
                float value = values[i];

                if (value > listStartValue + maxRange)
                {
                    listStartIndices.Add(i);
                    listStartValue = value;
                }   
            }
            listStartIndices.Add(values.Count);

            for (int i = 0; i < listStartIndices.Count - 1; i++)
            {
                int startIndex = listStartIndices[i];
                int endIndex = listStartIndices[i + 1];
                splits.Add(cards.GetRange(startIndex, endIndex - startIndex));
            }

            return splits;
        }

        private static string RemoveQuantityCount(string text)
        {
            var charStack = new Stack<char>();
            int i;
            for (i = text.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(text[i]))
                    break;

                charStack.Push(text[i]);
            }

            text = text.Remove(i - 1, text.Length - i + 1);

            return text;
        }

        public class StatusData
        {
            public Func<object, float> getStatusValue;
            public Func<string, List<float>, string, string> getTextOverride;
            public Func<List<InfoCard>, List<List<InfoCard>>> getSplitLists;
        }
    }
}
