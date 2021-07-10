using AzeLib;

namespace BetterInfoCards
{
    class OPTIONS : RegisterStrings
    {
        public class COMPACTNESS : BaseOpt
        {
            public static LocString NAME = "Info Card Compactness";
            public static LocString TOOLTIP = "How compact the info cards should be.";
        }

        public class INFOCARDOPACITY : BaseOpt
        {
            public static LocString NAME = "Info Card Opacity";
            public static LocString TOOLTIP = "Game default is 0.9.";
        }

        public class TEMPERATUREBANDWIDTH : BaseOpt
        {
            public static LocString NAME = "Temperature Band Width";
            public static LocString TOOLTIP = "Info cards are grouped by temperature if they are close enough, this sets the maximum temperature difference at which info cards will be grouped.";
        }

        public class USEBASESELECTION : BaseOpt
        {
            public static LocString NAME = "Restricted Selection";
            public static LocString TOOLTIP = "Only allow selecting items that would be selectable in the base game.";
        }

        public class FORCEFIRSTSELECTIONTOHOVER : BaseOpt
        {
            public static LocString NAME = "First Selection Hover";
            public static LocString TOOLTIP = "Limit the first selection to the hovered item, without further restricting future selections.";
        }
    }
}
