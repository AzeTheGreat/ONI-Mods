using System;
using System.Collections.Generic;
using System.Linq;

namespace AzeLib
{
    static class ReflectionHelpers
    {
        public static IEnumerable<T> CreateAndGetInstances<T>(this IEnumerable<Type> types) => types.Select(x => (T)Activator.CreateInstance(x));

        public static IEnumerable<Type> GetChildTypesOfType<T>() => typeof(T).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(T)) && !x.IsAbstract);
        public static IEnumerable<Type> GetChildTypesOfGenericType(Type t) => t.Assembly.GetTypes().Where(x => x.IsSubclassOfRawGeneric(t));

        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            // Don't count the base class as a subclass of itself.
            if (toCheck == generic)
                return false;

            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
