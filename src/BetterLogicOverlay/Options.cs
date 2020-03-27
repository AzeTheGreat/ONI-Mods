using Harmony;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Datafiles;
using System.IO;
using System.Reflection;
using AzeLib;

namespace BetterLogicOverlay
{
    [JsonObject(MemberSerialization.OptIn)]
    [PeterHan.PLib.Options.RestartRequired]
    public class Options : BaseOptions<Options>
    {
        public bool isTranslated = true;

        [Option("Fix Wire Overwriting", "If true, green signals will not make a red output port display as green.")]
        [JsonProperty]
        public bool FixWireOverwrite { get; set; }

        [Option("Display Logic Settings", "If true, logic gate and sensor settings will be displayed in the automation overlay.")]
        [JsonProperty]
        public bool DisplayLogicSettings { get; set; }

        [Option("Font Size", "Sets the font size for the automation overlay.  Default of 9")]
        [JsonProperty]
        [Limit(3, 9)]
        public float FontSize { get; set; }

        public enum FixMode
        {
            [Option("Hold Selected", "Continue holding the same tool. Reqiores Restart.")]
            Hold,
            [Option("Deselect Selected", "Deselect the tool.  Closest to vanilla behavior. Requires Restart.")]
            Close
        }

        public Options()
        {
            FixWireOverwrite = true;
            DisplayLogicSettings = true;
            FontSize = 9;
        }

        public static void OnLoad()
        {
            PLocalization.Register();
        }

        private bool GetTranslatedStatus()
        {
            Localization.Locale locale = Localization.GetLocale();
            if (locale == null)
                return true;

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/translations/";
            string fullPath = path + locale.Code + ".po";
            return File.Exists(fullPath);
        }

        [HarmonyPatch(typeof(Localization), nameof(Localization.SetLocale))]
        private class TestLocale { static void Postfix() => Opts.isTranslated = Opts.GetTranslatedStatus(); }
    }
}
