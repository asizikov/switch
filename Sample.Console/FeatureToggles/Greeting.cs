using Switch;
using Switch.Configuration;

namespace Sample.Console.FeatureToggles
{
    public class Greeting : Feature
    {
        public string GreetingText { get; set; }
        public bool Boolean { get; set; }

        [NotASetting]
        public string ShoulNotBeLoaded { get; set; }
    }
}