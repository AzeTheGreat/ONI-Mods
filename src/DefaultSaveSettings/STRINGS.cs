using AzeLib;

namespace DefaultSaveSettings
{
    public class OPTIONS : AStrings<OPTIONS>
    {
        public class ENABLEPROXIMITY
        {
            public static LocString NAME = "Enable Proximity Default";
            public static LocString TOOLTIP = "If checked, the 'Enable Proximity' option is checked by default for new games.";
        }

        public class AUTOSAVEINTERVAL
        {
            public static LocString NAME = "Default Auto Save Interval";
            public static LocString TOOLTIP = "Controls the default auto save interval.";
        }

        public class TIMELAPSERESOLUTION
        {
            public static LocString NAME = "Default Timelapse Resolution";
            public static LocString TOOLTIP = "Controls the default timelapse resolution.";
        }

        public class Germs
        {
            public static LocString CATEGORY = "Disease";
        }

        public class ENABLEAUTODISINFECT : Germs
        {
            public static LocString NAME = "Enable Auto Disinfect Default";
            public static LocString TOOLTIP = "If checked, the 'Disinfect At' option will default to true for new games.";
        }

        public class DISINFECTMINGERMCOUNT : Germs
        {
            public static LocString NAME = "Default Germ Count for Disinfect";
            public static LocString TOOLTIP = "Controls the default minimum germ count for auto disinfection.";
        }
    }
}
