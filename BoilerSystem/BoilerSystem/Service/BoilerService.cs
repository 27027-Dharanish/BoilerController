using System.Security.Cryptography.X509Certificates;
using BoilerSystem.Core.Constants;
using BoilerSystem.Repository;

namespace BoilerSystem.Service
{
    public class BoilerService
    {
        private readonly BoilerRepository _boilerRepository;
        private CancellationTokenSource? _boilerCancellationTokenSource;
        private readonly LoggerService _loggerService;
        private readonly NotificationService _notificationService;
        public BoilerService(BoilerRepository boilerRepository, LoggerService loggerService, NotificationService notificationService)
        {
            this._boilerRepository = boilerRepository;
            this._loggerService = loggerService;
            this._notificationService = notificationService;
        }
        public void InitialSetUp()
        {
            this._boilerRepository.SetSwitchStatus(SwitchStatus.Open);
            this._boilerRepository.SetBoilerStatus(BoilerStatus.Lockout);
            this._loggerService.Execute("Boiler Initialized", DateTime.Now);
        }
        public async Task StartBoilerAsync()
        {
            if(!this._boilerRepository.IsResetDone())
            {
                this._notificationService.Execute("Reset the boiler before starting !!");
                return;
            }

            if(this._boilerRepository.GetSwitchStatus() == SwitchStatus.Open)
            {
                this._notificationService.Execute("Close the interlock switch before starting the boiler !!");
                return;
            }
            this._boilerCancellationTokenSource = new CancellationTokenSource();
            var token = _boilerCancellationTokenSource.Token;
            try
            {
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Ready);
                this._notificationService.Execute("Boiler is ready!!");
                this._boilerRepository.SetBoilerStatus(BoilerStatus.PrePurge);
                await Task.Delay(this._boilerRepository.GetPrePurgeTime(), token);
                this._notificationService.Execute("Boiler pre purge time completed !!");
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Ignition);
                await Task.Delay(this._boilerRepository.GetIgnitionTime(), token);
                this._notificationService.Execute("Boiler ignition time completed !!");
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Operational);
            }
            catch (OperationCanceledException)
            {
                this._notificationService.Execute("Boiler operation cancelled !");
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Ready);
            }
            finally
            {
                this._boilerCancellationTokenSource.Dispose();
            }
        }
        public void StopBoiler()
        {
            if(_boilerCancellationTokenSource != null)
            {
                this._notificationService.Execute("Stopping the boiler...");
                _boilerCancellationTokenSource.Cancel();
            }
            else
            {
                this._notificationService.Execute("The boiler is not running or already stopped");
            }
        }
        public void StimulateBoilerError()
        {
            if(this._boilerCancellationTokenSource != null)
            {
                if(this._boilerRepository.GetBoilerStatus() == BoilerStatus.Operational)
                {
                    this._notificationService.Execute("Stopping the boiler due to some error .....");
                    this._notificationService.Execute("Changing boiler state to lockout....");
                    _boilerCancellationTokenSource.Cancel();
                    this._boilerRepository.SetBoilerStatus(BoilerStatus.Lockout);
                }
                else
                {
                    this._notificationService.Execute("Unable to stimulate error because boiler is not in operational state");
                }
            }
            else
            {
                this._notificationService.Execute("The boiler is not running");
            }
        }
        public void ToggleSwitch()
        {
            if (this._boilerCancellationTokenSource != null)
            {
                this._notificationService.Execute("Cannot toggle switch when boiler is running !!");
                return;
            }

            if (this._boilerRepository.GetSwitchStatus() == SwitchStatus.Open)
            {
                this._boilerRepository.SetSwitchStatus(SwitchStatus.Closed);
            }
            else
            {
                this._boilerRepository.SetSwitchStatus(SwitchStatus.Open);
            }
        }
        public void ResetBoiler()
        {
            if (this._boilerRepository.GetSwitchStatus() == SwitchStatus.Open)
            {
                this._notificationService.Execute("Close the interlock switch!!");
            }
            else
            {
                this._notificationService.Execute("Boiler reset successfully");
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Ready);
            }
        }
    }
}
