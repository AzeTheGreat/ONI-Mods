using System;
using System.Collections.Generic;
using System.Linq;

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
        public Status(string name, Func<object, T> getValue, Func<string, List<T>, string> getTextOverride, Func<List<InfoCard>, int, List<List<InfoCard>>> getSplitLists)
        {
            this.name = name;
            this.getValue = getValue;
            this.getTextOverride = getTextOverride;
            this.getSplitLists = getSplitLists;
        }

        private readonly string name;
        private readonly Func<object, T> getValue;
        private readonly Func<string, List<T>, string> getTextOverride;
        private readonly Func<List<InfoCard>, int, List<List<InfoCard>>> getSplitLists;

        public object GetTextValue(object data) => getValue(data);
        public string GetTextOverride(string original, List<object> values) => getTextOverride(original, values.Cast<T>().ToList());
        public List<List<InfoCard>> GetSplitLists(List<InfoCard> cards, int index) => getSplitLists(cards, index);
    }
}
