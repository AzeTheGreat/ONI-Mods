using System.Collections.Generic;
using System.Linq;

namespace BetterInfoCards
{
    public class DisplayCard
    {
        private List<InfoCard> infoCards;
        private int visCardIndex;
        private InfoCard VisCard => infoCards[visCardIndex];

        public DisplayCard(List<InfoCard> infoCards)
        {
            this.infoCards = infoCards;
            
            visCardIndex = infoCards.FindIndex(x => x.isSelected);
            if (visCardIndex == -1)
                visCardIndex = 0;
        }

        public void Draw()
        {
            // Explicitly drawing the VisCard is not necessary for 99% of cases.
            // However, custom converters could be created to group different values together.
            VisCard.Draw(infoCards, visCardIndex);
        }

        public List<KSelectable> GetAllSelectables()
        {
            return infoCards.Select(x => x.selectable).ToList();
        }
    }
}
