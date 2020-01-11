using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    public class ModifyWidgets
    {
        public static ModifyWidgets Instance { get; set; }

        private List<InfoCard> infoCards = new List<InfoCard>();
        private InfoCards infoCardManager = new InfoCards();

        [HarmonyPatch]
        private class Test
        {
            // HoverTextDrawer.Pool.Draw
            static MethodBase TargetMethod() => AccessTools.FirstInner(typeof(HoverTextDrawer), x => x.IsGenericType).MakeGenericType(typeof(object)).GetMethod("Draw");

            static void Postfix(Entry __result, GameObject ___prefab)
            {
                InfoCard card = Instance.infoCards.Last();
                card.AddWidget(__result, ___prefab);
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginShadowBar))]
        private class BeginShadowBar_Patch
        {
            static void Postfix()
            {
                Instance.infoCards.Add(new InfoCard());
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        private class EndShadowBar_Patch
        {
            static void Postfix()
            {
                List<TextInfo> entries = CollectHoverInfo.Instance.activeStatuses[Instance.infoCards.Count - 1];
                KSelectable selectable = CollectHoverInfo.Instance.selectables[Instance.infoCards.Count - 1];

                InfoCard card = Instance.infoCards.Last();
                card.AddData(entries, selectable);
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginDrawing))]
        private class BeginDrawing_Patch
        {
            static void Postfix()
            {
                Instance.infoCards.Clear();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
        private class EditWidgets_Patch
        {
            static void Postfix()
            {
                Instance.infoCardManager.UpdateData(Instance.infoCards);
                Instance.infoCardManager.Update();
            }
        }
    }
}
