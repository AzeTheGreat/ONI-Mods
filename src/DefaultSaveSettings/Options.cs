using AzeLib;
using PeterHan.PLib.Options;

namespace DefaultSaveSettings
{
    public class Options : BaseOptions<Options>
    {
        [Option] public bool EnableProximity { get; set; }
        [Option] public SaveIntervals AutoSaveInterval { get; set; }
        [Option] [Limit(0, 6)] public Resolutions TimelapseResolution { get; set; }

        [Option] public bool EnableAutoDisinfect { get; set; }
        [Option] [Limit(0, 1000000)] public int DisinfectMinGermCount { get; set; }

        public enum SaveIntervals
        {
            [Option("STRINGS.UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_NEVER")] Never,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.AUTOSAVEFREQ.FIFTY")] Fifty,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.AUTOSAVEFREQ.TWENTY")] Twenty,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.AUTOSAVEFREQ.TEN")] Ten,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.AUTOSAVEFREQ.FIVE")] Five,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.AUTOSAVEFREQ.TWO")] Two,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.AUTOSAVEFREQ.EVERY")] Every
        }

        public enum Resolutions
        {
            [Option("STRINGS.UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_DISABLED_DESCRIPTION")] Disabled,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.RESOLUTION.R256")] R256,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.RESOLUTION.R512")] R512,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.RESOLUTION.R1024")] R1024,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.RESOLUTION.R2048")] R2048,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.RESOLUTION.R4096")] R4096,
            [Option("STRINGS.DEFAULTSAVESETTINGS.OPTIONS.RESOLUTION.R8192")] R8192
        }

        public Options()
        {
            EnableProximity = true;
            AutoSaveInterval = SaveIntervals.Every;
            TimelapseResolution = Resolutions.R512;
            EnableAutoDisinfect = true;
            DisinfectMinGermCount = 10000;
        }
    }
}
