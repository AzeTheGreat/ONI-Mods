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
            static void Postfix(HoverTextDrawer __instance)
            {
                drawerInstance = __instance;
                IsInterceptMode = true;
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

                    curInfoCard.AddDraw(_ => drawerInstance.BeginShadowBar(selected));
                    curInfoCard.isSelected = selected;
                }
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int)})]
        class DrawIcon
        {
            static void Postfix(Sprite icon, Color color, int image_size, int horizontal_spacing)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(_ => drawerInstance.DrawIcon(icon, color, CardTweaker.GetNewIconSize(image_size), horizontal_spacing));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
        class DrawText
        {
            static void Postfix(string text, TextStyleSetting style, Color color, bool override_color)
            {
                if (!IsInterceptMode)
                    return;

                var (id, data) = ExportSelectToolData.ConsumeTextInfo();
                var ti = TextInfo.Create(id, text, data);
                curInfoCard.AddDraw(cards => drawerInstance.DrawText(ti.GetTextOverride(cards), style, color, override_color), ti);
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.AddIndent))]
        class AddIndent
        {
            static void Postfix(int width)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(_ => drawerInstance.AddIndent(width));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.NewLine))]
        class NewLine
        {
            static void Postfix(int min_height)
            {
                if (IsInterceptMode)
                    curInfoCard.AddDraw(_ => drawerInstance.NewLine(CardTweaker.GetNewLineHeight(min_height)));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        class EndShadowBar
        {
            static void Postfix()
            {
                if (IsInterceptMode)
                {
                    curInfoCard.AddDraw(_ => drawerInstance.EndShadowBar());
                    curInfoCard.selectable = ExportSelectToolData.ConsumeSelectable();
                }
            }
        }
    }
}
