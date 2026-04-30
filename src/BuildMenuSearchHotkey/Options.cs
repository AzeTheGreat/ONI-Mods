using AzeLib;
using PeterHan.PLib.Options;

namespace BuildMenuSearchHotkey;

public class Options : BaseOptions<Options>
{
    [Option] public bool PreferExactNameMatch { get; set; }

    public Options()
    {
        PreferExactNameMatch = true;
    }
}
