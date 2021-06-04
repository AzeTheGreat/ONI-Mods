using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards.Export
{
    public static class ExportWidgets
    {
        private static ICWidgetData curICWidgets;
        private static List<ICWidgetData> icWidgets = new();

        public static List<ICWidgetData> ConsumeWidgets()
        {
            var widgets = icWidgets;
            icWidgets = new();
            return widgets;
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

        [HarmonyPatch(typeof(HoverTextDrawer.Pool<MonoBehaviour>), nameof(HoverTextDrawer.Pool<MonoBehaviour>.Draw))]
        class GetWidget_Patch
        {
            static void Postfix(Entry __result, GameObject ___prefab)
            {
                curICWidgets.AddWidget(__result, ___prefab);
            }
        }
    }
}
