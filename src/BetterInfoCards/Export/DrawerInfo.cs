using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
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
