using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerSystem.Core.Interface
{
    public interface IBoilerService
    {
        /// <summary>
        /// Handle the initial setup of the boiler.
        /// </summary>
        public void InitialSetUp();

        /// <summary>
        /// Start the boiler sequential process.
        /// </summary>
        /// <returns>Task of boiler action.</returns>
        public Task StartBoilerAsync();

        /// <summary>
        /// Stop the boiler from execution.
        /// </summary>
        public void StopBoiler();

        /// <summary>
        /// Stimulate boiler error.
        /// </summary>
        public void StimulateBoilerError();

        /// <summary>
        /// Toggle the switch status.
        /// </summary>
        public void ToggleSwitch();

        /// <summary>
        /// Reset the boiler.
        /// </summary>
        public void ResetBoiler();
    }
}
