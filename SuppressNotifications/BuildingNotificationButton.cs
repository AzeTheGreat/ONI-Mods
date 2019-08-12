using System.Collections.Generic;
using System.Linq;

namespace SuppressNotifications
{
    public class BuildingNotificationButton : KMonoBehaviour
    {
        private StatusItemsSuppressedComp statusItemsSuppressedComp;
        private NotificationsSuppressedComp notificationsSuppressedComp;

        protected override void OnPrefabInit()
        {
            statusItemsSuppressedComp = gameObject.GetComponent<StatusItemsSuppressedComp>();
            notificationsSuppressedComp = gameObject.GetComponent<NotificationsSuppressedComp>();

            Subscribe(493375141, OnRefreshUserMenuDelegate);
        }

        private void OnRefreshUserMenu()
        {
            List<StatusItem> suppressableStatusItems = statusItemsSuppressedComp.GetSuppressableStatusItems();
            List<Notification> suppressableNotifications = notificationsSuppressedComp.GetSuppressableNotifications();

            if (suppressableStatusItems.Any() || suppressableNotifications.Any())
            {
                string iconName = "action_building_disabled";
                string text = "Suppress Current";
                System.Action on_click = new System.Action(OnSuppressClick);
                string tooltipText = "Suppress the following status items and notifications:\n"
                    + GetStatusItemListText(suppressableStatusItems)
                    + GetNotificationListText(suppressableNotifications);

                Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
            }
            if (statusItemsSuppressedComp.suppressedStatusItemTitles.Any() || notificationsSuppressedComp.suppressedNotifications.Any())
            {
                string iconName = "action_building_disabled";
                string text = "Clear Suppressed";
                System.Action on_click = new System.Action(OnClearClick);
                string tooltipText = "Stop the following status items and notifications from being suppressed:\n"
                    + GetStatusItemListText(statusItemsSuppressedComp.GetSuppressedStatusItems())
                    + GetNotificationListText(notificationsSuppressedComp.GetSuppressedNotifications());

                Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
            }
        }

        private void OnSuppressClick()
        {
            notificationsSuppressedComp.SuppressNotifications();
            statusItemsSuppressedComp.SuppressStatusItems();
            Game.Instance.userMenu.Refresh(base.gameObject);
        }

        private void OnClearClick()
        {
            notificationsSuppressedComp.UnsupressNotifications();
            statusItemsSuppressedComp.UnsuppressStatusItems();
            Game.Instance.userMenu.Refresh(base.gameObject);
        }

        private string GetStatusItemListText(List<StatusItem> statusItems)
        {
            string text = "--------------------\n";

            foreach (var statusItem in statusItems)
            {
                text = text + "Status: " + statusItem.Name + "\n";
            }

            return text;
        }

        private string GetNotificationListText(List<Notification> notifications)
        {
            string text = "";

            foreach (var notification in notifications)
            {
                text = text + "Notification: " + notification.titleText + "\n";
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
