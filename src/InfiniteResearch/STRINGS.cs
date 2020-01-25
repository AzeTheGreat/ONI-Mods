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

                public static LocString TOOLTIP = "Only duplicants with a Science Attribute between {Attributes} can learn from this building.";
            }
        }
    }
}
