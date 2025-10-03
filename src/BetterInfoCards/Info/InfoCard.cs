using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCard
    {
        public bool isSelected;
        public KSelectable selectable;
        public Dictionary<string, TextInfo> textInfos = [];

        private List<DrawActions> drawActions = [];
        private (int drawIndex, TextInfo ti, TextStyleSetting style) titleDrawer;

        public InfoCard Set(bool isSelected)
        {
            this.isSelected = isSelected;
            selectable = null;
            textInfos.Clear();
            drawActions.Clear();
            titleDrawer = (default, null, null);
            return this;
        }

        public void LogCard()
        {
            Debug.Log("  " + GetTitleKey() + "; " + selectable);
            foreach (var kvp in textInfos)
                Debug.Log("     " + kvp.Key + "; " + kvp.Value.ID + ", " + kvp.Value.Text);
        }

        public string GetTitleKey() => titleDrawer.ti?.Text.RemoveCountSuffix() ?? string.Empty;

        public void Draw(List<InfoCard> cards, int visCardIndex)
        {
            if (visCardIndex > 0 && titleDrawer.style != null)
            {
                var ti = TextInfo.Create(string.Empty, " #" + (++visCardIndex), null);
                var drawCount = new DrawActions.Text().Set(ti, titleDrawer.style, Color.white, false);
                drawActions.Insert(++titleDrawer.drawIndex, drawCount);
            }

            InterceptHoverDrawer.drawerInstance.BeginShadowBar(isSelected);

            foreach (var info in drawActions)
                info.Draw(cards);

            InterceptHoverDrawer.drawerInstance.EndShadowBar();
        }

        public void AddDraw(DrawActions drawAction) => drawActions.Add(drawAction);

        public void AddDraw(DrawActions drawAction, TextInfo ti)
        {
            if (titleDrawer.ti == null)
            {
                var textDraw = drawAction as DrawActions.Text;
                var style = textDraw?.Style;
                titleDrawer = (drawActions.Count, ti, style);
            }

            // This can allow multiple draw actions to exist for the same text info.
            // Because Klei cannot be trusted to prevent duplcate lines on info cards, this is necessary.
            textInfos[ti.ID] = ti;
            AddDraw(drawAction);
        }
    }
}
