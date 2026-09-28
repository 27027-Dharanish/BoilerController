using BoilerSystem.Core.Constants;

namespace BoilerSystem.Core.Interface
{
    /// <summary>
    /// Interface for the boiler repository.
    /// </summary>
    public interface IBoilerRepository
    {
        /// <summary>
        /// Modify the boiler status.
        /// </summary>
        /// <param name="status">Status of the boiler to be changed.</param>
        public void SetBoilerStatus(BoilerStatus status);

        /// <summary>
        /// Modify the switch status.
        /// </summary>
        /// <param name="status">Switch status to be updated.</param>
        public void SetSwitchStatus(SwitchStatus status);

        /// <summary>
        /// Get the boiler status of the boiler.
        /// </summary>
        /// <returns>Boiler status.</returns>
        public BoilerStatus GetBoilerStatus();

        /// <summary>
        /// Get the interlock switch status.
        /// </summary>
        /// <returns>Status of the interlock switch.</returns>
        public SwitchStatus GetSwitchStatus();

        /// <summary>
        /// Set the reset flag of the boiler.
        /// </summary>
        /// <param name="flag">Flag to be updated.</param>
        public void SetResetDone(bool flag);

        /// <summary>
        /// Check whether the reset of boiler is done.
        /// </summary>
        /// <returns>True if reset is done; Otherwise false.</returns>
        public bool IsResetDone();

        /// <summary>
        /// Get the ignition time of the boiler.
        /// </summary>
        /// <returns>Ignition time of the boiler.</returns>
        public TimeSpan GetIgnitionTime();

        /// <summary>
        /// Get the pre purge time of the boiler.
        /// </summary>
        /// <returns>Pre purge time of the boiler.</returns>
        public TimeSpan GetPrePurgeTime();
    }
}
