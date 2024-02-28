using BetterInfoCards;
using BetterInfoCards.Util;
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

            var dSplit = infoCards.SplitByKey(card => card.GetTitleKey());
            var dSplit2 = dSplit.SplitMany(cards => cards.SplitByKey(card => card.textInfos.Count));
            var dSplit3 = GetTextKeySplits(dSplit2);

            try
            {
                var badGroups = dSplit3.Where(Logging.IsGroupBad);
                if (badGroups.Any())
                {
                    Debug.Log("---------------------------------------------");
                    Debug.Log("BAD GROUPS FOUND in dSplit3");

                    if (badGroups.FirstOrDefault()?.FirstOrDefault()?.selectable is KSelectable sel)
                        if (sel?.gameObject is GameObject go)
                            if (go?.transform?.position is Vector3 pos)
                                Debug.Log($"Issue in cell: {(global::Grid.PosToCell(pos))} (raw pos: {pos}).");
                            else
                                Debug.Log($"Failed to get position from GO {go}");
                        else
                            Debug.Log($"Failed to get GO from selectable {sel}");
                    else
                        Debug.Log("Failed to get valid selectable.");

                    Debug.Log("---------------------------------------------");
                    Debug.Log("Bad Groups Dump:");
                    foreach (var bg in badGroups)
                        Logging.LogICGroup(bg);

                    Debug.Log("---------------------------------------------");
                    Debug.Log("Full IC Dump (dSplit3):");
                    foreach (var g in dSplit3)
                        Logging.LogICGroup(g);

                    Debug.Log("---------------------------------------------");
                    Debug.Log("dSplit2:");
                    foreach (var g in dSplit2)
                        Logging.LogICGroup(g);

                    Debug.Log("---------------------------------------------");
                    Debug.Log("dSplit:");
                    foreach (var g in dSplit)
                        Logging.LogICGroup(g);

                    Debug.Log("---------------------------------------------");
                    Debug.Log("Info Cards:");
                    Logging.LogICGroup(infoCards);
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("---------------------------------------------");
                Debug.Log("CAUGHT ERROR IN LOGGING");
                Debug.Log(e);
            }

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
                    // ToList is necessary to force immediate evaluation instead of lazy.
                    // If it lazy evaluates, then when the TIs are added to history, they prevent anything from actually being split.
                    var textInfos = cards.First().textInfos.Keys.Except(history).ToList();
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

public class Logging
{
    public static void LogICGroup(List<InfoCard> group)
    {
        Debug.Log("---------------");
        foreach (var ic in group)
            ic.LogCard();
    }

    public static bool IsGroupBad(List<InfoCard> group)
    {
        var tiKeys = group.FirstOrDefault().textInfos.Keys;
        return group.Any(ic => ic.textInfos.Keys.Except(tiKeys).Any() || tiKeys.Except(ic.textInfos.Keys).Any());
    }
}
