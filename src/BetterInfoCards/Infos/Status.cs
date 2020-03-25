using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public interface ITextDataConverter
    {
        string GetTextOverride(string original, List<object> values);
        object GetTextValue(object data);
        List<List<InfoCard>> GetSplitLists(List<InfoCard> cards, int index);
    }

    public class Status<T> : ITextDataConverter
    {
        public Status(Func<object, T> getValue, Func<string, List<T>, string> getTextOverride, List<(Func<T, float>, float)> splitListDefs = null)
        {
            this.getValue = getValue;
            this.getTextOverride = getTextOverride;
            this.splitListDefs = splitListDefs ?? new List<(Func<T, float>, float)>();
        }

        private readonly Func<object, T> getValue;
        private readonly Func<string, List<T>, string> getTextOverride;
        private readonly List<(Func<T, float>, float)> splitListDefs;

        public object GetTextValue(object data) => getValue(data);
        public string GetTextOverride(string original, List<object> values) => getTextOverride(original, values.Cast<T>().ToList());

        public List<List<InfoCard>> GetSplitLists(List<InfoCard> cards, int index)
        {
            var splits = new List<List<InfoCard>>() { cards };

            foreach (var splitter in splitListDefs)
            {
                var newSplits = new List<List<InfoCard>>();

                foreach (var split in splits)
                    newSplits.AddRange(GetSplitByRange(split, split.Select(x => splitter.Item1((T)x.textInfos[index].result)).ToList(), splitter.Item2));

                splits = newSplits;
            }

            return splits;
        }

        private static List<List<InfoCard>> GetSplitByRange(List<InfoCard> cards, List<float> values, float maxRange)
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
