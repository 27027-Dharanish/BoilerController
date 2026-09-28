using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerSystem.Core.Constants
{
    /// <summary>
    /// The boiler status.
    /// </summary>
    public enum BoilerStatus
    {
        /// <summary>
        /// Specifies the lockout state.
        /// </summary>
        Lockout = 1,
        /// <summary>
        /// Specifies the ready state.
        /// </summary>
        Ready,
        /// <summary>
        /// Specifies the Pre purge state.
        /// </summary>
        PrePurge,
        /// <summary>
        /// Specifies the ignition state.
        /// </summary>
        Ignition,
        /// <summary>
        /// Specifies the operational state.
        /// </summary>
        Operational,
    }
    /// <summary>
    /// The switch status.
    /// </summary>
    public enum SwitchStatus
    {
        /// <summary>
        /// Specifies switch is open.
        /// </summary>
        Open = 1,
        /// <summary>
        /// Specifies switch is close.
        /// </summary>
        Closed,
    }
}
