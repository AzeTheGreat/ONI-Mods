using AzeLib;
using PeterHan.PLib.Options;

namespace CleanHUD
{
    public class Options : BaseOptions<Options>
    {
        [Option(Format = "P0")] [Limit(0, 1)] public float VignetteAlpha { get; set; }
        [Option(Format = "P0")] [Limit(0, 1)] public float AlertVignetteAlpha { get; set; }
        [Option] public bool IsWatermarkDisabled { get; set; }
        [Option] public bool UseSmallButtons { get; set; }

        public Options()
        {
            VignetteAlpha = 0f;
            AlertVignetteAlpha = 20f;
            IsWatermarkDisabled = true;
            UseSmallButtons = false;
        }
    }
}
