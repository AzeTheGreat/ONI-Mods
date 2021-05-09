using AzeLib.Extensions;
using BetterInfoCards.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public abstract class TextInfo
    {
        public Entry TextEntry { get; protected set; }
        public string Key { get; protected set; }
        
        public static TextInfo Create(Entry textEntry, string name, object data)
        {
            var converter = ConverterManager.GetConverter(name);
            return converter(textEntry, name, data);
        }

        public string GetText() => ((LocText)TextEntry.widget).text;

        public abstract object Result { get; protected set; }
        public abstract string GetTextOverride(List<object> results);
        public abstract List<List<InfoCard>> SplitByTIDefs(List<InfoCard> cards, int i);
    }

    public class TextInfo<T> : TextInfo
    {
        private object _result;
        public override object Result
        {
            get
            {
                if (_result is null)
                    _result = getValue(data);
                return _result;
            }
            protected set { }
        }

        private readonly object data;
        private readonly Func<object, T> getValue;
        private readonly Func<string, List<T>, string> getTextOverride;
        private readonly List<(Func<T, float>, float)> splitListDefs;

        public TextInfo(Entry textEntry, string key, object data, Func<object, T> getValue, Func<string, List<T>, string> getTextOverride, List<(Func<T, float>, float)> splitListDefs, Func<TextInfo, string> keyModifier)
        {
            this.TextEntry = textEntry;
            this.data = data;
            this.getValue = getValue;
            this.getTextOverride = getTextOverride;
            this.splitListDefs = splitListDefs ?? new List<(Func<T, float>, float)>();
            this.Key = keyModifier is null ? key : keyModifier(this).NullIfEmpty() ?? key;
        }

        public override string GetTextOverride(List<object> results)
        {
            return getTextOverride(GetText(), results.Cast<T>().ToList());
        }

        public override List<List<InfoCard>> SplitByTIDefs(List<InfoCard> cards, int i)
        {
            if (splitListDefs is not null)
                return cards.SplitBySplitters(splitListDefs, (g, def, j) => GetSplitByRange(g, def, i));
            else
                return new List<List<InfoCard>>() { cards };
        }

        private List<List<InfoCard>> GetSplitByRange(List<InfoCard> cards, (Func<T, float>, float) def, int i)
        {
            var values = cards.Select(x => def.Item1((T)x.textInfos[i].Result)).ToList();
            var maxRange = def.Item2;

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

            for (int j = 0; j < values.Count; j++)
            {
                float value = values[j];

                if (value > listStartValue + maxRange)
                {
                    listStartIndices.Add(j);
                    listStartValue = value;
                }
            }
            listStartIndices.Add(values.Count);

            for (int j = 0; j < listStartIndices.Count - 1; j++)
            {
                int startIndex = listStartIndices[j];
                int endIndex = listStartIndices[j + 1];
                splits.Add(cards.GetRange(startIndex, endIndex - startIndex));
            }

            return splits;
        }
    }
}
