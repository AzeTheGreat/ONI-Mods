using AzeLib;
using PeterHan.PLib.Options;

namespace NoResearchAlerts
{
    [RestartRequired]
    class Options : BaseOptions<Options>
    {
        [Option] public Mode AlertMode { get; set; }
        [Option] public bool SuppressMessage { get; set; }

        public enum Mode
        {
            [Option("No Alerts", "Research alerts will never show")]
            None,
            [Option("Fast Clear", "Research alerts will be shown, but are cleared after looking at the category.")]
            FastClear
        }

        public Options()
        {
            AlertMode = Mode.None;
            SuppressMessage = false;
        }
    }
}
