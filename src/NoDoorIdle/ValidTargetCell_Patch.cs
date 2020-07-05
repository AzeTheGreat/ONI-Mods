using Harmony;
using PeterHan.PLib;

namespace NoDoorIdle
{
    [HarmonyPatch(typeof(IdleCellQuery), nameof(IdleCellQuery.IsMatch))]
    public class ValidTargetCell_Patch
    {
        static bool Prepare() => !Options.Opts.CanIdleInDoors;
     
        static void Postfix(ref int ___targetCell)
        {
            if (___targetCell != Grid.InvalidCell && Grid.HasDoor[___targetCell])
                ___targetCell = Grid.InvalidCell;
        }
    }
}
