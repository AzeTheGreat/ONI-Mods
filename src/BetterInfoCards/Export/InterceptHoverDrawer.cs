using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    static class InterceptHoverDrawer
    {
        public static bool IsInterceptMode { get; set; }
        public static HoverTextDrawer drawerInstance;

        private static InfoCard curInfoCard;
        private static List<InfoCard> infoCards = new();

        public static List<InfoCard> ConsumeInfoCards()
        {
            var cards = infoCards;
            infoCards = new();
            return cards;
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginDrawing))]
        public class BeginDrawing
        {
            public static System.Action onBeginDrawing;

            static void Postfix(HoverTextDrawer __instance)
            {
                drawerInstance = __instance;
                IsInterceptMode = true;
                onBeginDrawing?.Invoke();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginShadowBar))]
        class BeginShadowBar
        {
            static ResetPool<InfoCard> pool = new(ref BeginDrawing.onBeginDrawing);

            [HarmonyPriority(Priority.First)]
            static bool Prefix(bool selected)
            {
                if (IsInterceptMode)
                    infoCards.Add(curInfoCard = pool.Get().Set(selected));
                return !IsInterceptMode;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int)})]
        class DrawIcon
        {
            static ResetPool<DrawActions.Icon> pool = new(ref BeginDrawing.onBeginDrawing);

            [HarmonyPriority(Priority.First)]
            static bool Prefix(Sprite icon, Color color, int image_size, int horizontal_spacing)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(pool.Get().Set(icon, color, image_size, horizontal_spacing));
                return !IsInterceptMode;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
        class DrawText
        {
            static ResetPool<DrawActions.Text> pool = new(ref BeginDrawing.onBeginDrawing);

            [HarmonyPriority(Priority.First)]
            static bool Prefix(string text, TextStyleSetting style, Color color, bool override_color)
            {
                // Null check avoids crashes from drawing multiple empty strings.
                // This appears to now occur when hovering neutromium tiles.
                if (IsInterceptMode && !text.IsNullOrWhiteSpace())
                {
                    var (id, data) = ExportSelectToolData.ConsumeTextInfo();
                    var ti = TextInfo.Create(id, text, data);
                    curInfoCard.AddDraw(pool.Get().Set(ti, style, color, override_color), ti);
                }
                return !IsInterceptMode;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.AddIndent))]
        class AddIndent
        {
            static ResetPool<DrawActions.AddIndent> pool = new(ref BeginDrawing.onBeginDrawing);

            [HarmonyPriority(Priority.First)]
            static bool Prefix(int width)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(pool.Get().Set(width));
                return !IsInterceptMode;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.NewLine))]
        class NewLine
        {
            static ResetPool<DrawActions.NewLine> pool = new(ref BeginDrawing.onBeginDrawing);

            [HarmonyPriority(Priority.First)]
            static bool Prefix(int min_height)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(pool.Get().Set(min_height));
                return !IsInterceptMode;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        class EndShadowBar
        {
            [HarmonyPriority(Priority.First)]
            static bool Prefix()
            {
                if (IsInterceptMode)
                    curInfoCard.selectable = ExportSelectToolData.ConsumeSelectable();
                return !IsInterceptMode;
            }
        }
    }
}
