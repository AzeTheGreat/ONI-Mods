using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace FixedCameraPan
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option("Pan Speed", "Set to the framerate at which you find the speed comfortable.  At 60, the camera will always move at the same speed as it moves when the game is at 60 FPS.")]
        [Limit(1, 999)]
        [JsonProperty]
        public float PanSpeed { get; set; }

        public Options()
        {
            PanSpeed = 80f;
        }
    }
}
