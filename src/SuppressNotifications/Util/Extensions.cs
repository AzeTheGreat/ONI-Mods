namespace SuppressNotifications
{
    public static class Extensions
    {
        public static NotificationsSuppressedComp GetSuppressedComp(this Notification notification)
            => notification?.Notifier?.gameObject.GetComponent<NotificationsSuppressedComp>();
    }
}
