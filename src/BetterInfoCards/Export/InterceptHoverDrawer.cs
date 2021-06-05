using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    static class InterceptHoverDrawer
    {
        public static bool IsInterceptMode
        {
            get => !drawerInstance.skin.drawWidgets;
            set => drawerInstance.skin.drawWidgets = !value;
        }
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
        class BeginDrawing
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
            static void Postfix(bool selected)
            {
                if (IsInterceptMode)
                {
                    curInfoCard = new();
                    infoCards.Add(curInfoCard);

                    curInfoCard.isSelected = selected;
                }
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int)})]
        class DrawIcon
        {
            static ResetPool<DrawActions.Icon> pool = new(ref BeginDrawing.onBeginDrawing);

            static void Postfix(Sprite icon, Color color, int image_size, int horizontal_spacing)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(pool.Get().Set(icon, color, image_size, horizontal_spacing));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
        class DrawText
        {
            static ResetPool<DrawActions.Text> pool = new(ref BeginDrawing.onBeginDrawing);

            static void Postfix(string text, TextStyleSetting style, Color color, bool override_color)
            {
                if (!IsInterceptMode)
                    return;

                var (id, data) = ExportSelectToolData.ConsumeTextInfo();
                var ti = TextInfo.Create(id, text, data);
                curInfoCard.AddDraw(pool.Get().Set(ti, style, color, override_color), ti);
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.AddIndent))]
        class AddIndent
        {
            static ResetPool<DrawActions.AddIndent> pool = new(ref BeginDrawing.onBeginDrawing);

            static void Postfix(int width)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(pool.Get().Set(width));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.NewLine))]
        class NewLine
        {
            static ResetPool<DrawActions.NewLine> pool = new(ref BeginDrawing.onBeginDrawing);

            static void Postfix(int min_height)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(pool.Get().Set(min_height));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        class EndShadowBar
        {
            static void Postfix()
            {
                if (IsInterceptMode)
                    curInfoCard.selectable = ExportSelectToolData.ConsumeSelectable();
            }
        }
    }
}
