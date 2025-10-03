using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards.Process
{
    public static class ModifyHits
    {
        private static List<DisplayCard> displayCards = new List<DisplayCard>();

        [HarmonyPatch]
        private static class ChangeHits_Patch
        {
            private static KSelectable priorSelected;

            static MethodBase TargetMethod()
            {
                const string methodName = "GetObjectUnderCursor";
                var getObjectMethod = AccessTools.AllMethods(typeof(InterfaceTool))
                    .FirstOrDefault(method => method.Name == methodName && MatchesGetObjectUnderCursorSignature(method));

                if (getObjectMethod == null)
                {
                    Debug.LogWarning($"[BetterInfoCards] Failed to locate {nameof(InterfaceTool)}.{methodName}; skipping patch.");
                    return null;
                }

                if (!getObjectMethod.IsGenericMethodDefinition)
                    return getObjectMethod;

                var genericArguments = getObjectMethod.GetGenericArguments();

                // U56 introduces a third generic argument: InterfaceTool.IntersectionCandidate (ordered after Intersection).
                Type[] genericTypes = null;

                switch (genericArguments.Length)
                {
                    case 1:
                        genericTypes = new[] { typeof(KSelectable) };
                        break;
                    case 2:
                        genericTypes = new[] { typeof(KSelectable), typeof(InterfaceTool.Intersection) };
                        break;
                    case 3:
                        var intersectionCandidateType = typeof(InterfaceTool)
                            .GetNestedType("IntersectionCandidate", BindingFlags.Public | BindingFlags.NonPublic);

                        if (intersectionCandidateType == null)
                        {
                            Debug.LogWarning("[BetterInfoCards] Failed to locate InterfaceTool.IntersectionCandidate; skipping patch.");
                            return null;
                        }

                        genericTypes = new[]
                        {
                            typeof(KSelectable),
                            typeof(InterfaceTool.Intersection),
                            intersectionCandidateType
                        };
                        break;
                }

                if (genericTypes != null)
                    return getObjectMethod.MakeGenericMethod(genericTypes);

                Debug.LogWarning($"[BetterInfoCards] Unexpected generic arity ({genericArguments.Length}) for {nameof(InterfaceTool)}.{methodName}; skipping patch.");
                return null;
            }

            private static bool MatchesGetObjectUnderCursorSignature(MethodInfo method)
            {
                var parameters = method.GetParameters();

                if (parameters.Length < 1 || parameters.Length > 3)
                    return false;

                if (parameters[0].ParameterType != typeof(bool))
                    return false;

                if (parameters.Length == 1)
                    return true;

                var optionalParameters = parameters.Skip(1).ToArray();

                if (optionalParameters.Any(parameter => !parameter.IsOptional))
                    return false;

                var componentParameter = optionalParameters.FirstOrDefault(parameter =>
                    typeof(Component).IsAssignableFrom(parameter.ParameterType)
                    || parameter.Name != null && parameter.Name.IndexOf("component", StringComparison.OrdinalIgnoreCase) >= 0);

                if (componentParameter == null)
                    return false;

                if (optionalParameters.Length == 1)
                    return true;

                return optionalParameters.Any(parameter =>
                    parameter != componentParameter
                    && parameter.Name != null
                    && (parameter.Name.IndexOf("predicate", StringComparison.OrdinalIgnoreCase) >= 0
                        || parameter.Name.IndexOf("filter", StringComparison.OrdinalIgnoreCase) >= 0
                        || parameter.Name.IndexOf("test", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            static void Postfix(bool cycleSelection, ref KSelectable __result, List<InterfaceTool.Intersection> ___intersections)
            {
                if (!cycleSelection || !displayCards.Any())
                    return;

                // Remove semi-duplicate WorldSelectionCollider from end of hits
                // Must remove the second to last since it is displayed, and thus doesn't get re-added as an undisplayed selectable
                // Not sure what causes the duplicate, but it seems to only be missing if only an element is selectable
                if(___intersections.Count >= 2)
                    ___intersections.RemoveAt(___intersections.Count - 2);

                var selectables = GetPotentialSelectables(Options.Opts.UseBaseSelection, ___intersections);
                var index = GetIndex(priorSelected, selectables);
                var newSelected = GetGroupedSel() ?? GetHoverSel() ?? GetCardSel();

                __result = priorSelected = newSelected;

                // Other modifiers will not work easily, because OnLeftClickDown is not invoked if other modifiers are held.
                KSelectable GetGroupedSel() => 
                    Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? GetNextSelectable(selectables, index) : null;
                KSelectable GetHoverSel() => 
                    index == -1 && Options.Opts.ForceFirstSelectionToHover ? SelectTool.Instance.hover : null;
                KSelectable GetCardSel() => GetNextCard(selectables, index);
            }

            private static List<List<KSelectable>> GetPotentialSelectables(bool restrictToVanilla, List<InterfaceTool.Intersection> intersections)
            {
                var dispCardSels = displayCards.Select(x => x.GetAllSelectables())
                    .Where(x => !x.Contains(null));
                var vanillaSels = intersections.Select(x => x.component as KSelectable);

                if (restrictToVanilla)
                    dispCardSels = dispCardSels.Where(x => x.Intersect(vanillaSels).Any());

                var nonDispSels = vanillaSels.Where(x => !dispCardSels.SelectMany(y => y).Contains(x));
                return dispCardSels.Concat(nonDispSels.Select(x => new List<KSelectable> { x })).ToList();
            }

            private static KSelectable GetNextCard(List<List<KSelectable>> potentialSelectables, int index)
            {
                if (!potentialSelectables.Any())
                    return null;

                if (++index >= potentialSelectables.Count)
                    index = 0;

                return potentialSelectables[index].FirstOrDefault();
            }

            private static KSelectable GetNextSelectable(List<List<KSelectable>> potentialSelectables, int index)
            {
                if (!potentialSelectables.Any() || index == -1)
                    return null;

                var selectedGroup = potentialSelectables[index];

                var i = selectedGroup.IndexOf(priorSelected);
                if (++i >= selectedGroup.Count)
                    i = 0;

                var selected = selectedGroup[i];
                if (selected != priorSelected)
                    return selected;
                return null;
            }

            private static int GetIndex(KSelectable priorSelected, List<List<KSelectable>> newSelectables)
            {
                for (int i = 0; i < newSelectables.Count; i++)
                    if (newSelectables[i].Contains(priorSelected))
                        return i;

                return -1;
            }

            [HarmonyPatch(typeof(SelectTool), nameof(SelectTool.Select))]
            private class ResetSelection_Patch
            {
                static void Postfix(KSelectable new_selected)
                {
                    if (new_selected == null)
                        priorSelected = null;
                }
            }
        }

        public static void Update(List<DisplayCard> dispCards) => displayCards = dispCards;
    }
}
