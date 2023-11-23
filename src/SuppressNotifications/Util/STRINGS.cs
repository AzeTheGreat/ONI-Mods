using AzeLib;

namespace SuppressNotifications
{
    public class MYSTRINGS : RegisterStrings
    {
        public static LocString LINE_DIVIDER = "--------------------\n";
        public static LocString STATUS_LABEL = "Status: ";
        public static LocString NOTIFICATION_LABEL = "Notification: ";

        public class SUPPRESSBUTTON
        {
            public static LocString NAME = "Suppress Current";
            public static LocString TOOLTIP = "Suppress the following status items and notifications:";
        }

        public class CLEARBUTTON
        {
            public static LocString NAME = "Clear Suppressed";
            public static LocString TOOLTIP = "Stop the following status items and notifications from being suppressed:";
        }

        public class BUILDINGS
        {
            public static LocString DAMAGE_BAR = "Damage Bar";
        }
    }
}
