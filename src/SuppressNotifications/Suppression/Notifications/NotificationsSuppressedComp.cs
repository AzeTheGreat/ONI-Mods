using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuppressNotifications
{
    public class NotificationsSuppressedComp : KMonoBehaviour, ISaveLoadable
    {
        [Serialize]
        public List<string> suppressedNotifications;

        public List<Notification> notifications;

        public override void OnPrefabInit()
        {
            Init();
            Subscribe((int)GameHashes.CopySettings, (object data) => OnCopySettings(data));
        }

        internal void Init()
        {
            if (notifications == null)
                notifications = new List<Notification>();
            if (suppressedNotifications == null)
                suppressedNotifications = new List<string>();
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

            RefreshNotifications();
        }

        public void UnsupressNotifications()
        {
            suppressedNotifications.Clear();
            RefreshNotifications();
        }

        private void RefreshNotifications()
        {
            foreach (var notification in notifications.ToList())
            {
                Notifier notifier = notification.Notifier;
                notification.Clear();
                notifier.Add(notification);
            }
        }

        internal void OnCopySettings(object data)
        {
            NotificationsSuppressedComp comp = (data as GameObject).GetComponent<NotificationsSuppressedComp>();
            if (comp != null)
            {
                suppressedNotifications = new List<string>(comp.suppressedNotifications);
                RefreshNotifications();
            }
        }
    }
}
