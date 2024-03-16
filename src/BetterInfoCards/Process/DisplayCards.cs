using BetterInfoCards.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class DisplayCards
    {
        public List<DisplayCard> UpdateData(List<InfoCard> infoCards)
        {
            var displayCards = new List<DisplayCard>();

            if (infoCards == null || infoCards.Count <= 0)
                return displayCards;

            List<List<InfoCard>> dSplit1 = [], dSplit2 = [], dSplit3 = [], dSplit4 = [];
            try
            {
                dSplit1 = infoCards.SplitByKey(card => card.GetTitleKey());
                dSplit2 = dSplit1.SplitMany(cards => cards.SplitByKey(card => card.textInfos.Count));
                dSplit3 = GetTextKeySplits(dSplit2);

                // Assumes each IC in the group has the same TIs (order doesn't matter).
                dSplit4 = dSplit3.SplitMany(cards => cards.SplitBySplitters(
                    cards.First().textInfos.ToList(),
                    (group, ti) => ti.Value.SplitByTIDefs(group)));
            }
            catch (Exception)
            {
                Debug.Log("---------------------------------------------");
                Debug.Log("Better Info Cards CRASH - Grouping Info Cards");
                LogCell();
                Debug.Log("---------------------------------------------");

                Debug.Log($"Info Cards");
                Debug.Log("----------------------------------------");
                LogICGroup(infoCards);

                LogDSplit(dSplit1, "1");
                LogDSplit(dSplit2, "2");
                LogDSplit(dSplit3, "3");
                LogDSplit(dSplit4, "4");

                throw;
            }

            foreach (var cards in dSplit4)
                displayCards.Add(new DisplayCard(cards));

            return displayCards;

            // Assumes that the ICs in the same group have the same number of TIs.
            // It's too intensive to check TIs beyond the first IC in a group.
            List<List<InfoCard>> GetTextKeySplits(List<List<InfoCard>> groups, List<string> history = null)
            {
                // History is usually pretty short, so List is faster here than HashSet
                // (threshold around 5 items: https://stackoverflow.com/questions/150750/hashset-vs-list-performance/)
                history ??= [];

                return groups.SplitMany(cards =>
                {
                    // ToList is necessary to force immediate evaluation instead of lazy.
                    // If it lazy evaluates, then when the TIs are added to history, they prevent anything from actually being split.
                    var textInfos = cards.First().textInfos.Keys.Except(history).ToList();
                    if (!textInfos.Any())
                        return [cards];

                    var newHistory = new List<string>(history);
                    newHistory.AddRange(textInfos);

                    return GetTextKeySplits(
                        cards.SplitBySplitters(textInfos.ToList(), (group, ti) => group.SplitByKey(card => card.textInfos.ContainsKey(ti))),
                        newHistory);
                });
            }

            void LogDSplit(List<List<InfoCard>> groups, string name)
            {
                Debug.Log($"D Split - {name}");
                Debug.Log("----------------------------------------");
                foreach (var group in groups)
                    LogICGroup(group);
            }

            void LogICGroup(List<InfoCard> group)
            {
                foreach (var ic in group)
                    ic.LogCard();
                Debug.Log("---------------");
            }

            void LogCell()
            {
                try
                {
                    var problemCell = infoCards.First(card => card.selectable?.gameObject != null).selectable.gameObject.transform?.position ?? Vector3.negativeInfinity;
                    Debug.Log($"Issue in cell: {(global::Grid.PosToCell(problemCell))} (raw pos: {problemCell}).");
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to identify problem cell:");
                    Debug.Log(e);
                }
            }
        }
    }
}
