using System.Threading;

namespace BoilerSystem.FileLogger
{
    public class Logger
    {
        private  readonly string _filePath = "logger.csv";
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private Task<string> content;

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
