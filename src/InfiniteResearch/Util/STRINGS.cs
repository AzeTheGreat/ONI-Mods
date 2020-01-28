using Harmony;

namespace InfiniteResearch
{
    [HarmonyPatch(typeof(GlobalAssets), "Awake")]
    public class BUILDING
    {
        static void Postfix() => LocString.CreateLocStringKeys(typeof(BUILDING));

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
    }

    public class BUTTONS
    {
        public class ENABLELEARN
        {
            public static LocString NAME = "Disable Learning";
            public static LocString TOOLTIP = "Stop dupes from training their Science Attribute here.";
        }

        public class DISABLELEARN
        {
            public static LocString NAME = "Enable Learning";
            public static LocString TOOLTIP = "Allow dupes to train their Science Attribute here.";
        }
    }
}
