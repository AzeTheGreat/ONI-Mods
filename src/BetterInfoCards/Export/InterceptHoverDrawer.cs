using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    static class InterceptDrawer
    {
        public static bool isInterceptMode;
        public static HoverTextDrawer drawerInstance;

        public static InfoCard curInfoCard;
        public static List<InfoCard> infoCards = new();

        public static ICWidgetData curICWidgets;
        public static List<ICWidgetData> icWidgets = new();

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
        class ProcessHoverInfo
        {
            static DisplayCards displayCardManager = new();

            static void Prefix(HoverTextDrawer __instance)
            {
                drawerInstance = __instance;

                var displayCards = displayCardManager.UpdateData(infoCards);
                ModifyHits.Update(displayCards);

                isInterceptMode = false;
                foreach (var card in displayCards)
                    card.Draw();
                isInterceptMode = true;

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
            // TODO: Can maybe just use skin.DrawnWidgets instead?
            static bool Prefix(bool selected)
            {
                if (isInterceptMode)
                    Intercept(selected);
                else
                    Draw(selected);
                return !isInterceptMode;
            }

            static void Intercept(bool selected)
            {
                curInfoCard = new();
                infoCards.Add(curInfoCard);

                var sbInfo = new DrawerInfo.BeginSB
                {
                    isSelected = selected
                };
                curInfoCard.infos.Add(sbInfo);
                curInfoCard.isSelected = selected;
            }

            static void Draw(bool selected)
            {
                curICWidgets = new();
                icWidgets.Add(curICWidgets);
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        class EndShadowBar
        {
            static bool Prefix()
            {
                if (!isInterceptMode)
                    return true;

                var sbInfo = new DrawerInfo.EndSB();
                curInfoCard.infos.Add(sbInfo);

                curInfoCard.selectable = ExportSelectToolData.curSelectable;
                ExportSelectToolData.curSelectable = null;

                return false;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new [] { typeof(Sprite), typeof(Color), typeof(int), typeof(int)})]
        class DrawIcon
        {
            static bool Prefix(Sprite icon, Color color, int image_size, int horizontal_spacing)
            {
                if (!isInterceptMode)
                    return true;

                var iconInfo = new DrawerInfo.Icon()
                {
                    icon = icon,
                    color = color,
                    imageSize = image_size,
                    xSpacing = horizontal_spacing
                };

                curInfoCard.infos.Add(iconInfo);

                return false;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
        class DrawText
        {
            static bool Prefix(string text, TextStyleSetting style, Color color, bool override_color)
            {
                if (!isInterceptMode)
                    return true;

                var (id, data) = ExportSelectToolData.curTextInfo;
 
                var textInfo = new DrawerInfo.Text()
                {
                    textInfo = TextInfo.Create(id, text, data),
                    style = style,
                    color = color,
                    overrideColor = override_color
                };

                curInfoCard.infos.Add(textInfo);
                curInfoCard.textInfos.Add(textInfo.textInfo.ID, textInfo);

                ExportSelectToolData.curTextInfo = (string.Empty, null);

                return false;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.AddIndent))]
        class AddIndent
        {
            static bool Prefix(int width)
            {
                if (!isInterceptMode)
                    return true;

                var indentInfo = new DrawerInfo.Indent
                {
                    width = width
                };

                curInfoCard.infos.Add(indentInfo);

                return false;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.NewLine))]
        class NewLine
        {
            static bool Prefix(int min_height)
            {
                if (!isInterceptMode)
                    return true;

                var newLineInfo = new DrawerInfo.NewLine
                {
                    minHeight = min_height
                };

                curInfoCard.infos.Add(newLineInfo);

                return false;
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
