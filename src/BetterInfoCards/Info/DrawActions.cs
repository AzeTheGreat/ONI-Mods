using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    // These can not be structs with an interface.
    // The interface would cause boxing, making performance even worse.
    public abstract class DrawActions
    {
        public abstract void Draw(List<InfoCard> cards);

        public class Text : DrawActions
        {
            TextInfo ti; 
            TextStyleSetting style;
            Color color;
            bool overrideColor;

            public Text Set(TextInfo ti, TextStyleSetting style, Color color, bool overrideColor)
            {
                this.ti = ti;
                this.style = style;
                this.color = color;
                this.overrideColor = overrideColor;
                return this;
            }

            public override void Draw(List<InfoCard> cards)
            {
                InterceptHoverDrawer.drawerInstance.DrawText(ti.GetTextOverride(cards), style, color, overrideColor);
            }
        }

        public class Icon : DrawActions
        {
            Sprite icon;
            Color color;
            int imageSize;
            int horizontalSpacing;

            public Icon Set(Sprite icon, Color color, int imageSize, int horizontalSpacing)
            {
                this.icon = icon;
                this.color = color;
                this.imageSize = imageSize;
                this.horizontalSpacing = horizontalSpacing;
                return this;
            }

            public override void Draw(List<InfoCard> _)
            {
                InterceptHoverDrawer.drawerInstance.DrawIcon(icon, color, imageSize, horizontalSpacing);
            }
        }

        public class AddIndent : DrawActions
        {
            int width;

            public AddIndent Set(int width)
            {
                this.width = width;
                return this;
            }

            public override void Draw(List<InfoCard> _)
            {
                InterceptHoverDrawer.drawerInstance.AddIndent(width);
            }
        }

        public class NewLine : DrawActions
        {
            int minHeight;

            public NewLine Set(int minHeight)
            {
                this.minHeight = minHeight;
                return this;
            }

            public override void Draw(List<InfoCard> _)
            {
                InterceptHoverDrawer.drawerInstance.NewLine(minHeight);
            }
        }
    }
}
