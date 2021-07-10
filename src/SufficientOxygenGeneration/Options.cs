using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace SufficientOxygenGeneration
{
    public class Options : BaseOptions<Options>
    {
        [Option] public OxygenThresholMode Mode { get; set; }
        [Option] [Limit(0.0001f, 9999f)] public float ConstantThreshold { get; set; }
        [Option] [Limit(0f, 1f)] public float RatioThreshold { get; set; }

        [JsonIgnore] public float TimeDelay { get; set; }

        public enum OxygenThresholMode
        {
            [Option("Constant", "If you are underproducing by x amount in the current cycle, the notification will trigger.")]
            Constant,
            [Option("Ratio", "If the ratio of oxygen produced to oxygen consumed is below x, the notification will trigger.")]
            Ratio,
            [Option("Off", "The notification will never trigger")]
            Off
        }

        public Options()
        {
            ConstantThreshold = -10f;
            RatioThreshold = 0.9f;
            TimeDelay = 30f;
        }
    }
}
