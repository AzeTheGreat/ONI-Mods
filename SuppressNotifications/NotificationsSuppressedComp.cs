using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class NotificationsSuppressedComp : KMonoBehaviour
    {
        public List<Notification> notifications;
        public List<Notification> SuppressedNotifications { get; private set; }

        protected override void OnPrefabInit()
        {
            notifications = new List<Notification>();
            SuppressedNotifications = new List<Notification>();
        }

        public bool ShouldNotify(Notification notification)
        {
            return !SuppressedNotifications.Any(i => i.titleText == notification.titleText);
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

        public void SuppressNotifications()
        {
            List<Notification> suppressableNotifications = GetSuppressableNotifications();
            SuppressedNotifications.AddRange(suppressableNotifications);
            RefreshNotifications(notifications);
        }

        public void UnsupressNotifications()
        {
            SuppressedNotifications.Clear();
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
    }
}
