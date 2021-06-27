using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
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

        [Option("Mode", "The mode to use.")]
        [JsonProperty]
        public Mode AlertMode { get; set; }
        [Option("Prevent Notification", "If true, prevents notifications in the top left due to completed research.")]
        [JsonProperty]
        public bool SuppressMessage { get; set; }

        public Options()
        {
            AlertMode = Mode.None;
            SuppressMessage = false;
        }
    }
}
