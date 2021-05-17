using BetterInfoCards.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BetterInfoCards
{
    public class DisplayCards
    {
        static Stopwatch one = new();
        static Stopwatch two = new();
        static Stopwatch three = new();
        static Stopwatch four = new();
        static Stopwatch five = new();
        static Stopwatch dispCard = new();
        static Stopwatch dispCard2 = new();

        static int runs = 1;

        public List<DisplayCard> UpdateData(List<InfoCard> infoCards)
        {
            var displayCards = new List<DisplayCard>();

            if (infoCards == null || infoCards.Count <= 0)
                return displayCards;

            if(runs > 1000)
            {
                Debug.Log("5: " + five.ElapsedTicks / runs + " ticks");
                Debug.Log("1: " + one.ElapsedTicks / runs + " ticks");
                Debug.Log("2: " + two.ElapsedTicks / runs + " ticks");
                Debug.Log("3: " + three.ElapsedTicks / runs + " ticks");
                Debug.Log("4: " + four.ElapsedTicks / runs + " ticks");
                Debug.Log("DC1: " + dispCard.ElapsedTicks / runs + " ticks");
                Debug.Log("DC2: " + dispCard2.ElapsedTicks / runs + " ticks");
                
                
                runs = 1;

                one.Reset();    // 3323
                two.Reset();    // 7600
                five.Reset();
                three.Reset();  // 1834
                four.Reset();   // 4075
                dispCard.Reset();
                dispCard2.Reset(); 
                
                Debug.Log("RESET");
            }

            five.Start();
            var dSplit = infoCards.SplitByKey(card => card.GetTitleKey());
            five.Stop();

            one.Start();
            var dSplit2 = dSplit.SplitMany(cards => cards.SplitByKey(card => card.textInfos.Count));
            one.Stop();

            two.Start();
            var dSplit3 = GetTextKeySplits(dSplit2);
            two.Stop();

            three.Start();
            var dSplit4 = GetValBandSplits(dSplit3);
            three.Stop();

            four.Start();
            foreach (var cards in dSplit4)
                displayCards.Add(new DisplayCard(cards, dispCard, dispCard2));
            four.Stop();

            runs++;
            return displayCards;


            // TODO: OPTIMIZE
            List<List<InfoCard>> GetTextKeySplits(List<List<InfoCard>> groups, List<string> history = null)
            {
                if (history is null)
                    history = new();

                return groups.SplitMany(cards =>
                {
                    // Relies on the assumption that all the cards have the same number of TextInfos.
                    var textInfo = cards.First().textInfos.FirstOrDefault(x => !history.Contains(x.Key));
                    
                    if (textInfo.Value is null)
                        return new() { cards };

                    history.Add(textInfo.Key);
                    return GetTextKeySplits(cards.SplitByKey(card => card.textInfos.ContainsKey(textInfo.Key)), history.ToList());
                });
            }

            // Assumes each InfoCard in the List has the same TextInfos.
            List<List<InfoCard>> GetValBandSplits(List<List<InfoCard>> groups, List<string> history = null)
            {
                if (history is null)
                    history = new();

                return groups.SplitMany(cards =>
                {
                    var textInfo = cards.First().textInfos.FirstOrDefault(x => !history.Contains(x.Key));

                    if (textInfo.Value is null)
                        return new() { cards };

                    history.Add(textInfo.Key);
                    return GetValBandSplits(textInfo.Value.SplitByTIDefs(cards), history.ToList());
                });
            }
        }
    }
}
