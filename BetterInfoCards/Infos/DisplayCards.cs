using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterInfoCards
{
    public class DisplayCards
    {
        private List<DisplayCard> displayCards = new List<DisplayCard>();
        private GridInfo gridInfo;

        public DisplayCards(List<InfoCard> infoCards)
        {
            var nameSplit = new Dictionary<string, List<InfoCard>>();
            var displaySplit = new List<List<InfoCard>>();
            var detailSplit = new Dictionary<string, List<InfoCard>>();

            foreach (InfoCard card in infoCards)
            {
                string key = card.Title;
                if (!nameSplit.TryGetValue(key, out List<InfoCard> value))
                    value = new List<InfoCard>();
                value.Add(card);
                nameSplit[key] = value;
            }

            foreach (var kvp in nameSplit)
            {
                // If there are multiple infoCards with the same title, consider stacking them
                if(kvp.Value.Count > 1)
                {
                    foreach (InfoCard card in infoCards)
                    {
                        string key = card.Title;
                        if (!detailSplit.TryGetValue(key, out List<InfoCard> value))
                            value = new List<InfoCard>();
                        value.Add(card);
                        detailSplit[key] = value;
                    }
                }
                else
                {
                    displaySplit.Add(kvp.Value);
                }
            }

            ModifyHits.Instance.indexRedirect.Clear();
            // Not entirely sure why this isn't reset by the game's code...
            ModifyHits.Instance.localIndex = 0;

            foreach (var kvp in nameSplit)
            {
                DisplayCard card = new DisplayCard(kvp.Value);
                displayCards.Add(card);
                ModifyHits.Instance.indexRedirect.Add(card.TopCardIndex(infoCards));
            }

            gridInfo = new GridInfo(displayCards, infoCards[0].YMax);
        }

        public void Update()
        {
            gridInfo.MoveAndResizeInfoCards();
        }
    }
}
