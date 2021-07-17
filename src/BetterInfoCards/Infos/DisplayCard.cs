using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class DisplayCard
    {
        public float Width => VisCard.Width;
        public float Height => VisCard.Height;
        public float YMax => VisCard.YMax;
        public float YMin => VisCard.YMin;

        public Vector2 offset = new();

        private List<InfoCard> infoCards;
        private int visCardIndex;
        private InfoCard VisCard => infoCards[visCardIndex];

        // The HoverTextScreen is initialized before CameraController
        private float _minY = float.MaxValue;
        private float MinY => _minY == float.MaxValue ?
            _minY = -CameraController.Instance?.uiCamera.pixelRect.height / HoverTextScreen.Instance.GetComponentInParent<Canvas>().scaleFactor ?? float.MaxValue : _minY;

        public DisplayCard(List<InfoCard> infoCards)
        {
            this.infoCards = infoCards;
            
            visCardIndex = infoCards.FindIndex(x => x.selectBorder.widget != null);
            if (visCardIndex == -1)
                visCardIndex = 0;

            if(infoCards.Count > 1)
                VisCard.Rename(infoCards, visCardIndex);
        }

        public void Translate(float x)
        {
            VisCard.Translate(x, offset.y);

            foreach (var card in infoCards)
            {
                if(card != VisCard)
                {
                    if (card.YMax > MinY)
                        card.Translate(0f, MinY - card.YMax);
                }
            }
        }

        public void Resize(float newWidth)
        {
            VisCard.Resize(newWidth);
        }

        public List<KSelectable> GetAllSelectables()
        {
            return infoCards.Select(x => x.selectable).ToList();
        }
    }
}
