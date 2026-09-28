namespace BoilerSystem.Service
{
    public class NotificationService
    {
        public delegate void Notification(string message, string color);
        public event Notification? Notifier;
        public static int notificationCounter = 0;
        public void Execute(string message, string color)
        {
            Notifier?.Invoke(message, color);
        }
    }
}
