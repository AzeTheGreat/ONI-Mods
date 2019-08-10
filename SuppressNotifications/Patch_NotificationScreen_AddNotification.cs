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
        static bool Prefix(Notification notification)
        {
            var component = notification.Notifier.gameObject.GetComponent<NotificationsSuppressedComp>();
            bool shouldNotify = component?.ShouldNotify(notification) ?? true;

            if (shouldNotify)
                component?.suppressableNotifications.Add(notification);

            return shouldNotify;
        }
    }

    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Remove))]
    class Patch_NotificationScreen_RemoveNotification
    {
        static void Prefix(Notification notification)
        {                
            notification.Notifier?.gameObject.GetComponent<NotificationsSuppressedComp>()?.suppressableNotifications.Remove(notification);
        }
    }
}
