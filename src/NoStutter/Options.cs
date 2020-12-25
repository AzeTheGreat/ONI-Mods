using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;

namespace NoStutter
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        public enum DisableMode
        {
            [Option("Prevent", "The achievement will be completely disabled and it will be impossible to achieve.")]
            Prevent,
            [Option("Instant Success", "The achievement will be immediately achieved.")]
            InstantSuccess
        }

        [Option("Only In Debug", "If checked, the stutter (and thus achievement) will only be disabled if debug mode is activated.")]
        [JsonProperty]
        public bool OnlyInDebug { get; set; }

        [Option("Achievement Disabling Mode", "Switch between achievement disabling modes.")]
        [JsonProperty]
        public DisableMode Mode { get; set; }

        public Options()
        {
            OnlyInDebug = true;
            Mode = DisableMode.Prevent;
        }
    }
}
