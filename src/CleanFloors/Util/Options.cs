using AzeLib;
using PeterHan.PLib.Options;

namespace CleanFloors;

[RestartRequired]
public class Options : BaseOptions<Options>
{
    [Option] public bool RemoveOutsideCorners { get; set; } = true;
    [Option] public bool RemoveInsideCorners { get; set; } = false;
    [Option] public bool CornersBelowTops { get; set; } = false;
}
