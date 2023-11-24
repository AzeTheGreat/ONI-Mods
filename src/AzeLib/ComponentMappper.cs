using AzeLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AzeLib
{
    public class ComponentMapper : ComponentMapper<object>
    {
        /// <inheritdoc cref="ComponentMapper{T}"/>
        public ComponentMapper(List<(Type flagCmp, Type addCmp)> map) : base(map.Select(x => (x.flagCmp, x.addCmp, (object)null)).ToList())
        { }

        /// <inheritdoc cref="ComponentMapper{T}.ApplyMap(GameObject, Func{T, bool})"/>
        public void ApplyMap(GameObject go) => ApplyMap(go, _ => true);
    }

    public class ComponentMapper<T>
    {
        private readonly List<(Type flagCmp, Type addCmp, T filter)> map;

        /// <summary>
        /// Maps detected components to new components that will be added.
        /// Processed first to last, so interface fallbacks should be after specific implementations.
        /// </summary>
        public ComponentMapper(List<(Type flagCmp, Type addCmp, T filter)> map) => this.map = map;

        /// <summary>
        /// Apply component map to GO, adding new components if applicable.
        /// </summary>
        /// <param name="go">The GameObject to work on</param>
        /// <param name="shouldAdd">Function uses filter to determine if the new component should be added to the GO</param>
        public void ApplyMap(GameObject go, Func<T, bool> shouldAdd)
        {
            var typeToAdd = GetTypeToAdd(go, shouldAdd);
            if (typeToAdd != null)
                go.AddComponent(typeToAdd);
        }

        private Type GetTypeToAdd(GameObject go, Func<T, bool> shouldAdd)
        {
            foreach (var (flagCmp, addCmp, filter) in map)
                if (flagCmp != null && HasComponentOrDef(flagCmp, go) && shouldAdd(filter))
                    return addCmp;
            return null;

            bool HasComponentOrDef(Type cmpOrDef, GameObject go) => go.GetComponent(cmpOrDef) ?? go.GetDef(cmpOrDef) != null;
        }
    }
}
