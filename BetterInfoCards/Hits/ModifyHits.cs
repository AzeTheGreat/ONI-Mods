using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    public class ModifyHits
    {
        public static ModifyHits Instance { get; set; }

        private int localIndex = -1;
        private List<DisplayCard> displayCards;
        private DisplayCard priorSelected;

        [HarmonyPatch]
        private class ChangeHits_Patch
        {
            static MethodBase TargetMethod() => AccessTools.Method(typeof(InterfaceTool), "GetObjectUnderCursor").MakeGenericMethod(typeof(KSelectable));

            static void Postfix(bool cycleSelection, ref KSelectable __result, List<InterfaceTool.Intersection> ___intersections)
            {
                if (__result == null || !Instance.displayCards.Any())
                    return;

                List<KSelectable> validSelectables = ___intersections.Select(x => x.component as KSelectable).ToList();

                if (cycleSelection)
                {
                    int i = 0;
                    bool isValidSelection = false;
                    do
                    {
                        Instance.localIndex++;
                        if (Instance.localIndex > Instance.displayCards.Count - 1)
                            Instance.localIndex = 0;
                        i++;
                        KSelectable selectable = Instance.displayCards[Instance.localIndex].GetTopSelectable();
                        isValidSelection = selectable != null && validSelectables.Contains(selectable);
                    } while (!isValidSelection && i < Instance.displayCards.Count);

                    Instance.priorSelected = Instance.displayCards[Instance.localIndex];
                }

                if (Instance.localIndex != -1)
                    __result = Instance.displayCards[Instance.localIndex].GetTopSelectable();
            }
        }

        public void Update(List<DisplayCard> displayCards)
        {
            localIndex = GetNewIndex(priorSelected, displayCards);
            this.displayCards = displayCards;
        }

        private int GetNewIndex(DisplayCard priorSelected, List<DisplayCard> newCards)
        {
            if (priorSelected == null)
                return -1;

            var priorSelectables = priorSelected.GetAllSelectables();
            for (int i = 0; i < newCards.Count; i++)
            {
                var newSelectables = newCards[i].GetAllSelectables();

                if (priorSelectables.Intersect(newSelectables).Any())
                    return i;
            }

            return -1;
        }

        [HarmonyPatch(typeof(SelectTool), nameof(SelectTool.Select))]
        private class ResetIndex_Patch
        {
            static void Postfix(KSelectable new_selected)
            {
                if(new_selected == null)
                {
                    Instance.localIndex = -1;
                    Instance.priorSelected = null;
                }
            }
        }
    }
}
