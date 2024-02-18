using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace BuildMenuSearchHotkey
{
    public class Options : BaseOptions<Options>
    {
        [JsonIgnore][Option] public LocText PointToKeybindMenuLabel { get; set; }
        [Option] public bool HotkeyWorksWhenBuildMenuClosed { get; set; } = true;
    }
}
