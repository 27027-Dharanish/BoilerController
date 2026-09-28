using BoilerSystem.Core.Constants;

namespace BoilerSystem.Core.Model
{
    public class Boiler
    {
        public Boiler()
        {
            PrePlugeTime = TimeSpan.FromSeconds(10);
            IgnitionTime = TimeSpan.FromSeconds(10);
        }
        public TimeSpan PrePlugeTime { get; set; }
        public TimeSpan IgnitionTime { get; set; }
        public BoilerStatus BoilerStatus { get; set; }
        public SwitchStatus SwitchStatus { get; set; }
    }
}
