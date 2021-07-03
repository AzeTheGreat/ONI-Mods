using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace NoDoorIdle
{
    [RestartRequired]
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option("Idle In Doors", "When true, duplicants can idle inside of doors.")]
        [JsonProperty] public bool CanIdleInDoors { get; set; }

        [Option("Idle Traverse Doors", "When true, duplicants can cross doors while idling.")]
        [JsonProperty] public bool CanIdleTraverseDoors { get; set; }

        [Option("Fix Idle Trapping", "When true, fixes a base game bug that causes dupes to get stuck at checkpoints when idle.")]
        [JsonProperty] public bool FixIdleTrapping { get; set; }

        public Options()
        {
            CanIdleInDoors = false;
            CanIdleTraverseDoors = true;
            FixIdleTrapping = true;
        }
    }
}
