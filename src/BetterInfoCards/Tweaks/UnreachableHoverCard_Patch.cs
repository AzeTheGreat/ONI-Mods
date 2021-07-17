using AzeLib.Extensions;
﻿using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterInfoCards
{
    public static class DetectRunStart_Patch
    {
        public static IEnumerable<CodeInstruction> ChildTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo targetMethod = AccessTools.Method(typeof(Component), nameof(Component.GetComponent)).MakeGenericMethod(typeof(ChoreConsumer));

            bool afterTarget = false;

            foreach (CodeInstruction i in instructions)
            {
                if (i.Is(OpCodes.Callvirt, targetMethod))
                    afterTarget = true;

                if (afterTarget && i.OpCodeIs(OpCodes.Ldc_I4_0))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0) { labels = new List<Label>(i.labels) };
                    i.labels.Clear();

                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.overlayValidHoverObjects)));

                    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(DetectRunStart_Patch), nameof(DetectRunStart_Patch.DrawUnreachableCard)));
                    afterTarget = false;
                }

                yield return i;
            }
        }

        private static void DrawUnreachableCard(SelectToolHoverTextCard instance, List<KSelectable> overlayValidHoverObjects)
        {
            var unreachable = Db.Get().MiscStatusItems.PickupableUnreachable;

            if (overlayValidHoverObjects.Any(x => x.HasStatusItem(unreachable)))
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
