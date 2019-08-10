using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class NotificationsSuppressedComp : KMonoBehaviour
    {
        public List<Notification> suppressableNotifications;
        public List<Notification> SuppressedNotifications { get; private set; }

        protected override void OnPrefabInit()
        {
            suppressableNotifications = new List<Notification>();
            SuppressedNotifications = new List<Notification>();
        }

        public bool ShouldNotify(Notification notification)
        {
            return !SuppressedNotifications.Any(i => i.titleText == notification.titleText);
        }

        public void SuppressNotifications()
        {
            SuppressedNotifications.AddRange(suppressableNotifications);
            suppressableNotifications.Clear();
        }

        public void UnsupressNotifications()
        {
            suppressableNotifications.AddRange(SuppressedNotifications);
            SuppressedNotifications.Clear();
        }
    }
}
