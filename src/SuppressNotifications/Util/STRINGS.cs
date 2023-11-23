using AzeLib;
using STRINGS;

namespace SuppressNotifications
{
    public class MYSTRINGS : RegisterStrings
    {
        public static LocString STATUS_LABEL = "<b>Status Items:</b>";
        public static LocString NOTIFICATION_LABEL = "<b>Notifications:</b>";

        public class SUPPRESSBUTTON
        {
            public static LocString NAME = "Suppress Current";
            public static LocString TOOLTIP = UI.FormatAsKeyWord("Suppress") + " the following items.";
        }

        public class CLEARBUTTON
        {
            public static LocString NAME = "Clear Suppressed";
            public static LocString TOOLTIP = UI.FormatAsKeyWord("Stop suppressing") + " the following items.";
        }

        public class BUILDINGS
        {
            public static LocString DAMAGE_BAR = "Damage Bar";
        }
    }
}
