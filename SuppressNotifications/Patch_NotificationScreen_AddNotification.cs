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
            
            //Debug.Log(notification.titleText);
            var comp = notification.Notifier.gameObject.GetComponent<NotificationsSuppressedComp>();
            if (comp == null) { return; }
            Debug.Log("ADD TO " + notification?.Notifier?.name + ": " + notification.titleText);

            notification.Notifier.gameObject.GetComponent<NotificationsSuppressedComp>()?.notifications.Add(notification);

            foreach (var note in comp.notifications)
            {
                Debug.Log(note.titleText);
            }
        }
    }

    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Remove))]
    class Patch_NotificationScreen_RemoveNotification
    {
        static void Prefix(Notification notification)
        {
            if (notification.Notifier == null)
            {
                //Debug.Log("NULL");
                //Debug.Log(notification);
                //Debug.Log(notification.titleText);
                return;
            }
                
            //Debug.Log("Remove");
            //Debug.Log(notification.Notifier);
            //Debug.Log(notification.Notifier.gameObject);
            notification.Notifier.gameObject.GetComponent<NotificationsSuppressedComp>()?.notifications.Remove(notification);
        }
    }

    public static class Util
    {

    }
}
