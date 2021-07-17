using BetterInfoCards.Util;
using System.Collections.Generic;
using System.Linq;

namespace BetterInfoCards
{
    public class DisplayCards
    {
        public List<DisplayCard> UpdateData(List<InfoCard> infoCards)
        {
            var displayCards = new List<DisplayCard>();

            if (infoCards == null || infoCards.Count <= 0)
                return displayCards;

            var dSplit = infoCards.SplitByKey(card => card.GetTitleKey());
            var dSplit2 = dSplit.SplitMany(cards => cards.SplitByKey(card => card.textInfos.Count));
            var dSplit3 = GetTextKeySplits(dSplit2);

            // Assumes each IC in the group has the same TIs (order doesn't matter).
            var dSplit4 = dSplit3.SplitMany(cards => cards.SplitBySplitters(
                cards.First().textInfos.ToList(),
                (group, ti) => ti.Value.SplitByTIDefs(group)));

            foreach (var cards in dSplit4)
                displayCards.Add(new DisplayCard(cards));

            return displayCards;

            // Assumes that the ICs in the same group have the same number of TIs.
            // It's too intensive to check TIs beyond the first IC in a group.
            List<List<InfoCard>> GetTextKeySplits(List<List<InfoCard>> groups, List<string> history = null)
            {
                // History is usually pretty short, so List is faster here than HashSet
                // (threshold around 5 items: https://stackoverflow.com/questions/150750/hashset-vs-list-performance/)
                if (history is null)
                    history = new();

                return groups.SplitMany(cards =>
                {
                    var textInfos = cards.First().textInfos.Keys.Except(history);
                    if (!textInfos.Any())
                        return new() { cards };

                    history.AddRange(textInfos);

                    return GetTextKeySplits(
                        cards.SplitBySplitters(textInfos.ToList(), (group, ti) => group.SplitByKey(card => card.textInfos.ContainsKey(ti))),
                        new (history));
                });
            }
        }
    }
}
