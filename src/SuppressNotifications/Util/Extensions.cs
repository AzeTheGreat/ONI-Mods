namespace SuppressNotifications
{
    public static class Extensions
    {
        public static NotificationsSuppressedComp GetSuppressedComp(this Notification notification)
        {
            if(notification == null || notification.Notifier == null)
                return null;
            return notification.Notifier.gameObject.GetComponent<NotificationsSuppressedComp>();
        }
    }
}
