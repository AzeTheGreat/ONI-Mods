using AzeLib;

namespace BetterInfoCards
{
    class OPTIONS : AStrings<OPTIONS>
    {
        public class INFOCARDOPACITY
        {
            public static LocString NAME = "Info Card Opacity";
            public static LocString TOOLTIP = "Opactiy of info card backgrounds.  (Base game = 90%)";
        }

        public class INFOCARDBACKGROUNDRED
        {
            public static LocString NAME = "Background Tint - Red";
            public static LocString TOOLTIP = "Red channel value for the info card background tint.";
        }

        public class INFOCARDBACKGROUNDGREEN
        {
            public static LocString NAME = "Background Tint - Green";
            public static LocString TOOLTIP = "Green channel value for the info card background tint.";
        }

        public class INFOCARDBACKGROUNDBLUE
        {
            public static LocString NAME = "Background Tint - Blue";
            public static LocString TOOLTIP = "Blue channel value for the info card background tint.";
        }

        public class TEMPERATUREBANDWIDTH
        {
            public static LocString NAME = "Temperature Band Width";
            public static LocString TOOLTIP = "Maximum temperature difference at which info cards will be grouped.";
        }

        public class USEBASESELECTION
        {
            public static LocString NAME = "Restrict Selections";
            public static LocString TOOLTIP = "On: Restrict selectable info cards based on context.  (Base game)"
                + "\nOff: All visible info cards can be selected.";
        }

        public class FORCEFIRSTSELECTIONTOHOVER
        {
            public static LocString NAME = "First Selection Hover";
            public static LocString TOOLTIP = "On: First selection selects the highlighted item.  (Base game)" 
                + "\nOff: First selection selects the first info card.";
        }

        public class HIDEELEMENTCATEGORIES
        {
            public static LocString NAME = "Hide Element Categories";
            public static LocString TOOLTIP = "On: Remove element categories from info cards.";
        }

        public class INFOCARDSIZE
        {
            public static LocString CATEGORY = "Info Card Size";
        }

        public class SHOULDOVERRIDE
        {
            public static LocString NAME = "Override Card Size";
            public static LocString TOOLTIP = "On: Apply custom Info Card Size values.";
        }

        public class FONTSIZECHANGE
        {
            public static LocString NAME = "Font Size";
            public static LocString TOOLTIP = "Increase/decrease font size relative to the base game.";
        }

        public class LINESPACING
        {
            public static LocString NAME = "Line Spacing";
            public static LocString TOOLTIP = "Padding between text lines.";
        }

        public class ICONSIZECHANGE
        {
            public static LocString NAME = "Icon Size";
            public static LocString TOOLTIP = "Increase/decrease icon size relative to the base game.";
        }

        public class YPADDING
        {
            public static LocString NAME = "Borders";
            public static LocString TOOLTIP = "Padding on the borders.";
        }
    }
}
