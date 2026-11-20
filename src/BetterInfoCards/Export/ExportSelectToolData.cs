using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards;

public class ExportSelectToolData
{
    private static KSelectable curSelectable;
    private static (string id, object data) curTextInfo = (string.Empty, null);

    public static KSelectable ConsumeSelectable()
    {
        var sel = curSelectable;
        curSelectable = null;
        return sel;
    }

    public static (string id, object data) ConsumeTextInfo()
    {
        var ti = curTextInfo;
        curTextInfo = (string.Empty, null);
        return ti;
    }

    public class GetSelectInfo_Patch
    {
        public static IEnumerable<CodeInstruction> ChildTranspiler(IEnumerable<CodeInstruction> codes)
        {
            // Insert exports for: Selectable, Title, Germs, Status (x2), Temp
            // Game updates and other mods frequently change this method, so a target that can't be found is skipped with a warning instead of failing the whole patch.
            return new CodeMatcher(codes)
                .Do(MatchStart).InsertIfValid("Selectable", CodeInstruction.CallClosure(static (KSelectable selectable) => curSelectable = selectable))
                .Do(MatchStart).Do(cm => FindInsertGOExport(cm, CodeMatch.Calls(() => GameUtil.GetUnitFormattedName(null, false)), ConverterManager.title))
                // Germ text is built in a pooled string builder and cached in selectableDiseaseTexts just before it's drawn.
                .Do(MatchStart).Do(cm => FindInsertGOExport(cm, new CodeMatch(ci => ci.operand is FieldInfo { Name: nameof(SelectToolHoverTextCard.selectableDiseaseTexts) }), ConverterManager.germs))
                // The status item group is fetched once, then drawn in two loops: "Main" category entries first, then everything else.
                .Do(MatchStart).Do(cm => cm.MatchForwardIfValid(CodeMatch.Calls((KSelectable _) => _.GetStatusItemGroup))).Do(FindInsertStatusExport).Do(FindInsertStatusExport)
                .Do(MatchStart).Do(cm => FindInsertGOExport(cm, CodeMatch.Calls(() => GameUtil.GetFormattedTemperature), ConverterManager.temp))
                .InstructionEnumeration();

            // Match the start of the region this code is interested in.
            // This is the code that iterates overlayValidHoverObjects.
            static void MatchStart(CodeMatcher cm) => cm
                .Start().MatchStartForward(CodeMatch.Calls((Component _) => _.GetComponent<PrimaryElement>));

            // Exports are consumed by the next DrawText, so export right before the DrawText that follows the target.
            static void FindInsertGOExport(CodeMatcher cm, CodeMatch target, string matchID) => cm
                .MatchForwardIfValid(target)
                .MatchForwardIfValid(DrawText())
                .InsertIfValid(matchID, CodeInstruction.CallClosure(() => ExportGO(matchID)));

            // Each status loop checks IsStatusItemWarning (twice) right before drawing the entry.
            // Export at the first check, then skip to that loop's DrawText so the next call finds the next loop.
            // StatusItemGroup.Entry GetName is a better target, but it loads the entry as an address which is harder to work with.
            static void FindInsertStatusExport(CodeMatcher cm) => cm
                .MatchForwardIfValid(CodeMatch.Calls((SelectToolHoverTextCard _) => _.IsStatusItemWarning))
                .InsertIfValid("Status", CodeInstruction.CallClosure(static (StatusItemGroup.Entry entry) =>
                {
                    Export(entry.item.Id, entry.data);
                    return entry;
                }))
                .MatchForwardIfValid(DrawText());

            static CodeMatch DrawText() => CodeMatch.Calls((HoverTextDrawer _) => _.DrawText(null, null));
        }

        // These are technically public API... don't touch.
        private static void Export(string name, object data) => curTextInfo = (name, data);
        private static void ExportGO(string name) => Export(name, curSelectable.gameObject);
    }
}

file static class CodeMatcherExtensions
{
    // Once a match fails, stay invalid so later steps can't match from the wrong place.
    public static CodeMatcher MatchForwardIfValid(this CodeMatcher cm, CodeMatch match) =>
        cm.IsValid ? cm.MatchStartForward(match) : cm;

    public static CodeMatcher InsertIfValid(this CodeMatcher cm, string exportID, CodeInstruction instruction)
    {
        if (cm.IsInvalid)
        {
            Debug.LogWarning($"[Better Info Cards] Couldn't find where to export \"{exportID}\" in SelectToolHoverTextCard.UpdateHoverElements; " +
                "that info won't be combined when cards are grouped. The game or another mod has likely changed this method.");
            return cm;
        }

        // Move labels onto the export so jumps to the target still run it.
        cm.Insert(instruction).Instruction.MoveLabelsFrom(cm.InstructionAt(1));
        return cm;
    }
}
