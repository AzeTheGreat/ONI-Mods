using AzeLib;

namespace DefaultBuildingSettings
{
    public class STRINGS
    {
        public class DEFAULTBUILDINGSETTINGS : RegisterStrings
        {
            private const string mod = parentPath + nameof(DEFAULTBUILDINGSETTINGS) + d;

            public class OPTIONS
            {
                private const string options = mod + nameof(OPTIONS) + d;

                public class VACANCYONLY
                {
                    private const string t = options + nameof(VACANCYONLY) + d;
                    internal const string nameKey = t + nameof(NAME);
                    internal const string tooltipKey = t + nameof(TOOLTIP);
                    public static LocString NAME = "Vacancy Only";
                    public static LocString TOOLTIP = "If checked, atmo and jet suit checkpoint are set to Vacancy Only by default.";
                }

                public class SWEEPONLY
                {
                    private const string t = options + nameof(SWEEPONLY) + d;
                    internal const string nameKey = t + nameof(NAME);
                    internal const string tooltipKey = t + nameof(TOOLTIP);
                    public static LocString NAME = "Sweep Only";
                    public static LocString TOOLTIP = "If checked, storage receptacles are set to sweep only by default.";
                }

                public class AUTOREPAIROFF
                {
                    private const string t = options + nameof(AUTOREPAIROFF) + d;
                    internal const string nameKey = t + nameof(NAME);
                    internal const string tooltipKey = t + nameof(TOOLTIP);
                    public static LocString NAME = "Auto Repair Off";
                    public static LocString TOOLTIP = "If checked, buildings will not allow repairs by default.";
                }

                public class OPENDOOR
                {
                    private const string t = options + nameof(OPENDOOR) + d;
                    internal const string nameKey = t + nameof(NAME);
                    internal const string tooltipKey = t + nameof(TOOLTIP);
                    public static LocString NAME = "Open Doors";
                    public static LocString TOOLTIP = "If checked, Doors that allow airflow will immediately be opened after being built.";
                }

                public class ACTIVATIONRANGE
                {
                    private const string activationRange = options + nameof(ACTIVATIONRANGE) + d;

                    internal const string batteryCatKey = activationRange + nameof(BATTERYCATEGORY);
                    internal const string reservoirCatKey = activationRange + nameof(RESERVOIRCATEGORY);
                    public static LocString BATTERYCATEGORY = "Smart Batteries";
                    public static LocString RESERVOIRCATEGORY = "Reservoirs";

                    public class ACTIVATEVALUE
                    {
                        private const string t = activationRange + nameof(ACTIVATEVALUE) + d;
                        internal const string nameKey = t + nameof(NAME);
                        internal const string tooltipKey = t + nameof(TOOLTIP);
                        public static LocString NAME = "Activate Value";
                        public static LocString TOOLTIP = "The value at which the signal will turn green.  The left/lower value in the UI.";
                    }

                    public class DEACTIVATEVALUE
                    {
                        private const string t = activationRange + nameof(DEACTIVATEVALUE) + d;
                        internal const string nameKey = t + nameof(NAME);
                        internal const string tooltipKey = t + nameof(TOOLTIP);
                        public static LocString NAME = "Deactivate Value";
                        public static LocString TOOLTIP = "The value at which the signal will turn red.  The right/upper value in the UI.";
                    }
                }

                public class GENERATORS
                {
                    private const string generators = options + nameof(GENERATORS) + d;

                    internal const string categoryKey = generators + nameof(CATEGORY);
                    public static LocString CATEGORY = "Generators";

                    public class DELIVERGENVALUE
                    {
                        private const string t = generators + nameof(DELIVERGENVALUE) + d;
                        internal const string nameKey = t + nameof(NAME);
                        internal const string tooltipKey = t + nameof(TOOLTIP);
                        public static LocString NAME = "Delivery Generator Threshold";
                        public static LocString TOOLTIP = "The threshold at which the generators that require manual delivery will request more fuel.";
                    }

                    public class MANUALGENVALUE
                    {
                        private const string t = generators + nameof(MANUALGENVALUE) + d;
                        internal const string nameKey = t + nameof(NAME);
                        internal const string tooltipKey = t + nameof(TOOLTIP);
                        public static LocString NAME = "Manual Generator Threshold";
                        public static LocString TOOLTIP = "The threshold at which a duplicant will be requested to run on the manual generator.";
                    }
                }
            }
        }
    }
}


