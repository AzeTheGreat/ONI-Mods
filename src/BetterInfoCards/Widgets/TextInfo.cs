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

        public void AddData(string name, object data)
        {
            this.name = name;
            this.data = data;

            if(StatusDataManager.statusConverter.TryGetValue(name, out var statusData))
            {
                isStatus = true;
                resultFromData = statusData.GetTextValue;
                getSplitsFromResults = statusData.GetSplitLists;
                getTextOverrideFromResults = statusData.GetTextOverride;
            }
        }

        public string name;

        public object data;
        public object result;
        public Entry textEntry;
        private bool isStatus;

        public Func<object, object> resultFromData;
        public Func<List<InfoCard>, int, List<List<InfoCard>>> getSplitsFromResults;
        public Func<string, List<object>, string> getTextOverrideFromResults;

        public string GetKey()
        {
            if (isStatus)
                return name;
            else
                return (textEntry.widget as LocText).text;
        }

        public void FormTextResult()
        {
            if (isStatus)
                result = resultFromData(data);
        }

        //public List<List<object>> GetSplitLists(List<object> objects)
        //{

        //}

        //public void SetTextOverride(List<object> objects)
        //{

        //}

        public string GetTextOverride(List<object> results)
        {
            string original = ((LocText)textEntry.widget).text;

            if (isStatus)
                return getTextOverrideFromResults(original, results);
            else
                return original;
        }
    }
}
