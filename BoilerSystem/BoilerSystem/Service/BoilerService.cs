using BoilerSystem.Core.Constants;
using BoilerSystem.Core.Interface;
using BoilerSystem.Repository;

namespace BoilerSystem.Service
{
    public class BoilerService
        : IBoilerService
    {
        private readonly IBoilerRepository _boilerRepository;
        private CancellationTokenSource? _boilerCancellationTokenSource;
        private readonly LoggerService _loggerService;
        private readonly NotificationService _notificationService;
        public BoilerService(IBoilerRepository boilerRepository, LoggerService loggerService, NotificationService notificationService)
        {
            this._boilerRepository = boilerRepository;
            this._loggerService = loggerService;
            this._notificationService = notificationService;
        }

        /// <inheritdoc>
        public void InitialSetUp()
        {
            this._boilerRepository.SetSwitchStatus(SwitchStatus.Open);
            this._boilerRepository.SetBoilerStatus(BoilerStatus.Lockout);
            this._notificationService.Execute("Initial setup completed !", Colors.green);
            this._loggerService.Execute("Boiler Initialized", DateTime.Now);
        }

        /// <inheritdoc>
        public async Task StartBoilerAsync()
        {
            if (this._boilerCancellationTokenSource != null)
            {
                this._notificationService.Execute("[Failed] - Boiler is already running", Colors.red);
                this._loggerService.Execute("Start boiler failed. Reason - Boiler already running", DateTime.Now);
                return;
            }

            this._notificationService.Execute("Starting the boiler ......", Colors.green);
            this._loggerService.Execute("Starting the boiler", DateTime.Now);
            if (this._boilerRepository.GetSwitchStatus() == SwitchStatus.Open)
            {
                this._notificationService.Execute("[Failed]-Close the interlock switch before starting boiler !!", Colors.red);
                this._notificationService.Execute("Press option 4 to close the to toggle the interlock switch", Colors.yellow);
                this._loggerService.Execute("Boiler starting failed. Reason - Interlock switch is open", DateTime.Now);
                return;
            }

            if (!this._boilerRepository.IsResetDone())
            {
                this._notificationService.Execute("[Failed]-Reset the boiler !!", Colors.red);
                this._notificationService.Execute("Reset must be done after initialization or after failure..", Colors.yellow);
                this._notificationService.Execute("Press option 5 to reset the boiler", Colors.yellow);
                this._loggerService.Execute("Boiler starting failed. Reason - Reset not done", DateTime.Now);
                return;
            }

            this._boilerCancellationTokenSource = new CancellationTokenSource();
            var token = _boilerCancellationTokenSource.Token;
            try
            {
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Ready);
                this._notificationService.Execute("Boiler is ready and started its operation !!", Colors.green);
                this._loggerService.Execute("Boiler service started its execution and in ready state.", DateTime.Now);
                this._boilerRepository.SetBoilerStatus(BoilerStatus.PrePurge);
                this._loggerService.Execute("Boiler went into pre purge state", DateTime.Now);
                this._notificationService.Execute("Starting pre purge process....", Colors.yellow);
                await Task.Delay(this._boilerRepository.GetPrePurgeTime(), token);
                this._notificationService.Execute("Boiler pre purge process completed !!", Colors.green);
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Ignition);
                this._loggerService.Execute("Boiler went into ignition state.", DateTime.Now);
                this._notificationService.Execute("Starting ignition state....", Colors.yellow);
                await Task.Delay(this._boilerRepository.GetIgnitionTime(), token);
                this._notificationService.Execute("Boiler ignition process completed !!", Colors.green);
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Operational);
                this._notificationService.Execute("Boiler is in operational state....", Colors.green);
                this._loggerService.Execute("Boiler is in operational state....", DateTime.Now);
            }
            catch (OperationCanceledException)
            {
                this._notificationService.Execute("Boiler operation cancelled !", Colors.red);
                this._loggerService.Execute("Boiler operation stopped by user", DateTime.Now);
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Ready);
                this._boilerCancellationTokenSource = null;
            }
            finally
            {
            }
        }

        /// <inheritdoc>
        public void StopBoiler()
        {
            if(this._boilerRepository.GetBoilerStatus() == BoilerStatus.Operational && _boilerCancellationTokenSource != null)
            {
                _boilerCancellationTokenSource.Cancel();
                this._boilerCancellationTokenSource = null;
                this._notificationService.Execute("Boiler stopped....", Colors.yellow);
                this._loggerService.Execute("Trying to stop the boiler", DateTime.Now);
            }
            else if(_boilerCancellationTokenSource != null)
            {
                this._notificationService.Execute("Stopping the boiler...", Colors.yellow);
                this._loggerService.Execute("Trying to stop the boiler", DateTime.Now);
                _boilerCancellationTokenSource.Cancel();
            }
            else
            {
                this._notificationService.Execute("[Failed] - Boiler is not running!", Colors.red);
                this._loggerService.Execute("Stopping failed. Reason - Boiler is not running", DateTime.Now);
            }
        }

        /// <inheritdoc>
        public void StimulateBoilerError()
        {
            if(this._boilerCancellationTokenSource != null)
            {
                if(this._boilerRepository.GetBoilerStatus() == BoilerStatus.Operational)
                {
                    this._notificationService.Execute("Stopping the boiler due to some error .....", Colors.red);
                    this._notificationService.Execute("Changing boiler state to lockout....", Colors.green);
                    this._loggerService.Execute("Error identified in boiler.", DateTime.Now);
                    _boilerCancellationTokenSource.Cancel();
                    this._boilerRepository.SetResetDone(false);
                    this._boilerRepository.SetBoilerStatus(BoilerStatus.Lockout);
                    this._boilerCancellationTokenSource = null;
                }
                else
                {
                    this._notificationService.Execute("Unable to stimulate error because boiler is not in operational state", Colors.yellow);
                    this._loggerService.Execute("Error stimulation failed. Reason - Boiler in not in operational state", DateTime.Now);
                }
            }
            else
            {
                this._notificationService.Execute("[Failed] - The boiler is not running", Colors.red);
                this._loggerService.Execute("Error stimulation failed. Reason - Boiler is not running", DateTime.Now);
            }
        }

        /// <inheritdoc>
        public void ToggleSwitch()
        {
            if (this._boilerCancellationTokenSource != null)
            {
                this._notificationService.Execute("[failed] - Cannot toggle switch when boiler is running !!", Colors.red);
                this._loggerService.Execute("Toggle failed. Reason - Boiler is running", DateTime.Now);
                return;
            }

            if (this._boilerRepository.GetSwitchStatus() == SwitchStatus.Open)
            {
                this._boilerRepository.SetSwitchStatus(SwitchStatus.Closed);
                this._notificationService.Execute("Interlock switch is closed", Colors.green);
                this._loggerService.Execute("Interlock switch is closed", DateTime.Now);
            }
            else
            {
                this._boilerRepository.SetSwitchStatus(SwitchStatus.Open);
                this._notificationService.Execute("Interlock switch is opened", Colors.green);
                this._loggerService.Execute("Interlock switch is closed", DateTime.Now);
            }
        }

        /// <inheritdoc>
        public void ResetBoiler()
        {
            if (this._boilerRepository.GetSwitchStatus() == SwitchStatus.Open)
            {
                this._notificationService.Execute("[Failed] - Close the interlock switch and try again !!", Colors.red);
                this._notificationService.Execute("Press 5 to toggle the switch", Colors.yellow);
                this._loggerService.Execute("Reset boiler failed. Reason - Switch is open", DateTime.Now);
            }
            else
            {
                this._boilerRepository.SetResetDone(true);
                this._notificationService.Execute("Boiler reset successfully", Colors.green);
                this._loggerService.Execute("Boiler reset", DateTime.Now);
                this._boilerRepository.SetBoilerStatus(BoilerStatus.Ready);
            }
        }
    }
}
