using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace FixedCameraPan
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option]
        [Limit(1, 999)]
        [JsonProperty]
        public float PanSpeed { get; set; }

        public Options()
        {
            PanSpeed = 80f;
        }
    }
}
