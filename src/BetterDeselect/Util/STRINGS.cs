using AzeLib;
using STRINGS;
using System;

namespace BetterDeselect
{
    public class OPTIONS : AStrings<OPTIONS>
    {
        public class ClickOpt
        {
            public static LocString CATEGORY = "Deselection Order";
            public static LocString BASETOOLTIP = Environment.NewLine + "UI is closed in order from 1 to 3.  Objects that share the same number will be closed at the same time.";
        }

        public class SELECTEDOBJ : ClickOpt
        {
            public static LocString NAME = "Selected Object";
            public static LocString TOOLTIP = $"When to deselect the current {UI.FormatAsKeyWord("object")}.{BASETOOLTIP}";
        }

        public class OVERLAY : ClickOpt
        {
            public static LocString NAME = "Overlay";
            public static LocString TOOLTIP = $"When to clear the current {UI.FormatAsKeyWord("overlay")}.{BASETOOLTIP}";
        }

        public class BUILDMENU : ClickOpt
        {
            public static LocString NAME = "Build Menu";
            public static LocString TOOLTIP = $"When to close the open {UI.FormatAsKeyWord("build menu")}.{BASETOOLTIP}";
        }

        public class RESELECT
        {
            public static LocString NAME = "Selection Mode";
            public static LocString TOOLTIP = "Choose what to do with the currently selected tool when reselecting a held tool or opening a category.";
        }
    }
}
