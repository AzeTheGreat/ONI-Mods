using HarmonyLib;
using System;
using System.Collections.Generic;
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
            // Dynamically patch all Draw methods on all generic Pool<T> types
            var hoverTextDrawerType = typeof(HoverTextDrawer);
            foreach (var nestedType in hoverTextDrawerType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (nestedType.IsGenericTypeDefinition && nestedType.Name.StartsWith("Pool"))
                {
                    var drawMethod = AccessTools.Method(nestedType, "Draw");
                    if (drawMethod != null)
                    {
                        var harmony = new Harmony("BetterInfoCards.Export.ExportWidgets");
                        var postfix = typeof(ExportWidgets).GetMethod(nameof(GetWidget_Postfix), BindingFlags.NonPublic | BindingFlags.Static);
                        harmony.Patch(drawMethod, postfix: new HarmonyMethod(postfix));
                    }
                }
            }
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
