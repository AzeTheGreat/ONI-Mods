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

        // TODO: Correct to be string keys once they properly work in PLib.
        public enum SaveIntervals
        {
            [Option("Never")] Never,
            [Option("50 Cycles")] Fifty,
            [Option("20 Cycles")] Twenty,
            [Option("10 Cycles")] Ten,
            [Option("5 Cycles")] Five,
            [Option("2 Cycles")] Two,
            [Option("Every Cycle")] Every
        }

        public enum Resolutions
        {
            [Option("Disabled")] Disabled,
            [Option("256 x 358")] R256,
            [Option("512 x 768")] R512,
            [Option("1024 x 1536")] R1024,
            [Option("2048 x 3072")] R2048,
            [Option("4096 x 6144")] R4096,
            [Option("8192 x 12288")] R8192
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
