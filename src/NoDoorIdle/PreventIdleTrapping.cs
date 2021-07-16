using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace NoDoorIdle
{
    // In the base game, buildings that disallow idle traversal will not allow a dupe to move off their tile.
    // This means that dupes may not move off doors, and in the base game results in idle suit dupes getting stuck at checkpoints.
    [HarmonyPatch(typeof(MinionPathFinderAbilities), nameof(MinionPathFinderAbilities.TraversePath))]
    class PreventIdleTrapping
    {
        static bool Prepare() => Options.Opts.FixIdleTrapping;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var preventIdleTrav_getter = AccessTools.PropertyGetter(typeof(Grid.NavFlagsPreventIdleTraversalIndexer), "Item");
            var tryGetSuitMarkerTags_method = AccessTools.Method(typeof(Grid), nameof(Grid.TryGetSuitMarkerFlags));
            var doesTravDirRequireSuit_method = AccessTools.Method(typeof(SuitMarker), nameof(SuitMarker.DoesTraversalDirectionRequireSuit),
                new[] { typeof(int), typeof(int), typeof(Grid.SuitMarker.Flags) });
            
            var hasSuitMarker_storeToLoc = codes.First(i => i.Calls(tryGetSuitMarkerTags_method)).FindNext(codes, i => i.IsStloc());
            var agentRequiresSuit_storeToLoc = codes.First(i => i.Calls(doesTravDirRequireSuit_method)).FindNext(codes, i => i.IsStloc());

            var lastIdleGetter = codes.Last(i => i.Calls(preventIdleTrav_getter));
            var orBranch = codes.First(i => i.Calls(preventIdleTrav_getter)).FindNext(codes, i => i.Branches(out _));

            return codes
                // Add the call to the splice just after the last idle-getter.
                .Manipulator(
                    i => i == lastIdleGetter,
                    i => new[] {
                        i,
                        hasSuitMarker_storeToLoc.GetLoadFromStore(),
                        agentRequiresSuit_storeToLoc.GetLoadFromStore(),
                        CodeInstruction.Call(typeof(PreventIdleTrapping), nameof(Splice))})
                // Remove the branch after the first idle-getter so that the result falls through to the splice.
                .Manipulator(
                    i => i == orBranch,
                    i => i.MakeNop());
        }

        // Allow leaving an idle cell, and allow entering another idle cell if the source is an idle cell.
        // Ensure idle dupes do not use suits (fixes an edge case exposed by allowing dupes to leave idle cells).
        private static bool Splice(bool preventIdleOnToCell, bool preventIdleOnFromCell, bool hasSuitMarker, bool agentRequiresSuit) 
            => (preventIdleOnToCell && !preventIdleOnFromCell) || (hasSuitMarker && agentRequiresSuit);
    }
}
