using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class DisplayCard
    {
        public float Width { get { return VisCard.Width + widthDelta; } }
        public float Height { get { return VisCard.Height; } }
        public float YMax { get { return VisCard.YMax; } }
        public float YMin { get { return VisCard.YMin; } }

        private List<string> textOverrides = new List<string>();
        float widthDelta = 0f;

        public Vector2 offset = new Vector2();

        private List<InfoCard> infoCards;
        private int visCardIndex = 0;
        private InfoCard VisCard { get { return infoCards[visCardIndex]; } }

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

            visCardIndex = infoCards.FindIndex(x => x.selectBorder.widget != null);
            if (visCardIndex == -1)
                visCardIndex = 0;

            if(infoCards.Count > 1)
            {
                textOverrides = VisCard.GetTextOverrides(infoCards);
                if (visCardIndex > 0)
                    textOverrides[0] += " #" + (visCardIndex + 1);
                widthDelta = VisCard.GetWidthDelta(textOverrides);
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

        public void Rename()
        {
            if (infoCards.Count > 1)
                VisCard.Rename(textOverrides);
        }

        public KSelectable GetTopSelectable()
        {
            return VisCard.selectable;
        }

        public List<KSelectable> GetAllSelectables()
        {
            return infoCards.Select(x => x.selectable).ToList();
        }
    }
}
