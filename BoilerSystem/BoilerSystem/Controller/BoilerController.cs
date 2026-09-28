using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoilerSystem.Core.Constants;
using BoilerSystem.Core.Interface;
using BoilerSystem.Service;
using BoilerSystem.View;

namespace BoilerSystem.Controller
{
    /// Todo : Summary
    public class BoilerController
    {
        private readonly IBoilerService _boilerService;
        private readonly NotificationService _notificationService;
        private readonly LoggerService _loggerService;
        public BoilerController(IBoilerService boilerService, NotificationService notificationService, LoggerService loggerService)
        {
            _boilerService = boilerService;
            _notificationService = notificationService;
            _loggerService = loggerService;
            this._notificationService.Notifier += ConsoleActivity.DisplayNotification;
        }
        /// <summary>
        /// Start the execution flow of the boiler controller.
        /// </summary>
        public void Start()
        {
            this._boilerService.InitialSetUp();
            MenuItem userChoice;
            do
            {
                ConsoleActivity.ShowMenu("Boiler Controller System", new[] { "Start Boiler Sequence", "Stop Boiler Sequence", "Stimulate Boiler Error", "Toggle Run Interlock Switch", "Reset Boiler", "View Event Log", "Exit" });
                userChoice = (MenuItem)ConsoleActivity.GetIntegerInput("option to perform");
                switch(userChoice)
                {
                    case MenuItem.StartBoiler:
                        this.StartBoilerSequence();
                        break;
                    case MenuItem.StopBoiler:
                        this.StopBoilerSequence();
                        break;
                    case MenuItem.StimulateError:
                        this.StimulateBoilerError();
                        break;
                    case MenuItem.ToggleSwitch:
                        this.ToggleInterlockSwitch();
                        break;
                    case MenuItem.Reset:
                        this.ResetBoiler();
                        break;
                    case MenuItem.ViewLog:
                        this.ViewLog();
                        break;
                    case MenuItem.Exit:
                        break;
                    default:
                        ConsoleActivity.PrintAndWait("Select a valid operation [1-7]");
                        break;
                }
            }
            while(userChoice != MenuItem.Exit);
        }
        private async void StartBoilerSequence()
        {
            await this._boilerService.StartBoilerAsync();
        }
        private void StopBoilerSequence()
        {
            this._boilerService.StopBoiler();
        }
        private void StimulateBoilerError()
        {
            this._boilerService.StimulateBoilerError();
        }
        private void ToggleInterlockSwitch()
        {
            this._boilerService.ToggleSwitch();
        }
        private void ResetBoiler()
        {
            this._boilerService.ResetBoiler();
        }
        private void ViewLog()
        {
            string[] lines = this._loggerService.GetAllLog().Result;
            foreach (string line in lines)
            { 
                Console.WriteLine(line);
            }
            ConsoleActivity.WaitInConsole();
        }
    }
}
