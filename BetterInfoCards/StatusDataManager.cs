using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterInfoCards
{
    public class StatusDataManager
    {
        public static readonly Dictionary<string, StatusData> statusConverter = new Dictionary<string, StatusData>()
        {
            { Db.Get().MiscStatusItems.OreMass.Name, new StatusData() {
                getStatusValue = go => ((GameObject)go).GetComponent<PrimaryElement>().Mass,
                getTextOverride = (name, masses) => "Net: " + name.Replace("{Mass}", GameUtil.GetFormattedMass((masses.Cast<float>()).Sum())),
                //getSplitLists = (masses, infoCards) => SplitList(infoCards, GetSplitIndices(masses.Cast<float>().ToList(), 10f))
                getSplitLists = (masses, infoCards) => new List<List<InfoCard>> {infoCards}
            } }
        };

        private static List<int> GetSplitIndices(List<float> values, float maxVariance)
        {
            //var sortedValues = new List<float>(values);
            //sortedValues.Sort();

            //for (int i = 1; i < sortedValues.Count; i++)
            //{

            //}

            return null;
        }

        private static List<List<InfoCard>> SplitList(List<InfoCard> list, List<int> splitIndices)
        {
            return null;
        }

        public class StatusData
        {
            public Func<object, ValueType> getStatusValue;
            public Func<string, List<ValueType>, string> getTextOverride;
            public Func<List<ValueType>, List<InfoCard>, List<List<InfoCard>>> getSplitLists;
        }
    }
}
