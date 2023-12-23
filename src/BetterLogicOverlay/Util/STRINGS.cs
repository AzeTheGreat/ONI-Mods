using AzeLib;
using STRINGS;
using System.Collections.Generic;
using System.Linq;

namespace BetterLogicOverlay
{
    public class MYSTRINGS : AStrings<MYSTRINGS>
    {
        public class UNITMODIFIERS
        {
            public static LocString thousand = "k";
        }
    }

    public class ABBREVIATIONS : AFieldlessStrings<ABBREVIATIONS>
    {
        public override List<POTEntry> GetPOTEntries()
        {
            var elementEntries = ElementLoader.CollectElementsFromYAML();
            var uniqueIDs = elementEntries.Select(entry => ElementAbbreviation.StripElementModifiers(entry.elementId)).Distinct();

            return elementEntries
                .Where(entry => uniqueIDs.Contains(entry.elementId))
                .Select(entry => new POTEntry(
                    entry.elementId,
                    UI.StripLinkFormatting(Strings.Get(entry.localizationID))))
                .ToList();
        }

        // Modifiers
        static LocString DirtyPrefix = "P-";
        static LocString OreSuffix = " Ore";

        // Abbreviations
        static LocString Aluminum = "Al";
        static LocString AluminumOre = Aluminum + OreSuffix;

        static LocString Carbon = "C";
        static LocString RefinedCarbon = "R-" + Carbon;

        static LocString CarbonDioxide = "CO<sub>2</sub>";

        static LocString Chlorine = "Cl";

        static LocString Cobalt = "Co";
        static LocString Cobaltite = Cobalt + OreSuffix;

        static LocString Copper = "Cu";
        static LocString Cuprite = Copper + OreSuffix;

        static LocString CrudeOil = "C-Oil";

        static LocString CrushedRock = "C-Rock";

        static LocString Ethanol = "EtOH";

        static LocString Glass = "Glass";

        static LocString Gold = "Au";
        static LocString GoldAmalgam = Gold + "Amal";

        static LocString Hydrogen = "H";

        static LocString Iron = "Fe";
        static LocString IronOre = Iron + OreSuffix;

        static LocString Lead = "Pb";

        static LocString Methane = "CH<sub>4</sub>";

        static LocString Naphtha = "Nap";

        static LocString Oxygen = "O<sub>2</sub>";
        static LocString ContaminatedOxygen = DirtyPrefix + Oxygen;

        static LocString Petroleum = "Petrol";

        static LocString Phosphorus = "P";
        static LocString Phosphorite = Phosphorus + "-rite";

        static LocString Propane = "C<sub>3</sub>H<sub>8</sub>";

        static LocString Radium = "Ra";

        static LocString Salt = "NaCl";

        static LocString Sour = "Sour";

        static LocString Steel = "Steel";

        static LocString Sulfur = "S";

        static LocString SuperCoolant = "S-Cool";

        static LocString ToxicSand = DirtyPrefix + "Dirt";
        static LocString ToxicMud = DirtyPrefix + "Mud";

        static LocString Tungsten = "W";

        static LocString Uranium = "U";
        static LocString UraniumOre = Uranium + OreSuffix;
        static LocString DepletedUranium = "D-" + Uranium;
        static LocString EnrichedUranium = "E-" + Uranium;

        static LocString Niobium = "Nb";

        static LocString ViscoGel = "V-Gel";

        static LocString Water = "H<sub>2</sub>O";
        static LocString Steam = Water;
        static LocString SaltWater = "S-" + Water;
        static LocString DirtyWater = DirtyPrefix + Water;
        static LocString Ice = "Ice";
        static LocString BrineIce = "B-" + Ice;
        static LocString CrushedIce = "C-" + Ice;
        static LocString DirtyIce = DirtyPrefix + Ice;
    }

    public class OPTIONS : AStrings<OPTIONS>
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
