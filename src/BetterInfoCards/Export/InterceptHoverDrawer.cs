using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    static class InterceptDrawer
    {
        public static bool IsInterceptMode
        {
            get => !drawerInstance.skin.drawWidgets;
            set => drawerInstance.skin.drawWidgets = !value;
        }
        public static HoverTextDrawer drawerInstance;

        public static InfoCard curInfoCard;
        public static List<InfoCard> infoCards = new();

        public static ICWidgetData curICWidgets;
        public static List<ICWidgetData> icWidgets = new();

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginDrawing))]
        class BeginDrawing
        {
            static void Postfix(HoverTextDrawer __instance)
            {
                drawerInstance = __instance;
                IsInterceptMode = true;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
        class ProcessHoverInfo
        {
            static void Prefix()
            {
                var displayCards = new DisplayCards().UpdateData(infoCards);
                ModifyHits.Update(displayCards);

                IsInterceptMode = false;
                foreach (var card in displayCards)
                    card.Draw();
                IsInterceptMode = true;

                if (icWidgets.Count > 0)
                {
                    var gridInfo = new GridInfo(icWidgets, icWidgets[0].YMax);
                    gridInfo.MoveAndResizeInfoCards();
                }

                icWidgets.Clear();
                infoCards.Clear();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginShadowBar))]
        class BeginShadowBar
        {
            static void Prefix(bool selected)
            {
                if (IsInterceptMode)
                    Intercept(selected);
                else
                    Draw();
            }

            static void Intercept(bool selected)
            {
                curInfoCard = new();
                infoCards.Add(curInfoCard);

                curInfoCard.infos.Add(_ => drawerInstance.BeginShadowBar(selected));
                curInfoCard.isSelected = selected;
            }

            static void Draw()
            {
                curICWidgets = new();
                icWidgets.Add(curICWidgets);
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        class EndShadowBar
        {
            static void Prefix()
            {
                if (!IsInterceptMode)
                    return;

                curInfoCard.infos.Add(_ => drawerInstance.EndShadowBar());

                curInfoCard.selectable = ExportSelectToolData.curSelectable;
                ExportSelectToolData.curSelectable = null;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new [] { typeof(Sprite), typeof(Color), typeof(int), typeof(int)})]
        class DrawIcon
        {
            static void Prefix(Sprite icon, Color color, int image_size, int horizontal_spacing)
            {
                if (!IsInterceptMode)
                    return;

                curInfoCard.infos.Add(_ => drawerInstance.DrawIcon(icon, color, image_size, horizontal_spacing));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
        class DrawText
        {
            static void Prefix(string text, TextStyleSetting style, Color color, bool override_color)
            {
                if (!IsInterceptMode)
                    return;

                var (id, data) = ExportSelectToolData.curTextInfo;
                var ti = TextInfo.Create(id, text, data);

                curInfoCard.infos.Add(cards => drawerInstance.DrawText(ti.GetTextOverride(cards), style, color, override_color));
                curInfoCard.textInfos.Add(ti.ID, ti);

                ExportSelectToolData.curTextInfo = (string.Empty, null);
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.AddIndent))]
        class AddIndent
        {
            static void Prefix(int width)
            {
                if (!IsInterceptMode)
                    return;

                curInfoCard.infos.Add(_ => drawerInstance.AddIndent(width));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.NewLine))]
        class NewLine
        {
            static void Prefix(int min_height)
            {
                if (!IsInterceptMode)
                    return;

                curInfoCard.infos.Add(_ => drawerInstance.NewLine(min_height));
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
