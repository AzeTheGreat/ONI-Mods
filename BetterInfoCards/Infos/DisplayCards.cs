using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterInfoCards
{
    public class DisplayCards
    {
        private List<DisplayCard> displayCards = new List<DisplayCard>();

        public DisplayCards(List<InfoCard> infoCards)
        {
            var nameSplit = new Dictionary<string, List<InfoCard>>();
            var displaySplit = new List<List<InfoCard>>();
            var detailSplit = new Dictionary<string, List<InfoCard>>();

            foreach (InfoCard card in infoCards)
            {
                string key = card.Title;
                var value = new List<InfoCard>();

                if (!nameSplit.TryGetValue(key, out value))
                    nameSplit[key] = value;
                value.Add(card);
            }

            foreach (var kvp in nameSplit)
            {
                // If there are multiple infoCards with the same title, consider stacking them
                if(kvp.Value.Count > 1)
                {
                    foreach (InfoCard card in infoCards)
                    {
                        string key = card.Title;
                        var value = new List<InfoCard>();

                        if (!detailSplit.TryGetValue(key, out value))
                            detailSplit[key] = value;
                        value.Add(card);
                    }
                }
                else
                {
                    displaySplit.Add(kvp.Value);
                }
            }

            foreach (var kvp in detailSplit)
            {
                displayCards.Add(new DisplayCard(kvp.Value));
            }
        }
    }
}
