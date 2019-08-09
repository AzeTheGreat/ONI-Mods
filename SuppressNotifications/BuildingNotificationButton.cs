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
        private StatusItemsSuppressed component;

        protected override void OnPrefabInit()
        {
            component = gameObject.GetComponent<StatusItemsSuppressed>();
            Subscribe(493375141, OnRefreshUserMenuDelegate);
        }

        private void OnRefreshUserMenu()
        {
            KIconButtonMenu.ButtonInfo button;
            List<StatusItem> suppressableStatusItems = component.GetSuppressableStatusItems();

            if (suppressableStatusItems.Any())
            {
                string iconName = "action_building_disabled";
                string text = "Suppress Current";
                System.Action on_click = new System.Action(OnSuppressClick);
                string tooltipText = "Suppress the following status items and notifications:\n" + GetStatusItemListText(suppressableStatusItems);

                button = new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText);
            }
            else
            {
                string iconName = "action_building_disabled";
                string text = "Clear Suppressed";
                System.Action on_click = new System.Action(OnClearClick);
                string tooltipText = "Stop the following status items from being suppressed:\n" + GetStatusItemListText(component.SuppressedStatusItems);

                button = new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText);
            }

            Game.Instance.userMenu.AddButton(base.gameObject, button);
        }

        private void OnSuppressClick()
        {
            component.SuppressStatusItems();
            Game.Instance.userMenu.Refresh(base.gameObject);
        }

        private void OnClearClick()
        {
            component.UnsuppressStatusItems();
            Game.Instance.userMenu.Refresh(base.gameObject);
        } 

        private string GetStatusItemListText(List<StatusItem> statusItems)
        {
            string text = "--------------------\n\n";

            foreach (var statusItem in statusItems)
            {
                text = text + statusItem.Name + "\n";
            }

            return text;
        }

        private static readonly EventSystem.IntraObjectHandler<BuildingNotificationButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingNotificationButton>(Handler);
           
        private static void Handler(BuildingNotificationButton component, object data)
        {
            component.OnRefreshUserMenu();
        }
    }
}
