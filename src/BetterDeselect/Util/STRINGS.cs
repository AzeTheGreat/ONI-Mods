using AzeLib;

namespace BetterDeselect
{
    public class OPTIONS : AStrings<OPTIONS>
    {
        public class ClickOpt
        {
            public static LocString TOOLTIP = "What order to deslect items. The Cursor is deslected on First.";
        }

        public class OVERLAY : ClickOpt
        {
            public static LocString NAME = "Deselect Overlay";
        }

        public class BUILDMENU : ClickOpt
        {
            public static LocString NAME = "Deselect Build Menu";
        }

        public class RESELECT
        {
            public static LocString NAME = "Selection Mode";
            public static LocString TOOLTIP = "When reselecting a held tool, or opening a category, choose what to do with the selected tool.";
        }
    }
}
