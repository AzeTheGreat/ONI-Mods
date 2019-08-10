using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class NotificationsSuppressedComp : KMonoBehaviour
    {
        public List<Notification> notifications;

        private List<Notification> suppressedNotifications;

        protected override void OnPrefabInit()
        {
            notifications = new List<Notification>();
            suppressedNotifications = new List<Notification>();
        }

        public bool ShouldNotify(Notification notification)
        {
            return !suppressedNotifications.Contains(notification);
        }

        public List<Notification> GetSuppressableNotifications()
        {
            var suppressableNotifications = new List<Notification>();

            if (notifications == null)
                return suppressableNotifications;

            foreach (var notification in notifications)
            {
                suppressableNotifications.Add(notification);
            }
            return suppressableNotifications;
        }

        public void SuppressNotifications()
        {

        }

        public void UnsupressNotifications()
        {

        }
    }
}
