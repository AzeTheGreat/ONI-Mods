using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterInfoCards
{
    public static class Extensions
    {
        public static void SetOrAdd<T>(this List<T> list, int index, T value)
        {
            list.ExpandToIndex(index);
            list[index] = value;
        }

        public static void ExpandToIndex<T>(this List<T> list, int index)
        {
            if (list.Count <= index)
                list.AddRange(new T[index - list.Count + 1]);
        }
    }
}
