using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(NotificationScreen), "AddNotification")]
    class Patch_NotificationScreen_AddNotification
    {
        static void Prefix(Notification notification)
        {
            notification.Notifier.gameObject.GetComponent<NotificationsSuppressedComp>()?.notifications.Add(notification);
        }
    }

    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Remove))]
    class Patch_NotificationScreen_RemoveNotification
    {
        static void Prefix(Notification notification)
        {                
            notification.Notifier?.gameObject.GetComponent<NotificationsSuppressedComp>()?.notifications.Remove(notification);
        }
    }
}
