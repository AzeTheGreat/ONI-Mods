using HarmonyLib;

namespace SuppressNotifications
{
    // The Notification.Notifier prop is set in Notifier.Add, so this must be a postfix.
    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Add))]
    class TrackNotificationAdd
    {
        static void Postfix(Notification notification) => notification.GetSuppressedComp()?.notifications.Add(notification);
    }

    // Notifier.Remove sets the Notification.Notifier prop to null, so this must be a prefix.
    [HarmonyPatch(typeof(Notifier), nameof(Notifier.Remove))]
    class TrackNotificationRemove
    {
        static void Prefix(Notification notification) => notification.GetSuppressedComp()?.notifications.Remove(notification);
    }
}
