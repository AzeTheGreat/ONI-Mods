using AzeLib;
using STRINGS;
using System;

namespace BetterDeselect;

public class OPTIONS : AStrings<OPTIONS>
{
    public class ClickOpt
    {
        public static LocString CATEGORY = "Deselection Order";
    }

    public class SELECTEDOBJ : ClickOpt
    {
        public static LocString NAME = "Selected Object";
        public static LocString TOOLTIP = $"When to deselect the current {UI.FormatAsKeyWord("object")}";
    }

    public class OVERLAY : ClickOpt
    {
        public static LocString NAME = "Overlay";
        public static LocString TOOLTIP = $"When to clear the current {UI.FormatAsKeyWord("overlay")}";
    }

    public class BUILDMENU : ClickOpt
    {
        public static LocString NAME = "Build Menu";
        public static LocString TOOLTIP = $"When to close the open {UI.FormatAsKeyWord("build menu")}";
    }

    public class RESELECT
    {
        public static LocString NAME = "Selection Mode";
        public static LocString TOOLTIP = "Choose what to do with the currently selected tool when reselecting a held tool or opening a category.";
    }

    public class HOLD
    {
        public static LocString NAME = "Hold";
        public static LocString TOOLTIP = "Keep the current tool selected.";
    }

    public class CLOSE
    {
        public static LocString NAME = "Clear";
        public static LocString TOOLTIP = "Deselect the current tool. Closest to vanilla behavior.";
    }

    public class DeselectOrder
    {
        public static LocString TOOLTIP = "UI is closed in order from 1 to 3." + Environment.NewLine + 
            "Objects that share the same number will be closed at the same time.";
    }

    public class ONE : DeselectOrder
    {
        public static LocString NAME = "1";
    }

    public class TWO : DeselectOrder
    {
        public static LocString NAME = "2";
    }

    public class THREE : DeselectOrder
    {
        public static LocString NAME = "3";
    }
}
