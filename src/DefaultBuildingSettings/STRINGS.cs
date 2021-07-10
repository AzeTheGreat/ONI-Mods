using AzeLib;

namespace DefaultBuildingSettings
{
    public class OPTIONS : RegisterStrings
    {
        public class VACANCYONLY : BaseOpt
        {
            public static LocString NAME = "Vacancy Only";
            public static LocString TOOLTIP = "If checked, atmo and jet suit checkpoint are set to Vacancy Only by default.";
        }

        public class SWEEPONLY : BaseOpt
        {
            public static LocString NAME = "Sweep Only";
            public static LocString TOOLTIP = "If checked, storage receptacles are set to sweep only by default.";
        }

        public class AUTOREPAIROFF : BaseOpt
        {
            public static LocString NAME = "Auto Repair Off";
            public static LocString TOOLTIP = "If checked, buildings will not allow repairs by default.";
        }

        public class OPENDOORS : BaseOpt
        {
            public static LocString NAME = "Open Doors";
            public static LocString TOOLTIP = "If checked, Doors that allow airflow will immediately be opened after being built.";
        }

        public class SWITCHESOFF : BaseOpt
        {
            public static LocString NAME = "Switches Start Off";
            public static LocString TOOLTIP = "If checked, automation and power switches will initialize in the inactive state.";
        }

        public class ActivateValues
        {
            public static LocString NAME = "Activate Value";
            public static LocString TOOLTIP = "The value at which the signal will turn green.  The left/lower value in the UI.";
        }

        public class DeactivateValues
        {
            public static LocString NAME = "Deactivate Value";
            public static LocString TOOLTIP = "The value at which the signal will turn red.  The right/upper value in the UI.";
        }

        public class Batteries
        {
            public static LocString CATEGORY = "Smart Batteries";
        }

        public class SMARTBATTERYACTIVATEVALUE : ActivateValues
        {
            public static LocString CATEGORY = Batteries.CATEGORY;
        }

        public class SMARTBATTERYDEACTIVATEVALUE : DeactivateValues
        {
            public static LocString CATEGORY = Batteries.CATEGORY;
        }

        public class Reservoirs
        {
            public static LocString CATEGORY = "Reservoirs";
        }

        public class RESERVOIRACTIVATEVALUE : ActivateValues
        {
            public static LocString CATEGORY = Reservoirs.CATEGORY;
        }

        public class RESERVOIRDEACTIVATEVALUE : DeactivateValues
        {
            public static LocString CATEGORY = Reservoirs.CATEGORY;
        }

        public class Generators
        {
            public static LocString CATEGORY = "Generators";
        }

        public class DELIVERGENVALUE : Generators
        {
            public static LocString NAME = "Delivery Generator Threshold";
            public static LocString TOOLTIP = "The threshold at which the generators that require manual delivery will request more fuel.";
        }

        public class MANUALGENVALUE : Generators
        {
            public static LocString NAME = "Manual Generator Threshold";
            public static LocString TOOLTIP = "The threshold at which a duplicant will be requested to run on the manual generator.";
        }
    }
}