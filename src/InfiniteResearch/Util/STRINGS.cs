using AzeLib;

namespace InfiniteResearch
{
    public class MYSTRINGS : AStrings<MYSTRINGS>
    {
        public class BUILDING
        {
            public class STATUSITEMS
            {
                public class REQUIRESATTRIBUTERANGE
                {
                    public static LocString NAME = "Attribute-Required Operation";
                    public static LocString TOOLTIP = "Only duplicants with a base Science Attribute from {Attributes} can learn from this building.";
                }
            }
        }

        public class DUPLICANTS
        {
            public class STATUSITEMS
            {
                public class LEARNING
                {
                    public static LocString NAME = "Learning";
                    public static LocString TOOLTIP = "This Duplicant is intently studying to improve their Science Attribute.";
                }
            }

            public class CHORES
            {
                public class PRECONDITIONS
                {
                    public class REQUIRES_ATTRIBUTE_RANGE
                    {
                        public static LocString DESCRIPTION = "Science outside range";
                    }
                }
            }
        }

        public class BUTTONS
        {
            public class DISABLELEARN
            {
                public static LocString NAME = "Disable Learning";
                public static LocString TOOLTIP = "Stop dupes from training their Science Attribute here.";
            }

            public class ENABLELEARN
            {
                public static LocString NAME = "Enable Learning";
                public static LocString TOOLTIP = "Allow dupes to train their Science Attribute here.";
            }
        }
    }

    public class OPTIONS : AStrings<OPTIONS>
    {
        public class MIN
        {
            public static LocString NAME = "Minimum";
            public static LocString TOOLTIP = "The lowest level at which a dupe can operate this station.";
        }
        
        public class MAX
        {
            public static LocString NAME = "Maximum";
            public static LocString TOOLTIP = "The highest level at which a dupe can operate this station.";
        }

        public class EXPRATE
        {
            public static LocString NAME = "Experience Rate";
            public static LocString TOOLTIP = "How quickly duplicants gain experience while working at this station.  Vanilla defaults to 1.11.";
        }
    }
}
