using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuppressNotifications
{
    class NotificationsSuppressedComp : KMonoBehaviour, ISaveLoadable
    {
        [Serialize]
        public List<string> suppressedNotifications;

        public List<Notification> notifications;

        protected override void OnPrefabInit()
        {
            notifications = new List<Notification>();
            suppressedNotifications = new List<string>();
            Subscribe(-905833192, OnCopySettingsDelegate);
        }

        public bool ShouldNotify(Notification notification)
        {
            return !suppressedNotifications.Any(i => i == notification.titleText);
        }

        public List<Notification> GetSuppressableNotifications()
        {
            var suppressableNotifications = new List<Notification>();

            foreach (var notification in notifications)
            {
                if (ShouldNotify(notification))
                    suppressableNotifications.Add(notification);
            }

            return suppressableNotifications;
        }

        public List<Notification> GetSuppressedNotifications()
        {
            var suppressedNotifications = new List<Notification>();

            foreach (var notification in notifications)
            {
                if (!ShouldNotify(notification))
                    suppressedNotifications.Add(notification);
            }

            return suppressedNotifications;
        }

        public void SuppressNotifications()
        {
            List<Notification> suppressableNotifications = GetSuppressableNotifications();

            foreach (var note in suppressableNotifications)
            {
                suppressedNotifications.Add(note.titleText);
            }

            RefreshNotifications(notifications);
        }

        public void UnsupressNotifications()
        {
            suppressedNotifications.Clear();
            RefreshNotifications(notifications);
        }

        private void RefreshNotifications(List<Notification> notificationsToRefresh)
        {
            foreach (var notification in notificationsToRefresh.ToList())
            {
                Notifier notifier = notification.Notifier;
                notification.Clear();
                notifier.Add(notification);
            }
        }

        private void OnCopySettings(object data)
        {
            GameObject gameObject = (GameObject)data;
            NotificationsSuppressedComp comp = gameObject.GetComponent<NotificationsSuppressedComp>();
            if (comp != null)
            {
                suppressedNotifications = comp.suppressedNotifications;
            }
        }

        private static readonly EventSystem.IntraObjectHandler<NotificationsSuppressedComp> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<NotificationsSuppressedComp>(Handler);

        private static void Handler(NotificationsSuppressedComp comp, object data)
        {
            comp.OnCopySettings(data);
        }
    }
}
