using HarmonyLib;

namespace SuppressNotifications
{
    // The Notification.Notifier prop is set in Notifier.Add, so this must be a postfix.
    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Add))]
    class Patch_NotificationScreen_AddNotification
    {
        static void Postfix(Notification notification)
        {
            var component = notification?.Notifier?.gameObject.GetComponent<NotificationsSuppressedComp>();
            component?.notifications.Add(notification);
        }
    }

    // Notifier.Remove sets the Notification.Notifier prop to null, so this must be a prefix.
    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Remove))]
    class Patch_NotificationScreen_RemoveNotification
    {
        static void Prefix(Notification notification)
        {     
            notification?.Notifier?.gameObject.GetComponent<NotificationsSuppressedComp>()?.notifications.Remove(notification);
        }
    }
}
