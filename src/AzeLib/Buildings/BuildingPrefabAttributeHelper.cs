using AzeLib.Attributes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AzeLib.Buildings
{
    /// <summary>
    /// Discovers <see cref="ApplyToBuildingPrefabsAttribute"/> decorations and wires them into a
    /// Harmony postfix on <see cref="global::GeneratedBuildings.LoadGeneratedBuildings"/> so that
    /// decorated methods run automatically for each generated building prefab.
    /// </summary>
    internal static class BuildingPrefabAttributeHelper
    {
        private static readonly List<Handler> Handlers = new();
        private static bool handlersInitialized;
        private static bool isPatched;

        [OnLoad]
        internal static void Initialize(Harmony harmony)
        {
            if (harmony == null)
                throw new ArgumentNullException(nameof(harmony));

            if (isPatched)
                return;

            var target = AccessTools.Method(typeof(global::GeneratedBuildings), nameof(global::GeneratedBuildings.LoadGeneratedBuildings));
            if (target == null)
            {
                Debug.LogWarning("DefaultBuildingSettings: Failed to locate GeneratedBuildings.LoadGeneratedBuildings for prefab helpers.");
                return;
            }

            harmony.Patch(target, postfix: new HarmonyMethod(typeof(BuildingPrefabAttributeHelper), nameof(OnGeneratedBuildingsLoaded)));
            isPatched = true;
        }

        private static void OnGeneratedBuildingsLoaded()
        {
            EnsureHandlers();

            foreach (var def in global::Assets.BuildingDefs)
            {
                Process(def);
            }
        }

        internal static void Process(object buildingDef)
        {
            if (buildingDef == null)
                return;

            EnsureHandlers();

            foreach (var handler in Handlers)
            {
                if (!handler.CanInvoke(buildingDef))
                    continue;

                try
                {
                    handler.Invoke(buildingDef);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to run building prefab helper {handler.DisplayName}: {ex}");
                }
            }
        }

        internal static void ResetForTests()
        {
            Handlers.Clear();
            handlersInitialized = false;
        }

        private static void EnsureHandlers()
        {
            if (handlersInitialized)
                return;

            handlersInitialized = true;

            foreach (var (method, attribute) in DiscoverDecoratedMethods())
            {
                if (Handler.TryCreate(method, attribute, out var handler))
                    Handlers.Add(handler);
            }
        }

        private static IEnumerable<(MethodInfo method, ApplyToBuildingPrefabsAttribute attribute)> DiscoverDecoratedMethods()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic)
                    continue;

                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray()!;
                }

                foreach (var type in types)
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        foreach (var attribute in method.GetCustomAttributes<ApplyToBuildingPrefabsAttribute>())
                        {
                            yield return (method, attribute);
                        }
                    }
                }
            }
        }

        private sealed class Handler
        {
            private static readonly Type BuildingDefType = AccessTools.TypeByName("BuildingDef");

            private readonly MethodInfo method;
            private readonly Func<object, object[]> argumentFactory;
            private readonly Func<object, bool> filter;

            private Handler(MethodInfo method, Func<object, object[]> argumentFactory, Func<object, bool> filter)
            {
                this.method = method;
                this.argumentFactory = argumentFactory;
                this.filter = filter;
            }

            public string DisplayName => $"{method.DeclaringType?.FullName ?? "<global>"}.{method.Name}";

            public bool CanInvoke(object buildingDef) => filter(buildingDef);

            public void Invoke(object buildingDef)
            {
                var args = argumentFactory(buildingDef);
                method.Invoke(null, args);
            }

            public static bool TryCreate(MethodInfo method, ApplyToBuildingPrefabsAttribute attribute, out Handler handler)
            {
                handler = null;

                if (method == null)
                    return false;

                if (!method.IsStatic || method.ReturnType != typeof(void))
                    return false;

                var argumentFactory = BuildArgumentFactory(method);
                if (argumentFactory == null)
                    return false;

                var filter = BuildFilter(attribute);
                handler = new Handler(method, argumentFactory, filter);
                return true;
            }

            private static Func<object, object[]> BuildArgumentFactory(MethodInfo method)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 0)
                    return _ => Array.Empty<object>();

                var delegates = new Func<object, object>[parameters.Length];
                for (var i = 0; i < parameters.Length; i++)
                {
                    delegates[i] = BuildParameterAccessor(parameters[i]);
                    if (delegates[i] == null)
                        return null;
                }

                return def => delegates.Select(accessor => accessor(def)).ToArray();
            }

            private static Func<object, object> BuildParameterAccessor(ParameterInfo parameter)
            {
                var parameterType = parameter.ParameterType;

                if (BuildingDefType != null && parameterType.IsAssignableFrom(BuildingDefType))
                    return def => def;

                if (typeof(GameObject).IsAssignableFrom(parameterType))
                    return GetBuildingComplete;

                if (parameterType == typeof(object))
                    return def => def;

                Debug.LogWarning($"ApplyToBuildingPrefabsAttribute: Unsupported parameter type '{parameterType}' on method '{parameter.Member.DeclaringType?.FullName}.{parameter.Member.Name}'.");
                return null;
            }

            private static object GetBuildingComplete(object buildingDef)
            {
                if (buildingDef == null)
                    return null;

                var property = buildingDef.GetType().GetProperty("BuildingComplete", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                return property?.GetValue(buildingDef);
            }

            private static Func<object, bool> BuildFilter(ApplyToBuildingPrefabsAttribute attribute)
            {
                if (attribute?.BuildingIds == null || attribute.BuildingIds.Length == 0)
                    return _ => true;

                var filterSet = new HashSet<string>(attribute.BuildingIds, StringComparer.OrdinalIgnoreCase);
                return def =>
                {
                    var prefabId = GetPrefabId(def);
                    return prefabId != null && filterSet.Contains(prefabId);
                };
            }

            private static string GetPrefabId(object buildingDef)
            {
                if (buildingDef == null)
                    return null;

                var property = buildingDef.GetType().GetProperty("PrefabID", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var prefabId = property?.GetValue(buildingDef);
                if (prefabId == null)
                    return null;

                var nameProperty = prefabId.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
                if (nameProperty?.GetValue(prefabId) is string name && !string.IsNullOrWhiteSpace(name))
                    return name;

                return prefabId.ToString();
            }
        }
    }
}
