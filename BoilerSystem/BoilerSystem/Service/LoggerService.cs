using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoilerSystem.FileLogger;

namespace BoilerSystem.Service
{
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
        public void Execute(string message, DateTime time)
        {
            Log?.Invoke(message, time);
        }
        public async Task<string[]> GetAllLog()
        {
            string lines =  await logger.GetLogAsync();
            return lines.Split("\n");
        }
        public void LogMessage(string message, DateTime time)
        {
            _ = this.logger.LogAsync(message, time);
        }
    }
}
