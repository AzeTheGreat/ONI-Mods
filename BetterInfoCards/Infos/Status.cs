using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterInfoCards
{
    public interface ITextDataConverter
    {
        string GetTextOverride(string original, List<object> values);
        List<List<InfoCard>> GetSplitLists(List<InfoCard> cards);
        object GetTextValue(object data);
    }

    public class Status<T> : ITextDataConverter
    {
        public Status(string name, Func<object, T> getValue, Func<string, List<T>, string> getTextOverride, Func<List<InfoCard>, List<List<InfoCard>>> getSplitLists)
        {
            this.name = name;
            this.getStatusValue = getValue;
            this.getTextOverride = getTextOverride;
            this.getSplitLists = getSplitLists;
            //values = infoCards.ForEach(x => getStatusValue(x.statusDatas[name]));
        }

        private readonly string name;
        private readonly Func<object, T> getStatusValue;
        private readonly Func<string, List<T>, string> getTextOverride;
        private readonly Func<List<InfoCard>, List<List<InfoCard>>> getSplitLists;

        public object GetTextValue(object data) => getStatusValue(data);
        public string GetTextOverride(string original, List<object> values) => getTextOverride(original, values.Cast<T>().ToList());
        public List<List<InfoCard>> GetSplitLists(List<InfoCard> cards) => getSplitLists(cards);
    }
}
