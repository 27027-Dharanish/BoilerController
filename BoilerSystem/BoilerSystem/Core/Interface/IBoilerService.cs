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
        /// 
        /// </summary>
        /// <returns></returns>
        public Task StartBoilerAsync();

        public void StopBoiler();

        public void StimulateBoilerError();

        public void ToggleSwitch();
        public void ResetBoiler();
    }
}
