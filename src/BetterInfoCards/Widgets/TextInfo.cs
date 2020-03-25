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

            if(ConverterManager.converters.TryGetValue(name, out var converter))
            {
                isConvertable = true;
                this.converter = converter;
            }
        }

        public string name;

        public object data;
        public object result;
        public Entry textEntry;

        private bool isConvertable;
        private ITextDataConverter converter;

        public string GetKey()
        {
            if (isConvertable)
                return name;
            else
                return (textEntry.widget as LocText).text;
        }

        public void FormTextResult()
        {
            if (isConvertable)
                result = converter.GetTextValue(data);
        }

        public List<List<InfoCard>> GetSplitLists(List<InfoCard> cards, int index)
        {
            return converter.GetSplitLists(cards, index);
        }

        //public void SetTextOverride(List<object> objects)
        //{

        //}

        public string GetTextOverride(List<object> results)
        {
            string original = ((LocText)textEntry.widget).text;

            if (isConvertable)
                return converter.GetTextOverride(original, results);
            else
                return original;
        }
    }
}
