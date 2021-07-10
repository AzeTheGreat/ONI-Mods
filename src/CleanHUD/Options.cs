using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace CleanHUD
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option(Format = "P0")]
        [Limit(0, 1)]
        [JsonProperty]
        public float VignetteAlpha { get; set; }

        [Option(Format = "P0")]
        [Limit(0, 1)]
        [JsonProperty]
        public float AlertVignetteAlpha { get; set; }

        [Option]
        [JsonProperty]
        public bool IsWatermarkDisabled { get; set; }

        [Option]
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
