using System.Collections.Generic;

namespace BetterInfoCards
{
    public interface ITextDataConverter
    {
        string GetTextOverride(string original, List<object> values);
        List<List<InfoCard>> GetSplitLists(List<InfoCard> cards);
        object GetTextValue(object data);
    }
}
