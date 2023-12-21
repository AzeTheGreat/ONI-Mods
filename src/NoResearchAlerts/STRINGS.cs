using AzeLib;

namespace NoResearchAlerts
{
    class OPTIONS : AStrings<OPTIONS>
    {
        public class ALERTMODE
        {
            public static LocString NAME = "Mode";
            public static LocString TOOLTIP = "The mode to use.";
        }

        public class SUPPRESSMESSAGE
        {
            public static LocString NAME = "Prevent Notification";
            public static LocString TOOLTIP = "If true, prevents notifications in the top left due to completed research.";
        }
    }
}
