using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards.Export
{
    public static class ExportWidgets
    {
        private static InfoCardWidgets curICWidgets;
        private static List<InfoCardWidgets> icWidgets = new();

        static ExportWidgets()
        {
            var hoverTextDrawerType = typeof(HoverTextDrawer);
            var poolType = FindWidgetPoolType(hoverTextDrawerType, out var entryType);
            if (poolType == null)
            {
                if (entryType == null)
                    Debug.LogWarning("[BetterInfoCards] Unable to locate HoverTextDrawer widget entry type; skipping widget export patch.");
                else
                    Debug.LogWarning("[BetterInfoCards] Unable to locate HoverTextDrawer widget pool; skipping widget export patch.");
                return;
            }

            var drawMethod = AccessTools.Method(poolType, "Draw");
            if (drawMethod == null)
            {
                Debug.LogWarning($"[BetterInfoCards] Unable to locate Draw() on '{poolType.FullName}'; skipping widget export patch.");
                return;
            }

            var postfix = AccessTools.Method(typeof(ExportWidgets), nameof(GetWidget_Postfix));
            if (postfix == null)
            {
                Debug.LogWarning("[BetterInfoCards] Unable to locate ExportWidgets.GetWidget_Postfix; skipping widget export patch.");
                return;
            }

            var harmony = new Harmony("BetterInfoCards.Export.ExportWidgets");
            harmony.Patch(drawMethod, postfix: new HarmonyMethod(postfix));
        }

        private static Type FindWidgetPoolType(Type hoverTextDrawerType, out Type entryType)
        {
            entryType = FindWidgetEntryType(hoverTextDrawerType);
            if (entryType == null)
                return null;

            foreach (var nestedType in hoverTextDrawerType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (!nestedType.IsGenericTypeDefinition)
                    continue;

                if (nestedType.GetGenericArguments().Length != 1)
                    continue;

                if (!IsPoolTypeName(nestedType))
                    continue;

                try
                {
                    var constructed = nestedType.MakeGenericType(entryType);
                    if (HasRectTransformEntry(constructed))
                        return constructed;
                }
                catch (ArgumentException)
                {
                    // Ignore incompatible generic definitions.
                }
            }

            const BindingFlags memberFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            foreach (var field in hoverTextDrawerType.GetFields(memberFlags))
            {
                var poolType = field.FieldType;
                if (IsPoolType(poolType) && HasRectTransformEntry(poolType))
                    return poolType;
            }

            foreach (var property in hoverTextDrawerType.GetProperties(memberFlags))
            {
                var poolType = property.PropertyType;
                if (IsPoolType(poolType) && HasRectTransformEntry(poolType))
                    return poolType;
            }

            return null;
        }

        private static bool IsPoolType(Type type)
        {
            if (type == null)
                return false;

            var definition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            return IsPoolTypeName(definition);
        }

        private static bool IsPoolTypeName(Type type)
        {
            return type != null && type.Name.StartsWith("Pool", StringComparison.Ordinal);
        }

        private static Type FindWidgetEntryType(Type hoverTextDrawerType)
        {
            return FindWidgetEntryTypeRecursive(hoverTextDrawerType);
        }

        private static Type FindWidgetEntryTypeRecursive(Type declaringType)
        {
            if (declaringType == null)
                return null;

            const BindingFlags nestedFlags = BindingFlags.NonPublic | BindingFlags.Public;
            foreach (var nestedType in declaringType.GetNestedTypes(nestedFlags))
            {
                if (nestedType.IsValueType && !string.Equals(nestedType.Name, "Entry", StringComparison.Ordinal) && HasRectMember(nestedType))
                    return nestedType;

                var child = FindWidgetEntryTypeRecursive(nestedType);
                if (child != null)
                    return child;
            }

            return null;
        }

        private static bool HasRectMember(Type type)
        {
            const BindingFlags memberFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var rectField = type.GetField("rect", memberFlags);
            if (rectField != null && typeof(RectTransform).IsAssignableFrom(rectField.FieldType))
                return true;

            var rectProperty = type.GetProperty("rect", memberFlags);
            if (rectProperty != null && typeof(RectTransform).IsAssignableFrom(rectProperty.PropertyType))
                return true;

            return false;
        }

        private static bool HasRectTransformEntry(Type poolType)
        {
            var entryType = poolType.GetNestedType("Entry", BindingFlags.NonPublic | BindingFlags.Public);
            if (entryType == null)
                return false;

            if (entryType
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(field => typeof(RectTransform).IsAssignableFrom(field.FieldType)))
            {
                return true;
            }

            if (entryType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(property => typeof(RectTransform).IsAssignableFrom(property.PropertyType)))
            {
                return true;
            }

            return false;
        }

        public static List<InfoCardWidgets> ConsumeWidgets()
        {
            var cardWidgets = icWidgets;
            icWidgets = new();
            return cardWidgets;
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginDrawing))]
        class OnBeginDrawing
        {
            static void Postfix()
            {
                icWidgets.Clear();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginShadowBar))]
        class OnBeginShadowBar
        {
            static void Postfix()
            {
                if (!InterceptHoverDrawer.IsInterceptMode)
                {
                    curICWidgets = new();
                    icWidgets.Add(curICWidgets);
                }
            }
        }

        // This method will be used as a postfix for the Draw method via reflection.
        private static void GetWidget_Postfix(object __result, object ___prefab)
        {
            // Use reflection to access the Entry struct and its fields, since Pool<T> is private
            if (__result != null && ___prefab is GameObject prefab)
            {
                var entryType = __result.GetType();
                if (entryType.Name == "Entry")
                {
                    curICWidgets.AddWidget(__result, prefab);
                }
            }
        }
    }
}
