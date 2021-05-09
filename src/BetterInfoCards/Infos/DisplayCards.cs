using BetterInfoCards.Util;
using System.Collections.Generic;

namespace BetterInfoCards
{
    public class DisplayCards
    {
        public List<DisplayCard> UpdateData(List<InfoCard> infoCards)
        {
            var displayCards = new List<DisplayCard>();

            if (infoCards == null || infoCards.Count <= 0)
                return displayCards;

            var displaySplit = infoCards.SplitToDict((card) => card.GetTitleKey())
                .SplitToDict((card) => card.GetTitleKey() + card.GetTextKey())
                .SplitToList((cards) => cards.SplitBySplitters(cards[0].textInfos, (cards, textInfo, i) => textInfo.SplitByTIDefs(cards, i)));

            foreach (var cards in displaySplit)
                displayCards.Add(new DisplayCard(cards));

            return displayCards;
        }
    }
}
