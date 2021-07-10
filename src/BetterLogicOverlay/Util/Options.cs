using AzeLib;
using HarmonyLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System.IO;
using System.Reflection;

namespace BetterLogicOverlay
{
    [RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option] public bool FixWireOverwrite { get; set; }
        [Option] public bool DisplayLogicSettings { get; set; }

        [JsonIgnore] public bool isTranslated = true;

        public Options()
        {
            FixWireOverwrite = true;
            DisplayLogicSettings = true;
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
