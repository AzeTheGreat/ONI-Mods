using HarmonyLib;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace BuildMenuSearchHotkey;

[HarmonyPatch(typeof(BuildingGroupScreen), nameof(BuildingGroupScreen.OnSpawn))]
class SearchClickHotkeys
{
    static void Postfix()
    {
        // While typing, press Enter to select the first result.
        BuildingGroupScreen.Instance.inputField.onEndEdit.AddListener((string str) =>
        {
            // onSubmit handler never triggers, so this needs to be checked manually.
            if (Input.GetKeyDown(KeyCode.Return))
                SelectToggle(0, (PlanBuildingToggle toggle) => toggle.Click());
        });

        // While typing, press 1-9 to select the corresponding result.
        BuildingGroupScreen.Instance.inputField.onValidateInput += (string text, int charIndex, char addedChar) =>
        {
            if (int.TryParse(addedChar.ToString(), out var num))
            {
                SelectToggle(num - 1, (PlanBuildingToggle toggle) =>
                {
                    BuildingGroupScreen.Instance.inputField.DeactivateInputField();
                    toggle.Click();
                });

                return '\0';
            }

            return addedChar;
        };

        static void SelectToggle(int toggleIndex, Action<PlanBuildingToggle> selectAction)
        {
            // The displayed order of buttons is set by changing the sibling index.
            // Thus, these must be accessed through the transform to get the correct order.
            var parent = PlanScreen.Instance.allBuildingToggles.Values.FirstOrDefault()?.transform.parent;
            var children = parent.Cast<Transform>().Where(x => x.gameObject.activeSelf);
            var toggle = children.ElementAtOrDefault(toggleIndex)?.GetComponent<PlanBuildingToggle>();
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
    }
}
