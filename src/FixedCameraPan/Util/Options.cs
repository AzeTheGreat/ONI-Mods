using AzeLib;
using PeterHan.PLib.Options;

namespace FixedCameraPan
{
    public class Options : BaseOptions<Options>
    {
        [Option] [Limit(1, 999)] public float PanSpeed { get; set; }

        public Options()
        {
            PanSpeed = 80f;
        }
    }
}
