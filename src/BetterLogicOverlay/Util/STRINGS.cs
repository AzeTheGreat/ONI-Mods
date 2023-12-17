using AzeLib;

namespace BetterLogicOverlay
{
    public class MYSTRINGS : RegisterStrings
    {
        public class UNITMODIFIERS
        {
            public static LocString thousand = "k";
        }
    }

    public class ABBREVIATIONS : RegisterFieldlessStrings
    {
        public static LocString DirtyPrefix = "P-";

        public static LocString Aluminum = "Al";
        public static LocString AluminumOre = Aluminum;

        public static LocString Carbon = "C";
        public static LocString RefinedCarbon = Carbon;

        public static LocString CarbonDioxide = "CO<sub>2</sub>";

        public static LocString Chlorine = "Cl";

        public static LocString Copper = "Cu";

        public static LocString CrudeOil = "C-Oil";

        public static LocString Dirt = string.Empty;
        public static LocString ToxicSand = DirtyPrefix + Dirt;

        public static LocString Ethanol = "EtOH";

        public static LocString Glass = "Glass";

        public static LocString Gold = "Au";
        public static LocString GoldAmalgam = Gold + "Amal";

        public static LocString Hydrogen = "H";

        public static LocString Iron = "Fe";
        public static LocString IronOre = Iron + "Ore";

        public static LocString Lead = "Pb";

        public static LocString Methane = "CH<sub>4</sub>";

        public static LocString Naphtha = "Nap";

        public static LocString Oxygen = "O<sub>2</sub>";
        public static LocString ContaminatedOxygen = DirtyPrefix + Oxygen;

        public static LocString Petroleum = "Petrol";

        public static LocString Phosphorus = "P";
        public static LocString Phosphorite = Phosphorus + "-rite";

        public static LocString Propane = "C<sub>3</sub>H<sub>8</sub>";

        public static LocString Radium = "Ra";

        public static LocString RockGas = "Rock";
        public static LocString CrushedRock = "Rock";

        public static LocString Salt = "NaCl";

        public static LocString SourGas = "Sour";

        public static LocString Steel = "Steel";

        public static LocString Sulfur = "S";

        public static LocString SuperCoolant = "S-Cool";

        public static LocString Tungsten = "W";

        public static LocString Niobium = "Nb";

        public static LocString ViscoGel = "V-Gel";

        public static LocString Water = "H<sub>2</sub>O";
        public static LocString Steam = Water;
        public static LocString SaltWater = "S-" + Water;
        public static LocString Brine = "Brine";
        public static LocString DirtyWater = DirtyPrefix + Water;
        public static LocString Ice = "Ice";
        public static LocString BrineIce = "B-" + Ice;
        public static LocString CrushedIce = "C-" + Ice;
        public static LocString DirtyIce = DirtyPrefix + Ice;
    }

    public class OPTIONS : RegisterStrings
    {
        public class FIXWIREOVERWRITE
        {
            public static LocString NAME = "Fix Wire Overwriting";
            public static LocString TOOLTIP = "If true, green signals will not make a red output port display as green.";
        }

        public class DISPLAYLOGICSETTINGS
        {
            public static LocString NAME = "Display Logic Settings";
            public static LocString TOOLTIP = "If true, logic gate and sensor settings will be displayed in the automation overlay.";
        }
    }
}
