using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class DisplayCard
    {
        public float Width => VisCard.Width + widthDelta;
        public float Height => VisCard.Height;
        public float YMax => VisCard.YMax;
        public float YMin => VisCard.YMin;

        public Vector2 offset = new();

        private float widthDelta = 0f;
        private List<InfoCard> infoCards;
        private int visCardIndex;
        private InfoCard VisCard => infoCards[visCardIndex];

        // The HoverTextScreen is initialized before CameraController
        private float _minY = float.MaxValue;
        private float MinY => _minY == float.MaxValue ?
            _minY = -CameraController.Instance?.uiCamera.pixelRect.height / HoverTextScreen.Instance.GetComponentInParent<Canvas>().scaleFactor ?? float.MaxValue : _minY;

        public DisplayCard(List<InfoCard> infoCards, Stopwatch sw1, Stopwatch sw2)
        {
            this.infoCards = infoCards;
            
            visCardIndex = infoCards.FindIndex(x => x.selectBorder.widget != null);
            if (visCardIndex == -1)
                visCardIndex = 0;
            
            
            if(infoCards.Count > 1)
            {
                sw1.Start();
                VisCard.Rename(infoCards, visCardIndex);
                sw1.Stop();
                sw2.Start();
                widthDelta = VisCard.GetWidthDelta();
                sw2.Stop();
            }
            
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
