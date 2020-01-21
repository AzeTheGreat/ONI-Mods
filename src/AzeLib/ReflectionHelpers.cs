using System;
using System.Collections.Generic;
using System.Linq;

namespace AzeLib
{
    class ReflectionHelpers
    {
        public static IEnumerable<T> GetChildInstanceForType<T>()
        {
            return typeof(T).Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract)
                    .Select(t => (T)Activator.CreateInstance(t));
        }
    }
}
