using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    public class DisplayCard
    {
        public float Width { get { return infoCards[0].Width + widthDelta; } }
        public float Height { get { return infoCards[0].Height; } }
        public float YMax { get { return infoCards[0].YMax; } }
        public float YMin { get { return infoCards[0].YMin; } }

        private List<string> textOverrides = new List<string>();
        float widthDelta = 0f;

        public Vector2 offset = new Vector2();

        private List<InfoCard> infoCards;

        // The HoverTextScreen is initialized before CameraController
        private float _minY = float.MaxValue;
        private float MinY
        {
            get
            {
                if (_minY == float.MaxValue)
                    _minY = -CameraController.Instance?.uiCamera.pixelRect.height / HoverTextScreen.Instance.GetComponentInParent<Canvas>().scaleFactor ?? float.MaxValue;
                return _minY;
            }
        }

        public DisplayCard(List<InfoCard> infoCards)
        {
            this.infoCards = infoCards;

            if(infoCards.Count > 1)
            {
                textOverrides = infoCards[0].GetTextOverrides(infoCards);
                widthDelta = infoCards[0].GetWidthDelta(textOverrides);
            }  
        }

        public void Translate(float x)
        {
            infoCards[0].Translate(x, offset.y);

            for (int i = 1; i < infoCards.Count; i++)
            {
                InfoCard card = infoCards[i];

                // Push the duplicate cards off the screen, break iteration if they're already off since cards are ordered top to bottom
                if (card.YMax > MinY)
                    card.Translate(0f, MinY - card.YMax);
                else
                    break;
            }
        }

        public void Rename()
        {
            if (infoCards.Count > 1)
                infoCards[0].Rename(textOverrides);
        }

        public void Resize(float newWidth)
        {
            infoCards[0].Resize(newWidth);
        }

        public int TopCardIndex(List<InfoCard> cards)
        {
            int num = cards.IndexOf(infoCards[0]);
            if (DetectRunStart_Patch.isUnreachableCard)
                return num - 1;
            else
                return num;
        }
    }
}
