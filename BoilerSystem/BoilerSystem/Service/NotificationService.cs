using BoilerSystem.Core.Constants;

namespace BoilerSystem.Service
{
    /// <summary>
    /// Perform notification related events.
    /// </summary>
    public class NotificationService
    {
        public delegate void Notification(string message, Colors color);
        public event Notification? Notifier;
        public static int notificationCounter = 0;

        /// <summary>
        /// Invoked the event.
        /// </summary>
        /// <param name="message">Notification message.</param>
        /// <param name="color">Color of the notification.</param>
        public void Execute(string message, Colors color)
        {
            Notifier?.Invoke(message, color);
        }
    }
}
