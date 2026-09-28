using BoilerSystem.Core.Constants;

namespace BoilerSystem.Core.Model
{
    public class Boiler
    {
        public Boiler()
        {
            PrePurgeTime = TimeSpan.FromSeconds(10);
            IgnitionTime = TimeSpan.FromSeconds(10);
            IsResetDone = false;
        }

        /// <summary>
        /// Gets the pre-purge time. 
        /// </summary>
        public TimeSpan PrePurgeTime { get; set; }

        /// <summary>
        /// Gets the ignition time.
        /// </summary>
        public TimeSpan IgnitionTime { get; init; }

        /// <summary>
        /// Gets ot sets the boiler status.
        /// </summary>
        public BoilerStatus BoilerStatus { get; set; }

        /// <summary>
        /// Get or sets the switch status.
        /// </summary>
        public SwitchStatus SwitchStatus { get; set; }

        /// <summary>
        /// Gets or sets the reset flag.
        /// </summary>
        public bool IsResetDone { get; set; }
    }
}
