using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    public class DrawnWidgets : WidgetsBase
    {
        public static DrawnWidgets Instance { get; set; }

        public float selectPos
        {
            get
            {
                if(selectBorders.Count > 0)
                    return selectBorders[0].rect.anchoredPosition.y;
                return float.MaxValue;
            }
        }

        private float[] cachedShadowWidths = new float[0];
        private float[] cachedShadowHeights = new float[0];
        private Vector3 cachedSelectBorder = Vector3.positiveInfinity;

        private const float equalsThreshold = 0.001f;

        // This could theoretically fail if objects with the same exact shadow bar width and height were swapped in the same frame...
        public bool IsLayoutChanged()
        {
            if (shadowBars.Count != cachedShadowWidths.Length)
                return true;

            for (int i = 0; i < shadowBars.Count; i++)
            {
                Rect rect = shadowBars[i].rect.rect;

                if (!NearEquals(rect.height, cachedShadowHeights[i], equalsThreshold) || !NearEquals(rect.width, cachedShadowWidths[i], equalsThreshold))
                    return true;
            }
            return false;
        }

        // Could theoretically fail if user clicks and the layout changes in the same frame such that the new pos is identical to the old...
        public bool IsSelectedChanged()
        {
            Vector3 currentSelectBorder;
            if (selectBorders.Count > 0)
                currentSelectBorder = selectBorders[0].rect.anchoredPosition;
            else
                currentSelectBorder = Vector3.positiveInfinity;

            if (Vector3.Distance(currentSelectBorder, cachedSelectBorder) > 0.0001f)
            {
                cachedSelectBorder = currentSelectBorder;
                return true;
            }
            return false;
        }

        public void Update(List<Entry> cachedEntries, EntryType type, int drawnWidgets)
        {
            List<Entry> drawnEntries = GetEntries(type);

            if (drawnWidgets > drawnEntries.Count)
                drawnEntries.AddRange(cachedEntries.GetRange(drawnEntries.Count, drawnWidgets - drawnEntries.Count));
            if (drawnWidgets < drawnEntries.Count)
                drawnEntries.RemoveRange(drawnWidgets, drawnEntries.Count - drawnWidgets);
        }

        public List<InfoCard> FormInfoCards()
        {
            var infoCards = new List<InfoCard>();

            int iconIndex = 0;
            int textIndex = 0;

            cachedShadowWidths = new float[shadowBars.Count];
            cachedShadowHeights = new float[shadowBars.Count];

            // For each shadow bar, create an info card and add the relevant icons and texts.
            for (int i = 0; i < shadowBars.Count; i++)
            {
                Entry shadowBar = shadowBars[i];
                infoCards.Add(new InfoCard(shadowBar, ref iconIndex, ref textIndex));

                cachedShadowWidths[i] = shadowBar.rect.rect.width;
                cachedShadowHeights[i] = shadowBar.rect.rect.height;
            }

            return infoCards;
        }

        private bool NearEquals(float f1, float f2, float diff)
        {
            if (Math.Abs(f1 - f2) < diff)
                return true;
            else
                return false;
        }
    }
}
