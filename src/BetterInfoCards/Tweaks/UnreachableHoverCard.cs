using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static HarmonyLib.Code;

namespace BetterInfoCards;

public static class DetectRunStart_Patch
{
    public static IEnumerable<CodeInstruction> ChildTranspiler(IEnumerable<CodeInstruction> codes)
    {
        return new CodeMatcher(codes)
            .MatchStartForward(CodeMatch.Calls((Component _) => _.GetComponent<ChoreConsumer>()))
            .MatchStartForward(Ldc_I4_0)
            .InsertAfter([
                CodeInstruction.LoadArgument(0),
                CodeInstruction.CallClosure(DrawUnreachableCard)])
            .InstructionEnumeration();

        static void DrawUnreachableCard(SelectToolHoverTextCard instance)
        {
            var unreachable = Db.Get().MiscStatusItems.PickupableUnreachable;

            if (instance.overlayValidHoverObjects.Any(x => x.HasStatusItem(unreachable)))
            {
                HoverTextDrawer drawer = HoverTextScreen.Instance.drawer;

                drawer.BeginShadowBar();
                drawer.DrawIcon(unreachable.sprite.sprite, instance.Styles_BodyText.Standard.textColor, 18, -6);
                drawer.AddIndent(8);
                drawer.DrawText(unreachable.Name.ToUpper(), instance.Styles_Title.Standard);
                drawer.EndShadowBar();
            }
        }
    }
}
