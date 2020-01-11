using System.Collections.Generic;

namespace BetterInfoCards
{
    public class InfoCards
    {
        private List<InfoCard> infoCards;
        private DisplayCards displayCards = new DisplayCards();

        public void UpdateData(List<InfoCard> infoCards)
        {
            this.infoCards = infoCards;
        }

        public void Update()
        {
            displayCards.UpdateData(infoCards);
            displayCards.Update();
        }
    }
}
