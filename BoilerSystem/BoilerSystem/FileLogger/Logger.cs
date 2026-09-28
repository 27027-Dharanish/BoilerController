using System.Threading;

namespace BoilerSystem.FileLogger
{
    /// <summary>
    /// Perform log read and write operation through file.
    /// </summary>
    public class Logger
    {
        private  readonly string _filePath = "logger.csv";
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private Task<string>? content;

        /// <summary>
        /// Write the log in the file.
        /// </summary>
        /// <param name="message">Log message.</param>
        /// <param name="time">Time of log.</param>
        /// <returns></returns>
        public async Task LogAsync(string message, DateTime time)
        {
            string logMessage = $"{time:yyyy-MM-dd HH:mm:ss} - {message}";
            await _lock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(_filePath, logMessage + Environment.NewLine);
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// Get all the log present in the file.
        /// </summary>
        /// <returns>String of logs.</returns>
        public async Task<string> GetLogAsync()
        {
            await _lock.WaitAsync();
            try
            {
                content = File.ReadAllTextAsync(_filePath);
                return content.Result;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
