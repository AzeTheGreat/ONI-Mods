using Harmony;
using PeterHan.PLib;

namespace NoDoorIdle
{
    [HarmonyPatch(typeof(IdleCellQuery), nameof(IdleCellQuery.IsMatch))]
    public class ValidTargetCell_Patch
    {
        static bool Prepare() => !Options.Opts.CanIdleInDoors;
     
        static void Postfix(IdleCellQuery __instance)
        {
            var targetCell = __instance.GetResultCell();
            if (targetCell != Grid.InvalidCell && Grid.HasDoor[targetCell])
                Traverse.Create(__instance).SetField("targetCell", Grid.InvalidCell);
        }
    }
}
