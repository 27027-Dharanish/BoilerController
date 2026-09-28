using BoilerSystem.Core.Constants;
using BoilerSystem.Core.Model;

namespace BoilerSystem.Repository
{
    /// <summary>
    /// Provides a centralized data repository for storing, retrieving boiler entities.
    /// </summary>
    public class BoilerRepository
    {
        private readonly Boiler _boiler;
        private readonly object _locker;
        public BoilerRepository()
        {
            this._boiler = new Boiler();
            this._locker = new object();
        }
        public void ChangeBoilerStatus(BoilerStatus status)
        {
            lock(_locker)
            {
                this._boiler.BoilerStatus = status;
            }
        }
        public void ChangeSwitchStatus(SwitchStatus status)
        {
            lock(_locker)
            {
                this._boiler.SwitchStatus = status;
            }
        }
    }
}
