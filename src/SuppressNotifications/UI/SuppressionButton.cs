using System;
using System.Collections.Generic;
using System.Linq;
using static STRINGS.MISC;

namespace SuppressNotifications
{
    public class SuppressionButton : KMonoBehaviour
    {
        [MyCmpAdd]
        internal StatusItemsSuppressedComp statusItemsSuppressedComp;
        [MyCmpAdd]
        internal NotificationsSuppressedComp notificationsSuppressedComp;

        public override void OnPrefabInit()
        {
            Subscribe((int)GameHashes.RefreshUserMenu, (object data) => OnRefreshUserMenu());
        }

        private void OnRefreshUserMenu()
        {
            // For vanilla and SO, must explicitly set the right action since this enum is different.
            Enum.TryParse(nameof(Action.NumActions), out Action action);

            if (AreSuppressable())
                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", MYSTRINGS.SUPPRESSBUTTON.NAME, new System.Action(OnSuppressClick), action, tooltipText: GetSuppressableString()));
            else if (AreSuppressed())
                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", MYSTRINGS.CLEARBUTTON.NAME, new System.Action(OnClearClick), action, tooltipText: GetSuppressedString()));
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
            return MYSTRINGS.SUPPRESSBUTTON.TOOLTIP + "\n"
                    + GetStatusItemListText(statusItemsSuppressedComp.GetSuppressableStatusItems())
                    + GetNotificationListText(notificationsSuppressedComp.GetSuppressableNotifications());
        }

        internal virtual string GetSuppressedString()
        {
            return MYSTRINGS.CLEARBUTTON.TOOLTIP + "\n"
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

        private string GetStatusItemListText(List<StatusItem> statusItems) => GetStatusItemListText(statusItems.Select(x => x.Name).ToList());
        private string GetStatusItemListText(List<string> statusItems) => GetItemListText(MYSTRINGS.STATUS_LABEL, statusItems);

        private string GetNotificationListText(List<Notification> notifications) => GetNotificationListText(notifications.Select(x => x.titleText).ToList());
        private string GetNotificationListText(List<string> notifications) => GetItemListText(MYSTRINGS.NOTIFICATION_LABEL, notifications);

        private string GetItemListText(string label, List<string> items)
        {
            if (!items.Any())
                return string.Empty;

            string text = Environment.NewLine + label + Environment.NewLine;

            foreach (var item in items)
            {
                text = text + item + "\n";
            }

            return text;
        }
    }
}
