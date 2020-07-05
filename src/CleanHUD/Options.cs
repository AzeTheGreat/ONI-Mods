using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;

namespace CleanHUD
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option("Disable Vignette", "When true, the vignette is removed from the screen.")]
        [JsonProperty]
        public bool IsVignetteDisabled { get; set; }

        [Option("Disable Watermark", "When true, the build watermark is removed from the screen.")]
        [JsonProperty]
        public bool IsWatermarkDisabled { get; set; }

        [Option("Small Buttons", "When true, the management buttons all use the small format.")]
        [JsonProperty]
        public bool UseSmallButtons { get; set; }

        public Options()
        {
            IsVignetteDisabled = false;
            IsWatermarkDisabled = true;
            UseSmallButtons = false;
        }
    }
}
