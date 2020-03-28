using BetterInfoCards.Util;
using System.Collections.Generic;
using System.Linq;

namespace BetterInfoCards
{
    public class DisplayCards
    {
        private List<DisplayCard> displayCards;

        public List<DisplayCard> UpdateData(List<InfoCard> infoCards)
        {
            displayCards = new List<DisplayCard>();

            if (infoCards == null || infoCards.Count <= 0)
                return displayCards;

            var displaySplit = infoCards.SplitToDict((card) => card.GetTitleKey())
                .SplitToDict((card) => card.GetTitleKey() + card.GetTextKey())
                .SplitToList((cards) => SplitTest(cards));

            foreach (List<InfoCard> cards in displaySplit)
                displayCards.Add(new DisplayCard(cards));

            return displayCards;
        }

        private List<List<InfoCard>> SplitTest(List<InfoCard> source)
        {
            foreach (InfoCard card in source)
                card.FormTextValues();

            var textInfo = source[0].textInfos;
            return source.SplitBySplitters(textInfo, (g, def, i) => textInfo[i].GetSplitLists(g, i));
        }
    }
}
