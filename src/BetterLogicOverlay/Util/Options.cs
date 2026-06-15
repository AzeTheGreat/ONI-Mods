using AzeLib;
using HarmonyLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System.IO;
using System.Reflection;

namespace BetterLogicOverlay
{
    public class Options : BaseOptions<Options>
    {
        [RestartRequired][Option] public bool FixWireOverwrite { get; set; }
        [RestartRequired][Option] public bool DisplayLogicSettings { get; set; }
        [Option] public FontOpts FontOverride { get; set; }

        [JsonIgnore] public bool isTranslated = true;

        public Options()
        {
            FixWireOverwrite = true;
            DisplayLogicSettings = true;

            FontOverride = new()
            {
                FontSize = 14,
                FontDilation = 0.45f,
                CharSpaceDelta = -2,
                LineSpaceDelta = -32,
                OutlineWidth = 0.3f
            };
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class FontOpts
        {
            [Option] public LocText FontOptsNote { get; set; }
            [Option][Limit(1, 32)] public int FontSize { get; set; }
            [Option][Limit(0, 1)] public float FontDilation { get; set; }
            [Option][Limit(-10, 0)] public int CharSpaceDelta { get; set; }
            [Option][Limit(-50, 0)] public int LineSpaceDelta { get; set; }
            [Option][Limit(0, 1)] public float OutlineWidth { get; set; }
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
