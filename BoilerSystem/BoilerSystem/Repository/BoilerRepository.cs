using BoilerSystem.Core.Constants;
using BoilerSystem.Core.Interface;
using BoilerSystem.Core.Model;

namespace BoilerSystem.Repository
{
    /// <summary>
    /// Provides a centralized data repository for storing, retrieving boiler entities.
    /// </summary>
    public class BoilerRepository
        : IBoilerRepository
    {
        private readonly Boiler _boiler;
        private readonly object _locker;
        /// <summary>
        /// Todo : Summary
        /// </summary>
        public BoilerRepository()
        {
            this._boiler = new Boiler();
            this._locker = new object();
        }

        /// <inheritdoc>
        public void SetBoilerStatus(BoilerStatus status)
        {
            lock(_locker)
            {
                this._boiler.BoilerStatus = status;
            }
        }
        public void SetSwitchStatus(SwitchStatus status)
        {
            lock(_locker)
            {
                this._boiler.SwitchStatus = status;
            }
        }
        public BoilerStatus GetBoilerStatus()
        {
            lock (_locker)
            {
                return this._boiler.BoilerStatus;
            }
        }
        public SwitchStatus GetSwitchStatus()
        {
            lock (_locker)
            {
                return this._boiler.SwitchStatus;
            }
        }
        public void SetResetDone(bool flag)
        {
            lock(_locker)
            {
                this._boiler.IsResetDone = flag;
            }
        }
        public bool IsResetDone()
        {
            lock (_locker)
            {
                return this._boiler.IsResetDone;
            }
        }
        public TimeSpan GetIgnitionTime()
        {
            return this._boiler.IgnitionTime;
        }
        public TimeSpan GetPrePurgeTime()
        {
            return this._boiler.PrePlugeTime;
        }
    }
}
