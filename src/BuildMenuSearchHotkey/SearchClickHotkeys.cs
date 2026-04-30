using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BuildMenuSearchHotkey;

[HarmonyPatch(typeof(BuildingGroupScreen), nameof(BuildingGroupScreen.OnSpawn))]
class SearchClickHotkeys
{
    static void Postfix()
    {
        // While typing, press Enter to select the first result.
        // An exact (case-insensitive) name match wins over the fuzzy-ranked top, so e.g. "Ladder" picks
        // the building literally named "Ladder" even when the fuzzy score puts something else above it.
        BuildingGroupScreen.Instance.inputField.onEndEdit.AddListener((string str) =>
        {
            // onSubmit handler never triggers, so this needs to be checked manually.
            if (Input.GetKeyDown(KeyCode.Return))
                SelectToggle(toggles =>
                {
                    // SearchUtil.Canonicalize strips rich-text (e.g. <link="LADDER">Ladder</link>) and uppercases,
                    // matching the form the game's own fuzzy search uses — so we compare apples to apples.
                    var query = SearchUtil.Canonicalize(str.Trim());
                    return toggles.FirstOrDefault(t => SearchUtil.Canonicalize(t.def.Name) == query)
                        ?? toggles.FirstOrDefault();
                }, (PlanBuildingToggle toggle) => toggle.Click());
        });

        // While typing, press 1-9 to select the corresponding result.
        BuildingGroupScreen.Instance.inputField.onValidateInput += (string text, int charIndex, char addedChar) =>
        {
            if (int.TryParse(addedChar.ToString(), out var num))
            {
                SelectToggle(toggles => toggles.ElementAtOrDefault(num - 1), (PlanBuildingToggle toggle) =>
                {
                    BuildingGroupScreen.Instance.inputField.DeactivateInputField();
                    toggle.Click();
                });

                return '\0';
            }

            return addedChar;
        };

        static void SelectToggle(Func<IEnumerable<PlanBuildingToggle>, PlanBuildingToggle> pick, Action<PlanBuildingToggle> selectAction)
        {
            var toggle = pick(GetVisibleTogglesInDisplayOrder());
            if (toggle != null)
            {
                // Klei's input system exists simultaneously with Unity's.
                // When typing in TMP, Klei's events are still created, but there aren't any child components to fall through to.
                // If the inputField is deselected on the KeyDown frame, the focus changes and Klei's KeyDown event does fall through.
                // In this case, it errantly triggers the BuildMenu category hotkeys.
                BuildingGroupScreen.Instance.inputField.StartCoroutine(WaitPastKeyDown(() => selectAction(toggle)));

                static IEnumerator WaitPastKeyDown(System.Action onPast)
                {
                    while (Input.anyKeyDown)
                        yield return null;
                    onPast();
                }
            }
        }

        // Walks the build menu hierarchy in display order, yielding visible (search-filtered) toggles.
        // Works for both grid view and list view: the game sorts subcategory containers and the toggles
        // inside each subcategory's grid by score via SetAsLastSibling, so transform child order
        // already matches display order. In grid view only the "default" subcategory is active and
        // contains every toggle; in list view multiple subcategories may each be active.
        static IEnumerable<PlanBuildingToggle> GetVisibleTogglesInDisplayOrder()
        {
            foreach (Transform subcategory in PlanScreen.Instance.GroupsTransform)
            {
                if (!subcategory.gameObject.activeSelf)
                    continue;
                var grid = subcategory.GetComponent<HierarchyReferences>()?.GetReference<GridLayoutGroup>("Grid");
                if (grid == null)
                    continue;
                foreach (Transform child in grid.transform)
                {
                    if (!child.gameObject.activeSelf)
                        continue;
                    var toggle = child.GetComponent<PlanBuildingToggle>();
                    if (toggle != null)
                        yield return toggle;
                }
            }
        }
    }
}
