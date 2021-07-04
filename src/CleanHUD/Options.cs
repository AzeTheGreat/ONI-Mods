using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace CleanHUD
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option("Vignette Opacity", "Adjust the opacity of the vignette (standard value = 47%)", "Vignettes", Format = "P0")]
        [Limit(0, 1)]
        [JsonProperty]
        public float VignetteAlpha { get; set; }

        [Option("Warning Vignette Opacity", "Adjust the opacity of the warning vignettes (standard value = 30%) ", "Vignettes", Format = "P0")]
        [Limit(0, 1)]
        [JsonProperty]
        public float AlertVignetteAlpha { get; set; }

        [Option("Disable Watermark", "When true, the build watermark is removed from the screen.")]
        [JsonProperty]
        public bool IsWatermarkDisabled { get; set; }

        [Option("Small Buttons", "When true, the management buttons all use the small format.")]
        [JsonProperty]
        public bool UseSmallButtons { get; set; }

        public Options()
        {
            VignetteAlpha = 0f;
            AlertVignetteAlpha = 20f;
            IsWatermarkDisabled = true;
            UseSmallButtons = false;
        }
    }
}
