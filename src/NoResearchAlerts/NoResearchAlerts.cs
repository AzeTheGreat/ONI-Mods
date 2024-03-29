﻿using HarmonyLib;
using UnityEngine;

namespace NoResearchAlerts
{
    [HarmonyPatch(typeof(PlanCategoryNotifications), nameof(PlanCategoryNotifications.ToggleAttention))]
    public class NoAlerts
    {
        static bool Prepare() => Options.Opts.AlertMode == Options.Mode.None;

        static void Prefix(ref bool active) => active = false;
    }

    [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.CloseCategoryPanel))]
    public class FastClearAlerts
    {
        static bool Prepare() => Options.Opts.AlertMode == Options.Mode.FastClear;

        static void Prefix()
        {
            var toggleInfo = PlanScreen.Instance.ActiveCategoryToggleInfo;
            var bToggles = PlanScreen.Instance.activeCategoryBuildingToggles;
            if (toggleInfo == null || bToggles?.Count == 0)
                return;

            var category = (HashedString)toggleInfo.userData;
            PlanScreen.Instance.GetToggleEntryForCategory(category, out var toggleEntry);

            ClearCategoryAlert();
            ClearBuildingAlerts();

            void ClearCategoryAlert() => ClearAlert(toggleEntry.toggleInfo.toggle);
            void ClearBuildingAlerts()
            {
                toggleEntry.pendingResearchAttentions.Clear();
                foreach (var (_, tog) in bToggles)
                    ClearAlert(tog);
            }

            void ClearAlert(Component cmp) => cmp.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
        }
    }
}
