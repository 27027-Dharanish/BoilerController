using System.Threading;

namespace BoilerSystem.Logger
{
    public static class Logger
    {
        private static readonly string _filePath = "logger.csv";
        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private static CancellationToken token = default;
        private static Task<string> content;

        public static async Task LogAsync(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

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
        public static async Task<string> GetLogAsync()
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
