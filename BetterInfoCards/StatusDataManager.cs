using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterInfoCards
{
    public class StatusDataManager
    {
        public static readonly Dictionary<string, StatusData> statusConverter = new Dictionary<string, StatusData>()
        {
            { Db.Get().MiscStatusItems.OreMass.Name, new StatusData() {
                getStatusValue = go => ((GameObject)go).GetComponent<PrimaryElement>().Mass,
                getTextOverride = (name, masses) => "Net: " + name.Replace("{Mass}", GameUtil.GetFormattedMass((masses.Cast<float>()).Sum())),
                getSplitLists = (infoCards) => new List<List<InfoCard>> {infoCards}
            } },
            { Db.Get().MiscStatusItems.OreTemp.Name, new StatusData() {
                getStatusValue = go => ((GameObject)go).GetComponent<PrimaryElement>().Temperature,
                getTextOverride = (name, temps) => "~ " + name.Replace("{Temp}", GameUtil.GetFormattedTemperature(temps.Cast<float>().Average())),
                getSplitLists = (infoCards) => GetSplitLists(infoCards, Db.Get().MiscStatusItems.OreTemp.Name, 10f)
            } }
        };

        private static List<List<InfoCard>> GetSplitLists(List<InfoCard> cards, string name, float maxRange)
        {
            List<float> values = cards.Select(x => x.textValues[name]).Cast<float>().ToList();
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

        public class StatusData
        {
            public Func<object, ValueType> getStatusValue;
            public Func<string, List<ValueType>, string> getTextOverride;
            public Func<List<InfoCard>, List<List<InfoCard>>> getSplitLists;
        }
    }
}
