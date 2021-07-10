using HarmonyLib;

namespace NoDoorIdle
{
    [HarmonyPatch(typeof(IdleCellQuery), nameof(IdleCellQuery.IsMatch))]
    public class ChangeIdleTargetCells
    {
        static bool Prepare() => !Options.Opts.CanIdleInDoors;
     
        static void Prefix(IdleCellQuery __instance, out int __state)
        {
            // State caches the value of the targetCell before it is changed.
            __state = __instance.targetCell;
        }

        static void Postfix(IdleCellQuery __instance, int cell, int __state)
        {
            // Revert the target cell if it should not be idled in.
            if (ShouldNotIdleInCell(cell))
                __instance.targetCell = __state;
        }

        // Checking the cell above ensures dupes won't idle with their heads in horizontal doors.
        public static bool ShouldNotIdleInCell(int cell) => Grid.HasDoor[cell] || Grid.HasDoor[Grid.CellAbove(cell)];
    }

    // By default, idlers only move at a random lengthy interval. MoveToSafety handles moving them from "unsafe" situations.
    // Adds an additional transition to move the dupe if it should not be idling in its current cell.
    // Not a performance concern: the game already runs the path query every Brain update, and door checks are cheap.
    [HarmonyPatch(typeof(IdleChore.States), nameof(IdleChore.States.InitializeStates))]
    class UpdateIdlePosition
    {
        static bool Prepare() => !Options.Opts.CanIdleInDoors;

        static void Postfix(IdleChore.States __instance) => __instance.idle.onfloor.Transition(__instance.idle.move, 
            (IdleChore.StatesInstance smi) =>
            {
                var shouldNotIdle = ChangeIdleTargetCells.ShouldNotIdleInCell(Grid.PosToCell(smi));
                // Building a door on a dupe can result in having an idle cell that is not yet updated, causing an infinite SM loop.
                // Updating the sensor avoids this.  Checking HasIdleCell first avoids unnecessary extra updates.
                if (smi.HasIdleCell() && shouldNotIdle)
                    smi.idleCellSensor.Update();
                return smi.HasIdleCell() && shouldNotIdle;
            });
    }
}
