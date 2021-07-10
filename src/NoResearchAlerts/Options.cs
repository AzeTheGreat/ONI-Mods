using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace NoResearchAlerts
{
    [JsonObject(MemberSerialization.OptIn)]
    [RestartRequired]
    class Options : BaseOptions<Options>
    {
        public enum Mode
        {
            [Option("No Alerts", "Research alerts will never show")]
            None,
            [Option("Fast Clear", "Research alerts will be shown, but are cleared after looking at the category.")]
            FastClear
        }

        [Option]
        [JsonProperty]
        public Mode AlertMode { get; set; }
        [Option]
        [JsonProperty]
        public bool SuppressMessage { get; set; }

        public Options()
        {
            AlertMode = Mode.None;
            SuppressMessage = false;
        }
    }
}
