using AzeLib;

namespace RebalancedTiles
{
    public class OPTIONS : AStrings<OPTIONS>
    {
        public class ISTWEAKED
        {
            public static LocString NAME = "Apply Tweaks";
            public static LocString TOOLTIP = "When true, apply the below tweaks to this tile.";
        }

        public class DECOR
        {
            public static LocString NAME = "Decor";
            public static LocString TOOLTIP = "Set the decor value.";
        }

        public class DECORRADIUS
        {
            public static LocString NAME = "Decor Radius";
            public static LocString TOOLTIP = "Set the decor radius.";
        }

        public class STRENGTHMULTIPLIER
        {
            public static LocString NAME = "Strength Multiplier";
            public static LocString TOOLTIP = "Set the strength multiplier";
        }

        public class MOVEMENTSPEED
        {
            public static LocString NAME = "Move Speed Multiplier";
            public static LocString TOOLTIP = "Set the move speed multiplier.";
        }

        public class ISCOMBUSTIBLE
        {
            public static LocString NAME = "Is Combustible";
            public static LocString TOOLTIP = "When true, will take damage over a certain temperature.";
        }

        public class COMBUSTTEMP
        {
            public static LocString NAME = "Combustion Temperatuure";
            public static LocString TOOLTIP = "Set the combustion temperature.";
        }

        public class REEDFIBERCOUNT
        {
            public static LocString NAME = $"{STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME} Required";
            public static LocString TOOLTIP = $"Set the number of {STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME} required to build.";
        }

        public class ISNOTWALL
        {
            public static LocString NAME = "Prevent Wall Decor";
            public static LocString TOOLTIP = "When true, carpet tiles used as walls do not provide decor.";
        }

        public class DIAMONDLIGHTABSORPTIONFACTOR
        {
            public static LocString NAME = "Diamond Light Absorption";
            public static LocString TOOLTIP = "Set the light absorption when made with Diamond.";
        }

        public class GLASSLIGHTABSORPTIONFACTOR
        {
            public static LocString NAME = "Glass Light Absorption";
            public static LocString TOOLTIP = "Set the light absorption when made with Glass.";
        }

        public class LIGHTABSORPTIONFACTOR
        {
            public static LocString NAME = "Light Absorption";
            public static LocString TOOLTIP = "Set the light absorption.";
        }
    }
}
