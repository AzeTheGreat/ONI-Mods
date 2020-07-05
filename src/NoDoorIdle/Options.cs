using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace NoDoorIdle
{
    [RestartRequired]
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option("Idle In Doors", "When true, duplicants can idle inside of doors.")]
        [JsonProperty]
        public bool CanIdleInDoors { get; set; }

        [Option("Idle Traverse Doors", "When true, duplicants can cross doors while idling.")]
        [JsonProperty]
        public bool CanIdleTraverseDoors { get; set; }

        public Options()
        {
            CanIdleInDoors = false;
            CanIdleTraverseDoors = true;
        }
    }
}
