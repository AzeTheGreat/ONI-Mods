using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCard
    {
        public bool isSelected;
        public KSelectable selectable;
        public Dictionary<string, TextInfo> textInfos = new();
        
        private List<DrawActions> drawActions = new();
        private (int drawIndex, TextInfo ti) titleDrawer;

        public InfoCard Set(bool isSelected)
        {
            this.isSelected = isSelected;
            selectable = null;
            textInfos.Clear();
            drawActions.Clear();
            titleDrawer = (default, null);
            return this;
        }

        public string GetTitleKey() => titleDrawer.ti?.Text.RemoveCountSuffix() ?? string.Empty;

        public void Draw(List<InfoCard> cards, int visCardIndex)
        {
            if (visCardIndex > 0)
            {
                // Getting the style like this is not ideal since it could potentially be different from the title's.
                // It does not appear to be an issue under current game conditions though.
                var ti = TextInfo.Create(string.Empty, " #" + (++visCardIndex), null);
                var drawCount = new DrawActions.Text().Set(ti, SelectTool.Instance.hoverTextConfiguration.Styles_Title.Standard, Color.white, false);
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
                titleDrawer = (drawActions.Count, ti);

            // This can allow multiple draw actions to exist for the same text info.
            // Because Klei cannot be trusted to prevent duplcate lines on info cards, this is necessary.
            textInfos[ti.ID] = ti;
            AddDraw(drawAction);
        }
    }
}
