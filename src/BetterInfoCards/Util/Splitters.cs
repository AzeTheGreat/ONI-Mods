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

        public static List<List<TVal>> SplitBySplitters<TVal, TSplit>(this List<TVal> source, List<TSplit> splitters, Func<List<TVal>, TSplit, List<List<TVal>>> getSplits)
        {
            var splits = new List<List<TVal>>() { source };

            foreach (var splitter in splitters)
                splits = splits.SplitMany(x => getSplits(x, splitter));

            return splits;
        }

        private static void TryAddToDict<TKey, TVal>(this Dictionary<TKey, List<TVal>> dict, TKey key, TVal item)
        {
            if (!dict.TryGetValue(key, out var value))
                dict[key] = value = new ();
                
            value.Add(item);
        }
    }
}
