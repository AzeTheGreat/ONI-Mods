using System.Collections.Generic;
using System.Linq;

namespace SuppressNotifications
{
    public class SuppressionButton : KMonoBehaviour
    {
        [MyCmpAdd]
        internal StatusItemsSuppressedComp statusItemsSuppressedComp;
        [MyCmpAdd]
        internal NotificationsSuppressedComp notificationsSuppressedComp;

        protected override void OnPrefabInit()
        {
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);
        }

        private void OnRefreshUserMenu()
        {
            if (AreSuppressable())
                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", "Suppress Current", new System.Action(OnSuppressClick), tooltipText: GetSuppressableString()));
            else if (AreSuppressed())
                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", "Clear Suppressed", new System.Action(OnClearClick), tooltipText: GetSuppressedString()));
        }

        internal virtual bool AreSuppressable()
        {
            return statusItemsSuppressedComp.GetSuppressableStatusItems().Any() || notificationsSuppressedComp.GetSuppressableNotifications().Any();
        }

        internal virtual bool AreSuppressed()
        {
            return statusItemsSuppressedComp.suppressedStatusItemTitles.Any() || notificationsSuppressedComp.suppressedNotifications.Any();
        }

        internal virtual string GetSuppressableString()
        {
            return "Suppress the following status items and notifications:\n"
                    + GetStatusItemListText(statusItemsSuppressedComp.GetSuppressableStatusItems())
                    + GetNotificationListText(notificationsSuppressedComp.GetSuppressableNotifications());
        }

        internal virtual string GetSuppressedString()
        {
            return "Stop the following status items and notifications from being suppressed:\n"
                    + GetStatusItemListText(statusItemsSuppressedComp.suppressedStatusItemTitles)
                    + GetNotificationListText(notificationsSuppressedComp.suppressedNotifications);
        }

        internal virtual void OnSuppressClick()
        {
            notificationsSuppressedComp.SuppressNotifications();
            statusItemsSuppressedComp.SuppressStatusItems();
            Game.Instance.userMenu.Refresh(base.gameObject);
        }

        internal virtual void OnClearClick()
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

        private string GetStatusItemListText(List<string> statusItems)
        {
            string text = "";

            foreach (var statusItem in statusItems)
            {
                text = text + "Status: " + statusItem + "\n";
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

        private string GetNotificationListText(List<string> notifications)
        {
            string text = "";

            foreach (var notification in notifications)
            {
                text = text + "Notification: " + notification + "\n";
            }

            return text;
        }

        private static readonly EventSystem.IntraObjectHandler<SuppressionButton> OnRefreshUserMenuDelegate = 
            new EventSystem.IntraObjectHandler<SuppressionButton>(Handler);

        private static void Handler(SuppressionButton component, object data)
        {
            component.OnRefreshUserMenu();
        }
    }
}
