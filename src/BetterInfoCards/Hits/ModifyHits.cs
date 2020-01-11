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
        private List<DisplayCard> displayCards = new List<DisplayCard>();
        private DisplayCard priorSelected;
        private KSelectable selected;

        [HarmonyPatch]
        private class ChangeHits_Patch
        {
            static MethodBase TargetMethod() => AccessTools.Method(typeof(InterfaceTool), "GetObjectUnderCursor").MakeGenericMethod(typeof(KSelectable));

            static void Postfix(bool cycleSelection, ref KSelectable __result, List<InterfaceTool.Intersection> ___intersections)
            {
                if (__result == null || !Instance.displayCards.Any())
                    return;

                List<KSelectable> validSelectables = ___intersections.Select(x => x.component as KSelectable).ToList();
                bool cycleWithinCard = Input.GetKey(KeyCode.LeftShift);

                if(cycleSelection && cycleWithinCard)
                    Instance.selected = Instance.SelectNextValidSelectable(validSelectables);
                else if (cycleSelection)
                    Instance.selected = Instance.SelectNextValidDisplayCard(validSelectables).GetTopSelectable();

                if(Instance.localIndex != -1)
                    __result = Instance.selected;
            }
        }

        private DisplayCard SelectNextValidDisplayCard(List<KSelectable> validSelectables)
        {
            int i = 0;
            bool isValidSelection = false;
            do
            {
                if (++Instance.localIndex > Instance.displayCards.Count - 1)
                    Instance.localIndex = 0;
                i++;
                KSelectable selectable = Instance.displayCards[Instance.localIndex].GetTopSelectable();

                if (isValidSelection = selectable != null && validSelectables.Contains(selectable))
                {
                    Instance.priorSelected = Instance.displayCards[Instance.localIndex];
                    return Instance.displayCards[Instance.localIndex];
                }    
            }
            while (!isValidSelection && i < Instance.displayCards.Count);

            // No valid selectables, so go back to a valid state we "know" won't break anything.
            Instance.localIndex = -1;
            Instance.priorSelected = null;
            return null;
        }

        private KSelectable SelectNextValidSelectable(List<KSelectable> validSelectables)
        {
            if (Instance.localIndex == -1)
                return null;

            List<KSelectable> selectables = Instance.displayCards[Instance.localIndex].GetAllSelectables();
            int i = selectables.IndexOf(Instance.selected);
            if (++i > selectables.Count - 1)
                i = 0;
            return selectables[i];
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

        private int GetNewCardIndex()
        {
            if (Instance.localIndex == -1)
                return 0;

            int i = Instance.displayCards[Instance.localIndex].GetAllSelectables().IndexOf(Instance.selected);
            if (i != -1)
                return i;
            else
                return 0;
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
