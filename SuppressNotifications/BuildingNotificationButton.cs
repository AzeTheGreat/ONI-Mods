using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace SuppressNotifications
{
    public class BuildingNotificationButton: KMonoBehaviour
    {
        protected override void OnPrefabInit()
        {
            base.Subscribe<BuildingNotificationButton>(493375141, BuildingNotificationButton.OnRefreshUserMenuDelegate);
        }

        private void OnRefreshUserMenu()
        {
            KIconButtonMenu.ButtonInfo button;
            List<StatusItem> suppressableStatusItems = GetSuppressableStatusItems(base.gameObject);

            if (suppressableStatusItems.Any())
            {
                string iconName = "action_building_disabled";
                string text = "Suppress Current";
                System.Action on_click = new System.Action(OnSuppressClick);
                button = new KIconButtonMenu.ButtonInfo(iconName, text, on_click);
            }
            else
            {
                string iconName = "action_building_disabled";
                string text = "Clear Suppressed";
                System.Action on_click = new System.Action(OnClearClick);
                button = new KIconButtonMenu.ButtonInfo(iconName, text, on_click);
            }

            Game.Instance.userMenu.AddButton(base.gameObject, button);
        }

        private void OnSuppressClick()
        {
            
        }

        private void OnClearClick()
        {

        }

        private List<StatusItem> GetSuppressableStatusItems(GameObject go)
        {
            List<StatusItem> suppressableStatusItems = new List<StatusItem>();

            var statEnumerator = go.GetComponent<KSelectable>()?.GetStatusItemGroup()?.GetEnumerator();
            if (statEnumerator == null)
                return suppressableStatusItems;

            using (statEnumerator)
            {
                while (statEnumerator.MoveNext())
                {
                    StatusItem statusItem = statEnumerator.Current.item;

                    if (Util.ShouldShowIcon(statusItem, go))
                    {
                        suppressableStatusItems.Add(statusItem);
                    }
                }
            }

            return suppressableStatusItems;
        }

        private static readonly EventSystem.IntraObjectHandler<BuildingNotificationButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingNotificationButton>(Handler);
           
        private static void Handler(BuildingNotificationButton component, object data)
        {
            component.OnRefreshUserMenu();
        }
    }
}
