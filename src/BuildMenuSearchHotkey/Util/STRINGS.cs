﻿using AzeLib;

namespace BuildMenuSearchHotkey
{
    public class MYSTRINGS : AStrings<MYSTRINGS>
    {
        public class BUILDSEARCHHOTKEY
        {
            public static LocString NAME = "Build Menu Search";
        }
    }

    public class OPTIONS : AStrings<OPTIONS>
    {
        public class HOTKEYWORKSWHENBUILDMENUCLOSED
        {
            public static LocString NAME = "Open Menu if Closed";
            public static LocString TOOLTIP = $"When true, pressing the {MYSTRINGS.BUILDSEARCHHOTKEY.NAME} hotkey will open the first available category and then select search.";
        }

        public class POINTTOKEYBINDMENULABEL
        {
            public static LocString NAME = $"<i>Configure binding in Controls > Mods.</i>";
            public static LocString TOOLTIP = "Default binding is Ctrl + E.";
        }
    }
}
