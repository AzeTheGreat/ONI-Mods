using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCards
    {
        private List<InfoCard> infoCards;
        private Vector3 cachedMousePos = Vector3.positiveInfinity;
        private InfoCard cachedClosestInfoCard = null;

        private DisplayCards DisplayCards;

        public InfoCards(ref float[] cachedShadowWidths, ref float[] cachedShadowHeights, List<Entry> shadowBars, List<Entry> iconWidgets, List<Entry> textWidgets)
        {
            infoCards = new List<InfoCard>();

            int iconIndex = 0;
            int textIndex = 0;

            cachedShadowWidths = new float[shadowBars.Count];
            cachedShadowHeights = new float[shadowBars.Count];

            // For each shadow bar, create an info card and add the relevant icons and texts.
            for (int i = 0; i < shadowBars.Count; i++)
            {
                Entry shadowBar = shadowBars[i];
                infoCards.Add(new InfoCard(shadowBar, iconWidgets, textWidgets, ref iconIndex, ref textIndex));

                cachedShadowWidths[i] = shadowBar.rect.rect.width;
                cachedShadowHeights[i] = shadowBar.rect.rect.height;
            }
        }

        public void Update(List<Entry> selectBorders)
        {
            if (HasMouseMovedEnough())
                DisplayCards = new DisplayCards(infoCards);
            if (selectBorders.Count > 0)
                cachedClosestInfoCard.selectBorder = selectBorders[0];

            DisplayCards.Update();
        }

        public void UpdateSelected(float borderY)
        {
            if (borderY != float.MaxValue)
            {
                InfoCard closestInfoCard = infoCards.Aggregate((x, y) => Math.Abs(x.YMax - borderY) < Math.Abs(y.YMax - borderY) ? x : y);

                if (cachedClosestInfoCard != null)
                    cachedClosestInfoCard.selectBorder = new Entry();

                cachedClosestInfoCard = closestInfoCard;
            }
        }

        private bool HasMouseMovedEnough()
        {
            Vector3 cursorPos = Input.mousePosition;

            if (cursorPos != cachedMousePos)
            {
                cachedMousePos = cursorPos;
                return true;
            }
            return false;
        }
    }
}
