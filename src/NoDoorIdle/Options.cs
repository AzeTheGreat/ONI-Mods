using AzeLib;
using PeterHan.PLib.Options;

namespace NoDoorIdle
{
    [RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option] public bool CanIdleInDoors { get; set; }
        [Option] public bool CanIdleTraverseDoors { get; set; }
        [Option] public bool FixIdleTrapping { get; set; }

        public Options()
        {
            CanIdleInDoors = false;
            CanIdleTraverseDoors = true;
            FixIdleTrapping = true;
        }
    }
}
