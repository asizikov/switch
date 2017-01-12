using System.Configuration;

namespace Switch
{
    public class ConfigurationParameterReader
    {
        public string BaseKey { get; }

        public ConfigurationParameterReader(string baseKey)
        {
            BaseKey = baseKey;
        }

        public string Key(string key) => ConfigurationManager.AppSettings[$"{BaseKey}:{key}"];
    }
}