using AzeLib;
using STRINGS;

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

        public class AUTOSAVEFREQ
        {
            static LocString FIFTY = GetFormattedAutosaveFrequency(50);
            static LocString TWENTY = GetFormattedAutosaveFrequency(20);
            static LocString TEN = GetFormattedAutosaveFrequency(10);
            static LocString FIVE = GetFormattedAutosaveFrequency(5);
            static LocString TWO = GetFormattedAutosaveFrequency(2);
            static LocString EVERY = GetFormattedAutosaveFrequency(1);
            private static string GetFormattedAutosaveFrequency(int num) => string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_FREQUENCY_DESCRIPTION, num);
        }

        public class RESOLUTION
        {
            static LocString R256 = GetFormattedResolution(new(256, 358));
            static LocString R512 = GetFormattedResolution(new(512, 768));
            static LocString R1024 = GetFormattedResolution(new(1024, 1536));
            static LocString R2048 = GetFormattedResolution(new(2048, 3072));
            static LocString R4096 = GetFormattedResolution(new(4096, 6144));
            static LocString R8192 = GetFormattedResolution(new(8192, 12288));
            private static string GetFormattedResolution(Vector2I vec) => string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_RESOLUTION_DESCRIPTION, vec.x, vec.y);
        }
    }
}
