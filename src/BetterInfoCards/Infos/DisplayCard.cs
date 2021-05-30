using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class DisplayCard
    {
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
            
            visCardIndex = infoCards.FindIndex(x => x.isSelected);
            if (visCardIndex == -1)
                visCardIndex = 0;
        }

        public void Draw()
        {
            VisCard.Draw(infoCards);
        }

        public List<KSelectable> GetAllSelectables()
        {
            return infoCards.Select(x => x.selectable).ToList();
        }
    }
}
