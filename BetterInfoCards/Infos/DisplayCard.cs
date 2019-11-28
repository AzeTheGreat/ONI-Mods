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
        public string Title { get { return infoCards[0].Title; } }

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

            // Don't display "x 1" since it's implicit. Differs from vanilla so some players might want a config for this.
            int sum = infoCards.Sum(x => x.quantity);
            if (sum > 1)
                titleOverride = infoCards[0].Title + " x " + sum;
            else
                titleOverride = infoCards[0].Title;

            if(infoCards.Count > 1)
            {
                foreach (var textValue in infoCards[0].textValues)
                {
                    string name = textValue.Key;

                    string test = StatusDataManager.statusConverter[name].getTextOverride(name, infoCards.Select(x => x.textValues[name]).ToList());
                    Debug.Log(test);
                }
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
            if(titleOverride != string.Empty)
                infoCards[0].Title = titleOverride;
        }

        public void Resize(float newX)
        {
            infoCards[0].Resize(newX);
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
