using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterInfoCards.Util
{
    public static class Splitters
    {
        public static Dictionary<TKey, List<TVal>> SplitByKeyToDict<TKey, TVal>(this List<TVal> source, Func<TVal, TKey> getKey)
        {
            var splits = new Dictionary<TKey, List<TVal>>();

            foreach (var item in source)
                splits.TryAddToDict(getKey(item), item);

            return splits;
        }

        public static List<List<TVal>> SplitByKey<TKey, TVal>(this List<TVal> source, Func<TVal, TKey> getKey)
        {
            if (source.Count <= 1)
                return new() { source };
            return source.SplitByKeyToDict(getKey).Values.ToList();
        }

        public static List<List<T>> SplitMany<T>(this List<List<T>> source, Func<List<T>, List<List<T>>> getSplits)
        {
            var splits = new List<List<T>>();

            foreach (var group in source)
                if (group.Count <= 1)
                    splits.Add(group);
                else
                    splits.AddRange(getSplits(group));

            return splits;
        }

        public static List<List<T>> SplitToList<T>(this List<List<T>> source, Func<List<T>, List<List<T>>> splitter)
        {
            var splits = new List<List<T>>();

            foreach (var group in source)
                if (group.Count > 1)
                    splits.AddRange(splitter(group));
                else
                    splits.Add(group);

            return splits;
        }

        public static List<List<TVal>> SplitBySplitters<TVal, TSplit>(this List<TVal> source, List<TSplit> splitterDatas, Func<List<TVal>, TSplit, int, List<List<TVal>>> splitterFunc)
        {
            var splits = new List<List<TVal>>() { source };

            for (int i = 0; i < splitterDatas.Count; i++)
            {
                var splitterData = splitterDatas[i];
                splits = splits.SplitToList((group) => splitterFunc(group, splitterData, i));
            }

            return splits;
        }

        private static void TryAddToDict<TKey, TVal>(this Dictionary<TKey, List<TVal>> dict, TKey key, TVal item)
        {
            if (!dict.TryGetValue(key, out List<TVal> value))
                value = new List<TVal>();

            value.Add(item);
            dict[key] = value;
        }
    }
}
