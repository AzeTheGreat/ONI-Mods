using Harmony;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(NotificationScreen), "AddNotification")]
    class Patch_NotificationScreen_AddNotification
    {
        static bool Prefix(Notification notification)
        {
            var component = notification.Notifier.gameObject.GetComponent<NotificationsSuppressedComp>();
            component?.notifications.Add(notification);
            return component?.ShouldNotify(notification) ?? true;
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
