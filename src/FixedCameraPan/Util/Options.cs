using AzeLib;
using PeterHan.PLib.Options;

namespace FixedCameraPan
{
    public class Options : BaseOptions<Options>
    {
        [Option] [Limit(20, 500)] public int PanSpeed { get; set; }

        public Options()
        {
            PanSpeed = 130;
        }
    }
}
