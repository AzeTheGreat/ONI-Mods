using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class DisplayCard
    {
        public float Width { get { return infoCards[0].Width; } }
        public float Height { get { return infoCards[0].Height; } }
        public float YMax { get { return infoCards[0].YMax; } }
        public float YMin { get { return infoCards[0].YMin; } }

        private string titleOverride = string.Empty;

        public Vector2 offset = new Vector2();

        private List<InfoCard> infoCards;

        // The HoverTextScreen is initialized before CameraController
        private float _minY = float.MaxValue;
        private float MinY
        {
            get
            {
                if (_minY == float.MaxValue)
                { 
                    _minY = -CameraController.Instance?.uiCamera.pixelRect.height / HoverTextScreen.Instance.GetComponentInParent<Canvas>().scaleFactor ?? float.MaxValue;
                } 
                return _minY;
            }
        }

        public DisplayCard(List<InfoCard> infoCards)
        {
            this.infoCards = infoCards;

            int sum = infoCards.Sum(x => x.quantity);
            if (sum > 1)
                titleOverride = infoCards[0].Title + " x " + sum;
            else
                titleOverride = infoCards[0].Title;

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
            if(titleOverride != string.Empty)
                infoCards[0].Title = titleOverride;
        }

        public void Resize(float newX)
        {
            infoCards[0].Resize(newX);
        }

        public int TopCardIndex(List<InfoCard> cards)
        {
            return cards.IndexOf(infoCards[0]);
        }
    }
}
