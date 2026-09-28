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
        public delegate void Log(string message, DateTime time);
        public event Log? Logger;
        public static int notificationCounter = 0;
        public LoggerService(Logger logger)
        {
            this.logger = logger;
        }
        public void Execute(string message, DateTime time)
        {
            Logger?.Invoke(message, time);
        }
        public async Task<string[]> GetAllLog()
        {
            string lines =  await logger.GetLogAsync();
            return lines.Split(",");
        }
    }
}
