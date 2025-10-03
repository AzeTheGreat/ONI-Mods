using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AzeLib.Attributes
{
    public class AMonoBehaviour : MonoBehaviour
    {
        private static readonly ConcurrentDictionary<Type, FieldInfo[]> MyIntGetFieldCache = new();

        internal static class DebugHooks
        {
            internal static Action<Type, int>? CachePrimed;
            internal static Action<AMonoBehaviour, FieldInfo, object?>? ComponentResolved;
        }

        /// <summary>
        /// Resolves a component instance for the provided type.  Overridable to allow
        /// deterministic substitution in tests while defaulting to the base game lookup.
        /// </summary>
        /// <param name="componentType">The component type required by a decorated field.</param>
        /// <returns>The component instance that should be assigned to the field.</returns>
        protected virtual object? ResolveComponent(Type componentType) => GetComponent(componentType);

        public void OnSpawn()
        {
            var thisType = GetType();
            var cachedFields = MyIntGetFieldCache.GetOrAdd(thisType, BuildMyIntGetFieldCache);
            DebugHooks.CachePrimed?.Invoke(thisType, cachedFields.Length);

            foreach (var fieldInfo in cachedFields)
            {
                var component = ResolveComponent(fieldInfo.FieldType);
                fieldInfo.SetValue(this, component);
                DebugHooks.ComponentResolved?.Invoke(this, fieldInfo, component);
            }
        }

        private static FieldInfo[] BuildMyIntGetFieldCache(Type type)
        {
            return type
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(field => field.IsDefined(typeof(MyIntGetAttribute), inherit: false))
                .ToArray();
        }
    }
}
