using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards.Export
{
    public static class ExportWidgets
    {
        private static InfoCardWidgets curICWidgets;
        private static List<InfoCardWidgets> icWidgets = new();

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

        [HarmonyPatch(typeof(HoverTextDrawer.Pool<MonoBehaviour>), nameof(HoverTextDrawer.Pool<MonoBehaviour>.Draw))]
        class GetWidget_Patch
        {
            static void Postfix(HoverTextDrawer.Pool<MonoBehaviour>.Entry __result, GameObject ___prefab)
            {
                curICWidgets.AddWidget(__result, ___prefab);
            }
        }
    }
}
