using AzeLib;
using System;

namespace DefaultSaveSettings
{
    public class STRINGS
    {
        public class DEFAULTSAVESETTINGS : RegisterStrings
        {
            private const string mod = parentPath + nameof(DEFAULTSAVESETTINGS) + d;

            public class OPTIONS 
            {
                private const string options = mod + nameof(OPTIONS) + d;

                public class ENABLEPROXIMITY
                {
                    private const string t = options + nameof(ENABLEPROXIMITY) + d;
                    internal const string nameKey = t + nameof(NAME);
                    internal const string tooltipKey = t + nameof(TOOLTIP);
                    public static LocString NAME = "Enable Proximity Default";
                    public static LocString TOOLTIP = "If checked, the 'Enable Proximity' option is checked by default for new games.";
                }

                public class AUTOSAVEINTERVAL
                {
                    private const string saveInterval = options + nameof(AUTOSAVEINTERVAL) + d;
                    internal const string nameKey = saveInterval + nameof(NAME);
                    internal const string tooltipKey = saveInterval + nameof(TOOLTIP);
                    public static LocString NAME = "Default Auto Save Interval";
                    public static LocString TOOLTIP = "Controls the default auto save interval.";
                }

                public class TIMELAPSERESOLUTION
                {
                    private const string t = options + nameof(TIMELAPSERESOLUTION) + d;
                    internal const string nameKey = t + nameof(NAME);
                    internal const string tooltipKey = t + nameof(TOOLTIP);
                    public static LocString NAME = "Default Timelapse Resolution";
                    public static LocString TOOLTIP = "Controls the default timelapse resolution.";
                }

                public class GERMS
                {
                    private const string germs = options + nameof(GERMS) + d;

                    internal const string categoryKey = germs + nameof(CATEGORY);
                    public static LocString CATEGORY = "Disease";

                    public class ENABLEAUTODISINFECT
                    {
                        private const string t = germs + nameof(ENABLEAUTODISINFECT) + d;
                        internal const string nameKey = t + nameof(NAME);
                        internal const string tooltipKey = t + nameof(TOOLTIP);
                        public static LocString NAME = "Enable Auto Disinfect Default";
                        public static LocString TOOLTIP = "If checked, the 'Disinfect At' option will default to true for new games.";
                    }

                    public class DISINFECTMINGERMCOUNT
                    {
                        private const string t = germs + nameof(DISINFECTMINGERMCOUNT) + d;
                        internal const string nameKey = t + nameof(NAME);
                        internal const string tooltipKey = t + nameof(TOOLTIP);
                        public static LocString NAME = "Default Germ Count for Disinfect";
                        public static LocString TOOLTIP = "Controls the default minimum germ count for auto disinfection.";
                    }
                }
            }
        }
    }
}


