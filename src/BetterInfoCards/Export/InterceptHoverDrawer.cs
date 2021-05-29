using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    static class InterceptDrawer
    {
        public static bool isInterceptMode;
        public static InfoCard curInfoCard;
        public static List<InfoCard> infoCards = new();
        public static HoverTextDrawer drawerInstance;

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
        class ProcessHoverInfo
        {
            static DisplayCards displayCardManager = new();

            static void Prefix(HoverTextDrawer __instance)
            {
                drawerInstance = __instance;

                isInterceptMode = false;

                var displayCards = displayCardManager.UpdateData(infoCards);
                ModifyHits.Update(displayCards);

                foreach (var card in displayCards)
                {
                    card.Draw();
                }


                //if (displayCards.Count == 0)
                //    return;
                //var gridInfo = new GridInfo(displayCards, Instance.infoCards[0].YMax);
                //gridInfo.MoveAndResizeInfoCards();

                infoCards.Clear();

                isInterceptMode = true;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginShadowBar))]
        class BeginShadowBar
        {
            static bool Prefix(bool selected)
            {
                if (!isInterceptMode)
                    return true;

                curInfoCard = new();

                var sbInfo = new DrawerInfo.BeginSB
                {
                    isSelected = selected
                };
                curInfoCard.infos.Add(sbInfo);
                curInfoCard.isSelected = selected;

                return false;
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
                infoCards.Add(curInfoCard);

                ExportSelectToolData.curSelectable = null;
                curInfoCard = null;

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
    }

    public class InfoCardData
    {
        public bool isSelected;
        public KSelectable selectable;
        public List<DrawerInfo> infos = new();
        public Dictionary<string, DrawerInfo.Text> textInfos = new();

        public void Draw()
        {
            foreach (var info in infos)
            {
                info.Draw(null);
            }
        }
    }

    public abstract class DrawerInfo
    {
        public abstract void Draw(List<InfoCard> cards);

        HoverTextDrawer Drawer => InterceptDrawer.drawerInstance;

        public class Icon : DrawerInfo
        {
            public Sprite icon;
            public Color color;
            public int imageSize;
            public int xSpacing;

            public override void Draw(List<InfoCard> _) => Drawer.DrawIcon(icon, color, imageSize, xSpacing);
        }

        public class Text : DrawerInfo
        {
            public TextInfo textInfo;
            public TextStyleSetting style;
            public Color color;
            public bool overrideColor;

            // TODO: Change text to get override text.
            public override void Draw(List<InfoCard> cards) => Drawer.DrawText(textInfo.GetTextOverride(cards), style, color, overrideColor);
        }

        public class Indent : DrawerInfo
        {
            public int width;
            public override void Draw(List<InfoCard> _) => Drawer.AddIndent(width);
        }

        public class NewLine : DrawerInfo
        {
            public int minHeight;
            public override void Draw(List<InfoCard> _) => Drawer.NewLine(minHeight);
        }

        public class BeginSB : DrawerInfo
        {
            public bool isSelected;
            public override void Draw(List<InfoCard> _) => Drawer.BeginShadowBar(isSelected);
        }

        public class EndSB : DrawerInfo
        {
            public override void Draw(List<InfoCard> _) => Drawer.EndShadowBar();
        }
    }
}
