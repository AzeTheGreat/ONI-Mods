using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

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

            // Find the push code instruction from the true branch; used as the splice target.
            var pushTrue = codes.First(i => i.Calls(preventIdleTrav_getter)).FindNext(codes, i => i.OpCodeIs(OpCodes.Ldc_I4_1));

            return codes
                // Add the call to the splice.
                .Manipulator(
                    i => i == pushTrue,
                    i => new[] {
                        i.MakeNop(),
                        hasSuitMarker_storeToLoc.GetLoadFromStore(),
                        agentRequiresSuit_storeToLoc.GetLoadFromStore(),
                        CodeInstruction.Call(typeof(PreventIdleTrapping), nameof(Splice))})
                // Remove branches that occur after indexer calls so that the results fall through to the splice.
                .Manipulator(
                    i => i.Calls(preventIdleTrav_getter),
                    (codes, i) => codes.FindNext(i, i => i.Branches(out _)).MakeNop());
        }

        // Allow leaving an idle cell, and allow entering another idle cell if the source is an idle cell.
        // Ensure idle dupes do not use suits (fixes an edge case exposed by allowing dupes to leave idle cells).
        private static bool Splice(bool preventIdleOnToCell, bool preventIdleOnFromCell, bool hasSuitMarker, bool agentRequiresSuit) 
            => (preventIdleOnToCell && !preventIdleOnFromCell) || (hasSuitMarker && agentRequiresSuit);
    }
}
