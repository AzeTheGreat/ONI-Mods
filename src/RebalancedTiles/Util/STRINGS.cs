using AzeLib;

namespace RebalancedTiles
{
    public class OPTIONS : AStrings<OPTIONS>
    {
        public class BASETT
        {
            public static LocString IIFBLANK = "If blank, the game's default will be used.";
        }

        public class DECOR
        {
            public static LocString NAME = "Decor";
            public static LocString TOOLTIP = $"Set the decor value.  {BASETT.IIFBLANK}";
        }

        public class DECORRADIUS
        {
            public static LocString NAME = "Decor Radius";
            public static LocString TOOLTIP = $"Set the decor radius (tiles).  {BASETT.IIFBLANK}";
        }

        public class STRENGTHMULTIPLIER
        {
            public static LocString NAME = "Strength Multiplier";
            public static LocString TOOLTIP = $"Set the strength multiplier  {BASETT.IIFBLANK}";
        }

        public class MOVEMENTSPEED
        {
            public static LocString NAME = "Move Speed Multiplier";
            public static LocString TOOLTIP = $"Set the move speed multiplier.  {BASETT.IIFBLANK}";
        }

        public class COMBUSTTEMP
        {
            public static LocString NAME = "Combustion Temperature";
            public static LocString TOOLTIP = "Set the combustion temperature (K).  If blank, this will not combust.";
        }

        public class REEDFIBERCOUNT
        {
            public static LocString NAME = $"{STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME} Required";
            public static LocString TOOLTIP = $"Set the number of {STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME} required to build.    {BASETT.IIFBLANK}";
        }

        public class ISNOTWALL
        {
            public static LocString NAME = "Only Top Decorative";
            public static LocString TOOLTIP = "When true, only the top of carpet tiles will provide decor - none will be emitted below or to the sides.";
        }

        public class DIAMONDLIGHTABSORPTIONFACTOR
        {
            public static LocString NAME = "Diamond Light Absorption";
            public static LocString TOOLTIP = $"Set the light absorption (%) when made with Diamond.    {BASETT.IIFBLANK}";
        }

        public class GLASSLIGHTABSORPTIONFACTOR
        {
            public static LocString NAME = "Glass Light Absorption";
            public static LocString TOOLTIP = $"Set the light absorption (%) when made with Glass.    {BASETT.IIFBLANK}";
        }

        public class LIGHTABSORPTIONFACTOR
        {
            public static LocString NAME = "Light Absorption";
            public static LocString TOOLTIP = $"Set the light absorption (%).    If blank, no light will be absorbed.";
        }
    }
}
