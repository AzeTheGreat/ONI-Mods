using AzeLib;

namespace CleanHUD
{
    class OPTIONS : RegisterStrings
    {
        public class Vignette
        {
            public static LocString CATEGORY = "Vignettes";
        }

        public class VIGNETTEALPHA : Vignette
        {
            public static LocString NAME = "Vignette Opacity";
            public static LocString TOOLTIP = "Adjust the opacity of the vignette (standard value = 47%)";
        }

        public class ALERTVIGNETTEALPHA : Vignette
        {
            public static LocString NAME = "Warning Vignette Opacity";
            public static LocString TOOLTIP = "Adjust the opacity of the warning vignettes (standard value = 30%) ";
        }

        public class ISWATERMARKDISABLED
        {
            public static LocString NAME = "Disable Watermark";
            public static LocString TOOLTIP = "When true, the build watermark is removed from the screen.";
        }

        public class USESMALLBUTTONS
        {
            public static LocString NAME = "Small Buttons";
            public static LocString TOOLTIP = "When true, the management buttons all use the small format.";
        }

        public class InvalidArea
        {
            public static LocString CATEGORY = "Out-of-Bounds Warning";
        }

        public class INVALIDAREAOPACITY : InvalidArea
        {
            public static LocString NAME = "Warning Opacity";
            public static LocString TOOLTIP = "Sets the opacity of the warning.";
        }

        public class ISSOLIDINVALIDAREA : InvalidArea
        {
            public static LocString NAME = "Solid Warning";
            public static LocString TOOLTIP = "When true, the warning will be solid, rather than striped.";
        }
    }
}
