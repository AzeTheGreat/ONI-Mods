using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace NoDoorIdle
{
    [RestartRequired]
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option]
        [JsonProperty] public bool CanIdleInDoors { get; set; }

        [Option]
        [JsonProperty] public bool CanIdleTraverseDoors { get; set; }

        [Option]
        [JsonProperty] public bool FixIdleTrapping { get; set; }

        public Options()
        {
            CanIdleInDoors = false;
            CanIdleTraverseDoors = true;
            FixIdleTrapping = true;
        }
    }
}
