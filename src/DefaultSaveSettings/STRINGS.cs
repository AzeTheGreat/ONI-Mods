﻿using Harmony;
using System;

namespace DefaultSaveSettings
{
    [HarmonyPatch(typeof(GlobalAssets), "Awake")]
    public class STRINGS
    {
        static void Postfix() => LocString.CreateLocStringKeys(typeof(OPTIONS));

        public class OPTIONS
        {
            public class ENABLEPROXIMITY
            {
                public static LocString NAME = "Enable Proximity Default";
                public static LocString TOOLTIP = "If checked, the 'Enable Proximity' option is checked by default for new games.";
            }

            public class AUTOSAVEINTERVAL
            {
                public static LocString NAME = "Default Auto Save Interval";
                public static LocString TOOLTIP = "Controls the default auto save interval. Corresponds to the same values in the normal UI's sliders (sorry about the lack of clarity right now).";
            }

            public class TIMELAPSERESOLUTION
            {
                public static LocString NAME = "Default Timelapse Resolution";
                public static LocString TOOLTIP = "Controls the default timelapse resolution. Corresponds to the same values in the normal UI's sliders (sorry about the lack of clarity right now).";
            }
        }
    }
}