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

        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            // Don't count the base class as a subclass of itself.
            if (toCheck == generic)
                return false;

            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
