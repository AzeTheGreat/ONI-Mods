using System;
using System.Collections.Generic;

namespace BetterInfoCards
{
    // TODO: Potentially change to struct?
    public class TextInfo
    {
        public TextInfo(Entry textEntry)
        {
            this.textEntry = textEntry;
        }

        public string name;

        public object data;
        public object result;
        public Entry textEntry;

        public Func<object, object> resultFromData;
        public Func<List<object>, List<List<object>>> getSplitsFromResults;
        public Func<List<object>, string> getTextOverrideFromResults;

        public void FormTextResults()
        {

        }

        public List<List<object>> GetSplitLists(List<object> objects)
        {
            return null;
        }

        public void SetTextOverride(List<object> objects)
        {

        }

        public void AddData(string name, object data)
        {
            this.name = name;
            this.data = data;
        }
    }
}
