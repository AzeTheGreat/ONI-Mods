using HarmonyLib;

namespace SuppressNotifications
{
    // Preventing notifications from showing requires a Prefix, so this cannot be combined with the Notifier.Add patch.
    [HarmonyPatch(typeof(NotificationScreen), nameof(NotificationScreen.AddNotification))]
    class NotificationInterceptor
    {
        static bool Prefix(Notification notification) => notification?.Notifier?.gameObject.GetComponent<NotificationsSuppressedComp>()?.ShouldNotify(notification) ?? true;
    }
}
