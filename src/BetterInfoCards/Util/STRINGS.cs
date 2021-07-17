using AzeLib;

namespace BetterInfoCards
{
    class OPTIONS : RegisterStrings
    {
        public class COMPACTNESS
        {
            public static LocString NAME = "Info Card Compactness";
            public static LocString TOOLTIP = "How compact the info cards should be.";
        }

        public class INFOCARDOPACITY
        {
            public static LocString NAME = "Info Card Opacity";
            public static LocString TOOLTIP = "Game default is 0.9.";
        }

        public class TEMPERATUREBANDWIDTH
        {
            public static LocString NAME = "Temperature Band Width";
            public static LocString TOOLTIP = "Info cards are grouped by temperature if they are close enough, this sets the maximum temperature difference at which info cards will be grouped.";
        }

        public class USEBASESELECTION
        {
            public static LocString NAME = "Restricted Selection";
            public static LocString TOOLTIP = "Only allow selecting items that would be selectable in the base game.";
        }

        public class FORCEFIRSTSELECTIONTOHOVER
        {
            public static LocString NAME = "First Selection Hover";
            public static LocString TOOLTIP = "Limit the first selection to the hovered item, without further restricting future selections.";
        }

        public class HIDEELEMENTCATEGORIES
        {
            public static LocString NAME = "Hide Element Categories";
            public static LocString TOOLTIP = "When true, info cards will not include the element categories.";
        }

        public class INFOCARDSIZE
        {
            public static LocString CATEGORY = "Info Card Size";
        }

        public class SHOULDOVERRIDE
        {
            public static LocString NAME = "Override Card Size";
            public static LocString TOOLTIP = "When true, these custom values will modify the info card size.";
        }

        public class FONTSIZECHANGE
        {
            public static LocString NAME = "Font Size";
            public static LocString TOOLTIP = "Controls how much to increase/decrease the font size.";
        }

        public class MINHEIGHT
        {
            public static LocString NAME = "Line Height";
            public static LocString TOOLTIP = "Controls the space between lines.";
        }

        public class MAXIMAGESIZE
        {
            public static LocString NAME = "Icon Size";
            public static LocString TOOLTIP = "Controls the maximum size of icons.";
        }

        public class YPADDING
        {
            public static LocString NAME = "Padding";
            public static LocString TOOLTIP = "Sets the size of the border padding.";
        }
    }
}
