using System;
using System.Collections.Generic;

namespace BetterInfoCards.Util
{
    public static class Splitters
    {
        public static Dictionary<TKey, List<TVal>> SplitToDict<TKey, TVal>(this List<TVal> source, Func<TVal, TKey> getKey)
        {
            var splits = new Dictionary<TKey, List<TVal>>();

            foreach (TVal item in source)
                splits.TryAddToDict(getKey(item), item);

            return splits;
        }

        public static Dictionary<TKey, List<TVal>> SplitToDict<TKey, TVal>(this Dictionary<TKey, List<TVal>> source, Func<TVal, TKey> getNewKey)
        {
            var splits = new Dictionary<TKey, List<TVal>>();

            foreach (var kvp in source)
            {
                if (kvp.Value.Count > 1)
                {
                    foreach (var item in kvp.Value)
                        splits.TryAddToDict(getNewKey(item), item);
                }
                else
                    splits[kvp.Key] = kvp.Value;
            }

            return splits;
        }

        public static List<List<TVal>> SplitToList<TKey, TVal>(this Dictionary<TKey, List<TVal>> source, Func<List<TVal>, List<List<TVal>>> splitter)
        {
            var splits = new List<List<TVal>>();

            foreach (var kvp in source)
            {
                if (kvp.Value.Count > 1)
                    splits.AddRange(splitter(kvp.Value));
                else
                    splits.Add(kvp.Value);
            }
                
            return splits;
        }

        public static List<List<T>> SplitToList<T>(this List<List<T>> source, Func<List<T>, List<List<T>>> splitter)
        {
            var splits = new List<List<T>>();

            foreach (var group in source)
                splits.AddRange(splitter(group));

            return splits;
        }

        public static List<List<TVal>> SplitBySplitters<TVal, TSplit>(this List<TVal> source, List<TSplit> splitterDatas, Func<List<TVal>, TSplit, List<List<TVal>>> splitterFunc)
        {
            var splits = new List<List<TVal>>() { source };

            foreach (var splitterData in splitterDatas)
                splits = splits.SplitToList((group) => splitterFunc(group, splitterData));

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
