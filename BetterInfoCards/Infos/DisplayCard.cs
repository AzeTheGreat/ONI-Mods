using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterInfoCards
{
    public class DisplayCard
    {
        public float Width { get { return infoCards[0].Width; } }
        public float YMax { get { return infoCards[0].YMax; } }
        public float YMin { get { return infoCards[0].YMin; } }
        public string Title { get { return infoCards[0].Title; } }

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
        }

        public void Translate(float x, float y)
        {
            infoCards[0].Translate(x, y);

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

        public void Resize(float newX)
        {
            infoCards[0].Resize(newX);
        }
    }
}
