using BetterInfoCards.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public abstract class TextInfo
    {
        public abstract Entry TextEntry { get; protected set; }
        public abstract object Result { get; protected set; }
        public abstract string Key { get; protected set; }

        public static TextInfo Create(Entry textEntry, string name, object data)
        {
            var converter = ConverterManager.GetConverter(name);
            return converter(textEntry, name, data);
        }

        public abstract List<List<InfoCard>> GetSplitLists(List<InfoCard> cards, int index);
        public abstract string GetTextOverride(List<object> results);
        public abstract void FormTextResult();
    }

    public class TextInfo<T> : TextInfo
    {
        public override string Key { get; protected set; }
        public override object Result { get; protected set; }
        public override Entry TextEntry { get; protected set; }

        private readonly object data;

        private readonly Func<object, T> getValue;
        private readonly Func<string, List<T>, string> getTextOverride;
        private readonly List<(Func<T, float>, float)> splitListDefs;

        public TextInfo(Entry textEntry, string key, object data, Func<object, T> getValue, Func<string, List<T>, string> getTextOverride, List<(Func<T, float>, float)> splitListDefs = null)
        {
            this.TextEntry = textEntry;
            this.Key = key;
            this.data = data;
            this.getValue = getValue;
            this.getTextOverride = getTextOverride;
            this.splitListDefs = splitListDefs ?? new List<(Func<T, float>, float)>();
        }

        public override void FormTextResult()
        {
            Result = getValue(data);
        }

        public override List<List<InfoCard>> GetSplitLists(List<InfoCard> source, int index)
        {
            var cards = new List<List<InfoCard>>() { source };

            if (Result == null)
                return cards;
            return cards.SplitToList((group) => GetSplits(group));

            List<List<InfoCard>> GetSplits(List<InfoCard> group) => group.SplitBySplitters(splitListDefs, (g, def) => GetSplitByRange(g, g.Select(x => def.Item1((T)x.textInfos[index].Result)).ToList(), def.Item2));
        }

        public override string GetTextOverride(List<object> results)
        {
            string original = ((LocText)TextEntry.widget).text;
            return getTextOverride(original, results.Cast<T>().ToList());
        }

        private List<List<InfoCard>> GetSplitByRange(List<InfoCard> cards, List<float> values, float maxRange)
        {
            cards = cards.OrderBy(x => values[cards.IndexOf(x)]).ToList();
            values.Sort();

            var splits = new List<List<InfoCard>>();

            float range = values[values.Count - 1] - values[0];
            int maxLists = Mathf.CeilToInt(range / maxRange);

            if (maxLists <= 1)
            {
                splits.Add(cards);
                return splits;
            }

            var listStartIndices = new List<int>() { 0 };
            float listStartValue = values[0];

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
    }
}
