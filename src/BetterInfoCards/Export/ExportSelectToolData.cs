using HarmonyLib;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            return new CodeMatcher(codes)
                .Do(MatchStart).Insert(CodeInstruction.CallClosure(static (KSelectable selectable) => curSelectable = selectable))
                .Do(MatchStart).Do(cm => FindInsertGOExport(cm, () => GameUtil.GetUnitFormattedName(null, false), ConverterManager.title))
                .Do(MatchStart).Do(cm => FindInsertGOExport(cm, () => GameUtil.GetFormattedDisease, ConverterManager.germs))
                .Do(MatchStart).Do(FindInsertStatusExport).Do(FindInsertStatusExport)
                .Do(MatchStart).Do(cm => FindInsertGOExport(cm, () => GameUtil.GetFormattedTemperature, ConverterManager.temp))
                .InstructionEnumeration();

            // Match the start of the region this code is interested in.
            // This is the code that iterates overlayValidHoverObjects.
            static void MatchStart(CodeMatcher cm) => cm
                .Start().MatchStartForward(CodeMatch.Calls((Component _) => _.GetComponent<PrimaryElement>));

            static void FindInsertGOExport(CodeMatcher cm, LambdaExpression target, string matchID) => cm
                .MatchStartForward(CodeMatch.Calls(target))
                .Insert(CodeInstruction.CallClosure(() => ExportGO(matchID)));

            // StatusItemGroup.Entry GetName is a better target, but it loads the entry as an address which is harder to work with.
            static void FindInsertStatusExport(CodeMatcher cm) => cm
                .MatchStartForward(CodeMatch.Calls((KSelectable _) => _.GetStatusItemGroup))
                .MatchStartForward(CodeMatch.Calls((SelectToolHoverTextCard _) => _.IsStatusItemWarning))
                .InsertAndAdvance(CodeInstruction.CallClosure(static (StatusItemGroup.Entry entry) =>
                {
                    Export(entry.item.Id, entry.data);
                    return entry;
                })).Advance();
        }

        // These are technically public API... don't touch.
        private static void Export(string name, object data) => curTextInfo = (name, data);
        private static void ExportGO(string name) => Export(name, curSelectable.gameObject);
    }
}