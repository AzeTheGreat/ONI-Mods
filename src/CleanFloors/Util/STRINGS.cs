using AzeLib;

namespace CleanFloors;

public class OPTIONS : AStrings<OPTIONS>
{
    public class REMOVEOUTSIDECORNERS
    {
        public static LocString NAME = "Hide Outside Corners";
        public static LocString TOOLTIP = "When true, decor bits on the outside corners of tiles will be hidden.";
    }

    public class REMOVEINSIDECORNERS
    {
        public static LocString NAME = "Hide Inside Corners";
        public static LocString TOOLTIP = """
            When true, decor bits on the inside corners of tiles will be hidden.
            Note: these bits hide small anomalies between connected tiles.
            """;
    }
}
