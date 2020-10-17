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
        private List<KSelectable> priorSelected;
        private KSelectable selected;

        [HarmonyPatch]
        private class ChangeHits_Patch
        {
            static MethodBase TargetMethod() => AccessTools.Method(typeof(InterfaceTool), nameof(InterfaceTool.GetObjectUnderCursor)).MakeGenericMethod(typeof(KSelectable));

            static void Postfix(bool cycleSelection, ref KSelectable __result, List<InterfaceTool.Intersection> ___intersections)
            {
                if (!cycleSelection || __result == null || !Instance.displayCards.Any())
                    return;

                // Remove semi-duplicate WorldSelectionCollider from end of hits
                // Must remove the second to last since it is displayed, and thus doesn't get re-added as an undisplayed selectable
                // Not sure what causes the duplicate, but it seems to only be missing if only an element is selectable
                if(___intersections.Count >= 2)
                    ___intersections.RemoveAt(___intersections.Count - 2);

                var selectableGroups = Instance.displayCards.Where(x => x.GetTopSelectable() != null)   // Ignore null selectables (unreachable card)
                    .Select(x => x.GetAllSelectables())                                                 // Each group of selectables in display cards
                    .Concat(___intersections.Select(x => x.component as KSelectable)                    // Combined with undisplayed selectables
                        .Except(Instance.displayCards.SelectMany(x => x.GetAllSelectables()))           // Selectables not already in display cards
                        .Select(x => new List<KSelectable> { x }))                                      // In list form (for selecting within a group)
                    .ToList();

                Instance.localIndex = Instance.GetNewIndex(Instance.priorSelected, selectableGroups);

                if (Input.GetKey(KeyCode.LeftShift))
                    Instance.selected = Instance.SelectNextValidSelectable(selectableGroups);
                else
                    Instance.selected = Instance.SelectNextValidDisplayCard(selectableGroups);

                if (Instance.localIndex != -1)
                    __result = Instance.selected;
            }
        }

        private KSelectable SelectNextValidDisplayCard(List<List<KSelectable>> potentialSelectables)
        {
            if (++localIndex >= potentialSelectables.Count)
                localIndex = 0;

            // No valid selectables, so go back to a valid state we "know" won't break anything.
            if (localIndex == -1)
            { 
                localIndex = -1;
                priorSelected = null;
                return null;
            }

            priorSelected = potentialSelectables[localIndex];   
            return priorSelected.FirstOrDefault();
        }

        private KSelectable SelectNextValidSelectable(List<List<KSelectable>> selectableGroups)
        {
            if (Instance.localIndex == -1)
                return null;

            var selectedGroup = selectableGroups[localIndex];
            
            var i = selectedGroup.IndexOf(selected);
            if (++i >= selectedGroup.Count)
                i = 0;
            
            return selectedGroup[i];
        }

        public void Update(List<DisplayCard> displayCards)
        {
            this.displayCards = displayCards;
        }

        private int GetNewIndex(List<KSelectable> priorSelectables, List<List<KSelectable>> newSelectables)
        {
            if (priorSelectables == null)
                return -1;

            for (int i = 0; i < newSelectables.Count; i++)
                if (priorSelectables.Intersect(newSelectables[i]).Any())
                    return i;

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
