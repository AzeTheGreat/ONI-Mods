using AzeLib;

namespace NoDoorIdle
{
    class OPTIONS : AStrings<OPTIONS>
    {
        public class CANIDLEINDOORS
        {
            public static LocString NAME = "Idle In Doors";
            public static LocString TOOLTIP = "When true, duplicants can idle inside of doors.";
        }

        public class CANIDLETRAVERSEDOORS
        {
            public static LocString NAME = "Idle Traverse Doors";
            public static LocString TOOLTIP = "When true, duplicants can cross doors while idling.";
        }

        public class FIXIDLETRAPPING
        {
            public static LocString NAME = "Fix Idle Trapping";
            public static LocString TOOLTIP = "When true, fixes a base game bug that causes dupes to get stuck at checkpoints when idle.";
        }
    }
}
