using BoilerSystem.Controller;
using BoilerSystem.Core.Interface;
using BoilerSystem.FileLogger;
using BoilerSystem.Repository;
using BoilerSystem.Service;

namespace BoilerSystem
{
    /// <summary>
    /// Represents the main entry point for the application and handles initial setup.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Start the execution flow of the boiler controller system.
        /// </summary>
        public static void Main()
        {
            BoilerRepository boilerRepository = new BoilerRepository();
            Logger logger = new Logger();
            NotificationService notificationService = new NotificationService();
            LoggerService loggerService = new LoggerService(logger);
            IBoilerService boilerService = new BoilerService(boilerRepository, loggerService, notificationService);
            BoilerController boilerController = new BoilerController(boilerService, notificationService, loggerService);
            boilerController.Start();
        }
    }
}
