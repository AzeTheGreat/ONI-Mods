using System;
using System.Collections.Generic;
using System.Linq;

namespace SuppressNotifications
{
    public class SuppressionButton : KMonoBehaviour
    {
        [MyCmpAdd] private StatusItemsSuppressedComp statusItemsSuppressedComp;
        [MyCmpAdd] private NotificationsSuppressedComp notificationsSuppressedComp;

        public override void OnPrefabInit()
        {
            Subscribe((int)GameHashes.RefreshUserMenu, (_) => OnRefreshUserMenu());
        }

        private void OnRefreshUserMenu()
        {
            if (AreSuppressable())
                AddButton(MYSTRINGS.SUPPRESSBUTTON.NAME, OnSuppressClick, GetSuppressableString);
            else if (AreSuppressed())
                AddButton(MYSTRINGS.CLEARBUTTON.NAME, OnClearClick, GetSuppressedString);

            void AddButton(string text, System.Action onClick, Func<string> getTooltip)
            {
                // For vanilla and SO, must explicitly set the right action since this enum is different.
                Enum.TryParse(nameof(Action.NumActions), out Action action);

                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", text, onClick, action, tooltipText: getTooltip()));
            }
        }

        internal virtual bool AreSuppressable() => 
            statusItemsSuppressedComp.GetSuppressableStatusItems().Any() || notificationsSuppressedComp.GetSuppressableNotifications().Any();
        internal virtual bool AreSuppressed() => 
            statusItemsSuppressedComp.suppressedStatusItemTitles.Any() || notificationsSuppressedComp.suppressedNotifications.Any();

        internal virtual void OnSuppressClick()
        {
            notificationsSuppressedComp.SuppressNotifications();
            statusItemsSuppressedComp.SuppressStatusItems();
            Game.Instance.userMenu.Refresh(gameObject);
        }

        internal virtual void OnClearClick()
        {
            notificationsSuppressedComp.UnsupressNotifications();
            statusItemsSuppressedComp.UnsuppressStatusItems();
            Game.Instance.userMenu.Refresh(gameObject);
        }

        internal virtual string GetSuppressableString()
        {
            return MYSTRINGS.SUPPRESSBUTTON.TOOLTIP + Environment.NewLine
                    + GetStatusItemListText(statusItemsSuppressedComp.GetSuppressableStatusItems())
                    + GetNotificationListText(notificationsSuppressedComp.GetSuppressableNotifications());
        }

        internal virtual string GetSuppressedString()
        {
            return MYSTRINGS.CLEARBUTTON.TOOLTIP + Environment.NewLine
                    + GetStatusItemListText(statusItemsSuppressedComp.suppressedStatusItemTitles)
                    + GetNotificationListText(notificationsSuppressedComp.suppressedNotifications);
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
                text += item + Environment.NewLine;

            return text;
        }
    }
}
