using System.Threading;

namespace BoilerSystem.FileLogger
{
    public class Logger
    {
        private  readonly string _filePath = "logger.csv";
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private CancellationToken token = default;
        private Task<string> content;

        public async Task LogAsync(string message)
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
