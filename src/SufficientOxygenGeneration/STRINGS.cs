using AzeLib;

namespace SufficientOxygenGeneration
{
    class OPTIONS : RegisterStrings
    {
        public class MODE
        {
            public static LocString NAME = "Mode";
            public static LocString TOOLTIP = "Switch between constant or ratio threshold.";
        }

        public class CONSTANTTHRESHOLD
        {
            public static LocString NAME = "Constant Threshold";
            public static LocString TOOLTIP = "Set to oxygen deficit in kg below which you'd like to be notified.";
        }

        public class RATIOTHRESHOLD
        {
            public static LocString NAME = "Ratio Threshold";
            public static LocString TOOLTIP = "Set to the ratio of oxygen produced to consumed, below which you'd like to be notified.";
        }
    }
}
