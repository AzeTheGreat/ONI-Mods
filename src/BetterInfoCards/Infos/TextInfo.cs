using AzeLib.Extensions;
using BetterInfoCards.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Entry = HoverTextDrawer.Pool<UnityEngine.MonoBehaviour>.Entry;

namespace BetterInfoCards
{
    public abstract class TextInfo
    {
        public Entry TextEntry { get; protected set; }
        public LocText Widget { get; protected set; }
        public string Key { get; protected set; }
        
        public static TextInfo Create(Entry textEntry, string name, object data)
        {
            var converter = ConverterManager.GetConverter(name);
            return converter(textEntry, name, data);
        }

        public string GetText() => Widget.text;

        public abstract string GetTextOverride(List<InfoCard> cards);
        public abstract List<List<InfoCard>> SplitByTIDefs(List<InfoCard> cards);
    }

    public class TextInfo<T> : TextInfo
    {
        private bool _isResultCached;
        private T _result;
        private T Result => _isResultCached ? _result : ((_result, _isResultCached) = (getValue(data), true))._result;

        private readonly object data;
        private readonly Func<object, T> getValue;
        private readonly Func<string, List<T>, string> getTextOverride;
        private readonly List<(Func<T, float>, float)> splitListDefs;

        public TextInfo(Entry textEntry, string key, object data, Func<object, T> getValue, Func<string, List<T>, string> getTextOverride, List<(Func<T, float>, float)> splitListDefs, Func<TextInfo, string> keyModifier)
        {
            this.TextEntry = textEntry;
            this.Widget = textEntry.widget as LocText;
            this.data = data;
            this.getValue = getValue;
            this.getTextOverride = getTextOverride;
            this.splitListDefs = splitListDefs;

            this.Key = keyModifier is null ? key : keyModifier(this).NullIfEmpty() ?? key;
        }

        public override string GetTextOverride(List<InfoCard> cards)
        {
            if (getTextOverride is null)
                return GetText();

            var results = cards.Select(x => ((TextInfo<T>)x.textInfos[Key]).Result);
            return getTextOverride(GetText(), results.ToList());
        }

        public override List<List<InfoCard>> SplitByTIDefs(List<InfoCard> cards)
        {
            if (splitListDefs is not null)
                return cards.SplitBySplitters(splitListDefs, (g, def) => GetSplitByRange(g, def));
            else
                return new() { cards };
        }

        private List<List<InfoCard>> GetSplitByRange(List<InfoCard> cards, (Func<T, float>, float) def)
        {
            var values = new SortedSet<float>(cards.Select(x => GetTIValue(x)));
            var bandRange = def.Item2;

            // Skip processing if the entire set needs only one band.
            if (values.Max - values.Min <= bandRange)
                return new List<List<InfoCard>>() { cards };

            // Determine the breakpoints by which the cards should be split.
            // Extends each band as long as it can be without exceeding the band range.
            // Not an optimal solution for range of each band, but reasonably fast.
            var breakPoints = new List<float>();
            var startVal = values.Min;
            var prevVal = 0f;

            foreach (var val in values)
            {
                if (val - startVal > bandRange)
                {
                    breakPoints.Add(prevVal);
                    startVal = val;
                }

                prevVal = val;
            }

            breakPoints.Add(prevVal);

            // Split the cards according to the breakpoints.
            return cards.SplitByKeyToDict(x => GetBreakIndex(GetTIValue(x)))
                .OrderBy(x => x.Key).Select(x=> x.Value).ToList();

            // Round the value to simplify the calculations since negligible differences are common.
            float GetTIValue(InfoCard ic) => Mathf.Round(def.Item1(((TextInfo<T>)ic.textInfos[Key]).Result));
            int GetBreakIndex(float f) => breakPoints.FindIndex(bp => f <= bp);
        }
    }
}
