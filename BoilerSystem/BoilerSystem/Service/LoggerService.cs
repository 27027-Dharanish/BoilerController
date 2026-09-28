using BoilerSystem.FileLogger;

namespace BoilerSystem.Service
{
    /// <summary>
    /// Perform log read and write operation.
    /// </summary>
    public class LoggerService
    {
        private Logger logger;
        public delegate void LogFile(string message, DateTime time);
        public event LogFile? Log;
        public static int notificationCounter = 0;
        public LoggerService(Logger logger)
        {
            this.logger = logger;
            this.Log += LogMessage;
        }

        /// <summary>
        /// Execute the log event.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="time">Date and time of the log.</param>
        public void Execute(string message, DateTime time)
        {
            Log?.Invoke(message, time);
        }

        /// <summary>
        /// Get all the log.
        /// </summary>
        /// <returns>String of log present.</returns>
        public async Task<string[]> GetAllLog()
        {
            string lines =  await logger.GetLogAsync();
            return lines.Split("\n");
        }

        /// <summary>
        /// Event the subscribe the Log.
        /// </summary>
        /// <param name="message">Log message.</param>
        /// <param name="time">Time of log raised.</param>
        public void LogMessage(string message, DateTime time)
        {
            _ = this.logger.LogAsync(message, time);
        }
    }
}
