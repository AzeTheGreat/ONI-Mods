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
            var detailSplit = new Dictionary<string, List<InfoCard>>();
            var displaySplit = new List<List<InfoCard>>();

            foreach (InfoCard card in infoCards)
            {
                string key = card.GetTitleKey();
                if (!nameSplit.TryGetValue(key, out List<InfoCard> value))
                    value = new List<InfoCard>();
                value.Add(card);
                nameSplit[key] = value;
            }

            foreach (var kvp in nameSplit)
            {
                if (kvp.Value.Count > 1)
                {
                    foreach (InfoCard card in kvp.Value)
                    {
                        string key = card.GetTitleKey() + card.GetTextKey();
                        if (!detailSplit.TryGetValue(key, out List<InfoCard> value))
                            value = new List<InfoCard>();
                        value.Add(card);
                        detailSplit[key] = value;
                    }
                }
                else
                    detailSplit[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in detailSplit)
            {
                if (kvp.Value.Count > 1)
                {
                    foreach (InfoCard card in kvp.Value)
                    {
                        card.FormTextValues();
                    }

                    var splits = new List<List<InfoCard>>
                    {
                        kvp.Value
                    };

                    // Split into the fewest display cards possible while preserving information
                    foreach (var textValue in kvp.Value[0].textValues)
                    {
                        string name = textValue.Key;
                        var converter = StatusDataManager.statusConverter[name];

                        var newSplits = new List<List<InfoCard>>();

                        foreach (List<InfoCard> split in splits)
                        {
                            //List<ValueType> values = split.Select(x => x.textValues[name]).ToList();
                            newSplits.AddRange(converter.GetSplitLists(split));
                        }

                        splits = newSplits;
                    }
                    displaySplit.AddRange(splits);
                }
                else
                    displaySplit.Add(kvp.Value);
            }

            var redirects = new List<int>();

            bool isFirst = true;
            foreach (List<InfoCard> cards in displaySplit)
            {
                DisplayCard card = new DisplayCard(cards);
                displayCards.Add(card);

                if(!(DetectRunStart_Patch.isUnreachableCard && isFirst))
                    redirects.Add(card.TopCardIndex(infoCards));

                isFirst = false;
            }

            ModifyHits.Instance.Reset(redirects);
            gridInfo = new GridInfo(displayCards, infoCards[0].YMax);
        }

        public void Update()
        {
            gridInfo.MoveAndResizeInfoCards();
        }
    }
}
